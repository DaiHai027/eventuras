﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using losol.EventManagement.Domain;
using losol.EventManagement.Pages.Account;
using losol.EventManagement.Services;
using losol.EventManagement.ViewModels;
using losol.EventManagement.Web.Services;
using System.Text;

namespace losol.EventManagement.Web.Pages.Events.Register
{
	public class EventRegistrationModel : PageModel
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ConfirmationEmailSender _confirmationEmailSender;// TODO REMOVE?
		private readonly StandardEmailSender _standardEmailSender; // TODO REMOVE?
		private readonly ILogger<EventRegistrationModel> _logger;
		private readonly IEventInfoService _eventsService;
		private readonly IPaymentMethodService _paymentMethodService;
		private readonly IRegistrationService _registrationService;
		private readonly RegistrationEmailSender _registrationEmailSender;

		public EventRegistrationModel(
			UserManager<ApplicationUser> userManager,
			ConfirmationEmailSender confirmationEmailSender, // REMOVE?
			StandardEmailSender standardEmailSender, // REMOVE?
			RegistrationEmailSender registrationEmailSender,
			ILogger<EventRegistrationModel> logger,
			IEventInfoService eventsService,
			IPaymentMethodService paymentMethodService,
			IRegistrationService registrationService
		)
		{
			_userManager = userManager;
			_confirmationEmailSender = confirmationEmailSender;
			_standardEmailSender = standardEmailSender;
			_logger = logger;
			_eventsService = eventsService;
			_paymentMethodService = paymentMethodService;
			_registrationService = registrationService;
			_registrationEmailSender = registrationEmailSender;
		}

		[BindProperty]
		public RegisterVM Registration { get; set; }
		public EventInfo EventInfo { get; set; }
		public List<PaymentMethod> PaymentMethods { get; set; }
		public List<Product> Products => EventInfo.Products;
		public int DefaultPaymentMethod => _paymentMethodService.GetDefaultPaymentMethodId();

		public async Task<IActionResult> OnGetAsync(int id)
		{

			EventInfo = await _eventsService.GetWithProductsAsync(id);

			if (EventInfo == null)
			{
				return NotFound();
			}

			PaymentMethods = await _paymentMethodService.GetActivePaymentMethodsAsync();
			Registration = new RegisterVM(EventInfo, DefaultPaymentMethod);

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int id)
		{

			if (!ModelState.IsValid)
			{
				PaymentMethods = await _paymentMethodService.GetActivePaymentMethodsAsync();
				return Page();
			}

			EventInfo = await _eventsService.GetWithProductsAsync(id);
			if (EventInfo == null) return NotFound();
			Registration.EventInfoId = EventInfo.EventInfoId;

			// Check if user exists with email registered
			var user = await _userManager.FindByEmailAsync(Registration.Email);
			if (user != null)
			{
				Registration.UserId = user.Id;
				_logger.LogInformation("Found existing user.");
				// Any registrations for this user on this event?
				var registration = await _registrationService.GetAsync(user.Id, Registration.EventInfoId);
				if (registration != null)
				{
					// The user has already registered for the event.
					await _registrationEmailSender.SendRegistrationAsync(user.Email, "Velkommen på kurs!", registration.RegistrationId);
					return RedirectToPage("/Info/EmailSent");		
				}
			}
			else
			{
				// Create new user
				var newUser = new ApplicationUser { UserName = Registration.Email, Name = Registration.ParticipantName, Email = Registration.Email, PhoneNumber = (Registration.PhoneCountryCode + Registration.Phone) };
				var result = await _userManager.CreateAsync(newUser);

				if (result.Succeeded)
				{
					_logger.LogInformation("User created a new account with password.");

					Registration.UserId = newUser.Id;
					user = newUser;
				}
				else
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}	
					PaymentMethods = await _paymentMethodService.GetActivePaymentMethodsAsync();
					return Page();
				}
			}

			// If we came here, we should enter our new participant into the database!
			_logger.LogInformation("Starting new registration:");

			var newRegistration = Registration.Adapt<Registration>();
			newRegistration.VerificationCode = Guid.NewGuid().ToString();
			int[] selectedProductIds = null;
			int[] selectedVariantIds = null;

			// If the eventinfo has products, then register and make order
			if (Registration.HasProducts)
			{
				// Populate the ids required to create the registration
				selectedProductIds = Registration.SelectedProducts.ToArray();
				selectedVariantIds = Registration.SelectedVariants.ToArray();
				
				var registeredProducts = EventInfo.Products
					.Where(x => selectedProductIds.Contains(x.ProductId))
					.ToList();

				// Get the list of product names along with id and variants ...
				var productNames = registeredProducts.Select(product =>
				{
					var variantId = Registration.Products
						.Where(p => product.ProductId == p.Value)
						.Select(p => p.SelectedVariantId)
						.FirstOrDefault();

					var variantString = "";
					if (variantId != null)
					{
						var variantName = Products.Where(p => p.ProductId == product.ProductId)
							.SelectMany(p => p.ProductVariants)
							.Where(v => v.ProductVariantId == variantId)
							.Select(v => v.Name)
							.Single();

						variantString = $" ({variantId}. {variantName})";
					}

					return $"{product.ProductId}. {product.Name}{variantString}";
				});

				// ... and concatenate them together into the notes field
				Registration.Notes = String.Join(", ", productNames);
			}
			await _registrationService.CreateRegistration(newRegistration, selectedProductIds, selectedVariantIds);
			await _registrationEmailSender.SendRegistrationAsync(user.Email, "Velkommen på kurs!", newRegistration.RegistrationId);

			return RedirectToPage("/Info/EmailSent");
		}

		private async Task<IActionResult> userAlreadyRegisteredResult(Registration registration)
		{
			// Prepare an email to send out
			var emailVM = new EmailMessage()
			{
				Name = Registration.ParticipantName,
				Email = Registration.Email,
				Subject = "Du var allerede påmeldt!",
				Message = @"Vi hadde allerede registrert deg i systemet.
								Ta kontakt med ole@nordland-legeforening hvis du tror det er skjedd noe feil her!
								"
			};

			// If registered but not verified, just send reminder of verification. 
			if (registration.Verified == false)
			{
				var verificationUrl = Url.Action("Confirm", "Register", new { id = registration.RegistrationId, auth = registration.VerificationCode }, protocol: Request.Scheme);
				emailVM.Subject = "En liten bekreftelse bare...";
				emailVM.Message = $@"Vi hadde allerede registrert deg i systemet, men du har ikke bekreftet enda.
								<p><a href='{verificationUrl}'>Bekreft her</a></p>
								<p></p>
								<p>Hvis lenken ikke virker, så kan du kopiere inn teksten under i nettleseren:
								{verificationUrl} </p>";
			}

			await _standardEmailSender.SendAsync(emailVM);
			return RedirectToPage("/Info/EmailSent");
		}

	}
}
