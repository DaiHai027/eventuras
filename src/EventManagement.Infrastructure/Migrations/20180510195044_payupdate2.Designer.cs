﻿// <auto-generated />
using losol.EventManagement.Domain;
using losol.EventManagement.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace losol.EventManagement.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180510195044_payupdate2")]
    partial class payupdate2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("losol.EventManagement.Domain.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<bool>("Archived");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("SignatureImageBase64");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("losol.EventManagement.Domain.Certificate", b =>
                {
                    b.Property<int>("CertificateId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("Auth");

                    b.Property<Guid>("CertificateGuid");

                    b.Property<string>("Description");

                    b.Property<string>("EvidenceDescription");

                    b.Property<string>("IssuedByName");

                    b.Property<DateTime>("IssuedDate");

                    b.Property<string>("IssuedInCity");

                    b.Property<int?>("IssuingOrganizationId");

                    b.Property<string>("IssuingOrganizationName");

                    b.Property<string>("IssuingUserId");

                    b.Property<string>("RecipientEmail");

                    b.Property<string>("RecipientName");

                    b.Property<string>("RecipientUserId");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("CertificateId");

                    b.HasIndex("IssuingOrganizationId");

                    b.HasIndex("IssuingUserId");

                    b.HasIndex("RecipientUserId");

                    b.ToTable("Certificates");
                });

            modelBuilder.Entity("losol.EventManagement.Domain.CertificateEvidence", b =>
                {
                    b.Property<int>("CertificateEvidenceId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CertificateId");

                    b.Property<int>("RegistrationId");

                    b.HasKey("CertificateEvidenceId");

                    b.HasIndex("CertificateId");

                    b.HasIndex("RegistrationId");

                    b.ToTable("CertificateEvidences");
                });

            modelBuilder.Entity("losol.EventManagement.Domain.EventInfo", b =>
                {
                    b.Property<int>("EventInfoId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Category");

                    b.Property<string>("CertificateDescription");

                    b.Property<string>("CertificateTitle");

                    b.Property<string>("City");

                    b.Property<string>("Code")
                        .IsRequired();

                    b.Property<DateTime?>("DateEnd");

                    b.Property<DateTime?>("DateStart");

                    b.Property<string>("Description")
                        .HasMaxLength(300);

                    b.Property<bool>("Featured");

                    b.Property<string>("FeaturedImageCaption");

                    b.Property<string>("FeaturedImageUrl");

                    b.Property<string>("InformationRequest");

                    b.Property<DateTime?>("LastCancellationDate");

                    b.Property<DateTime?>("LastRegistrationDate");

                    b.Property<string>("Location");

                    b.Property<bool>("ManageRegistrations");

                    b.Property<int>("MaxParticipants");

                    b.Property<string>("MoreInformation");

                    b.Property<bool>("OnDemand");

                    b.Property<int?>("OrganizationId");

                    b.Property<string>("OrganizerUserId");

                    b.Property<string>("PracticalInformation");

                    b.Property<decimal?>("Price");

                    b.Property<string>("Program");

                    b.Property<string>("ProjectCode");

                    b.Property<bool>("Published");

                    b.Property<string>("RegistrationsUrl");

                    b.Property<string>("Title");

                    b.Property<string>("WelcomeLetter");

                    b.HasKey("EventInfoId");

                    b.HasIndex("OrganizationId");

                    b.HasIndex("OrganizerUserId");

                    b.ToTable("EventInfos");
                });

            modelBuilder.Entity("losol.EventManagement.Domain.MessageLog", b =>
                {
                    b.Property<int>("MessageLogId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("EventInfoId");

                    b.Property<string>("MessageContent");

                    b.Property<string>("MessageType");

                    b.Property<string>("Provider");

                    b.Property<string>("Recipients");

                    b.Property<string>("Result");

                    b.Property<DateTime>("TimeStamp");

                    b.HasKey("MessageLogId");

                    b.HasIndex("EventInfoId");

                    b.ToTable("MessageLogs");
                });

            modelBuilder.Entity("losol.EventManagement.Domain.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comments");

                    b.Property<string>("CustomerEmail");

                    b.Property<string>("CustomerInvoiceReference");

                    b.Property<string>("CustomerName");

                    b.Property<string>("CustomerVatNumber");

                    b.Property<string>("ExternalInvoiceId");

                    b.Property<string>("Log");

                    b.Property<DateTime>("OrderTime");

                    b.Property<bool>("Paid");

                    b.Property<int?>("PaymentMethodId");

                    b.Property<int>("RegistrationId");

                    b.Property<int>("Status");

                    b.Property<string>("UserId");

                    b.HasKey("OrderId");

                    b.HasIndex("PaymentMethodId");

                    b.HasIndex("RegistrationId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("losol.EventManagement.Domain.OrderLine", b =>
                {
                    b.Property<int>("OrderLineId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comments");

                    b.Property<int>("OrderId");

                    b.Property<decimal>("Price");

                    b.Property<string>("ProductDescription");

                    b.Property<int?>("ProductId");

                    b.Property<string>("ProductName");

                    b.Property<string>("ProductVariantDescription");

                    b.Property<int?>("ProductVariantId");

                    b.Property<string>("ProductVariantName");

                    b.Property<int>("Quantity");

                    b.Property<int?>("RefundOrderId");

                    b.Property<decimal>("VatPercent");

                    b.HasKey("OrderLineId");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.HasIndex("ProductVariantId");

                    b.HasIndex("RefundOrderId");

                    b.ToTable("OrderLines");
                });

            modelBuilder.Entity("losol.EventManagement.Domain.Organization", b =>
                {
                    b.Property<int>("OrganizationId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountNumber");

                    b.Property<string>("Description")
                        .HasMaxLength(300);

                    b.Property<string>("Email")
                        .HasMaxLength(300);

                    b.Property<string>("LogoBase64");

                    b.Property<string>("LogoUrl")
                        .HasMaxLength(300);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Phone")
                        .HasMaxLength(100);

                    b.Property<string>("Url")
                        .HasMaxLength(300);

                    b.Property<string>("VatId");

                    b.HasKey("OrganizationId");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("losol.EventManagement.Domain.PaymentMethod", b =>
                {
                    b.Property<int>("PaymentMethodId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<bool>("AdminOnly");

                    b.Property<string>("Code");

                    b.Property<bool>("IsDefault");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(75);

                    b.Property<int>("Provider");

                    b.HasKey("PaymentMethodId");

                    b.ToTable("PaymentMethods");
                });

            modelBuilder.Entity("losol.EventManagement.Domain.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasMaxLength(300);

                    b.Property<bool>("EnableQuantity");

                    b.Property<int>("EventInfoId");

                    b.Property<int>("Inventory");

                    b.Property<int>("MinimumQuantity");

                    b.Property<string>("MoreInformation");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<decimal>("Price");

                    b.Property<bool>("Published");

                    b.Property<int>("VatPercent");

                    b.HasKey("ProductId");

                    b.HasIndex("EventInfoId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("losol.EventManagement.Domain.ProductVariant", b =>
                {
                    b.Property<int>("ProductVariantId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("AdminOnly");

                    b.Property<string>("Description")
                        .HasMaxLength(300);

                    b.Property<int>("Inventory");

                    b.Property<string>("Name");

                    b.Property<decimal>("Price");

                    b.Property<int>("ProductId");

                    b.Property<bool>("Published");

                    b.Property<int>("VatPercent");

                    b.HasKey("ProductVariantId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductVariants");
                });

            modelBuilder.Entity("losol.EventManagement.Domain.Registration", b =>
                {
                    b.Property<int>("RegistrationId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CertificateId");

                    b.Property<string>("CustomerEmail");

                    b.Property<string>("CustomerInvoiceReference");

                    b.Property<string>("CustomerName");

                    b.Property<string>("CustomerVatNumber");

                    b.Property<bool>("Diploma");

                    b.Property<int>("EventInfoId");

                    b.Property<bool>("FreeRegistration");

                    b.Property<string>("Log");

                    b.Property<string>("Notes");

                    b.Property<string>("ParticipantCity");

                    b.Property<string>("ParticipantEmployer");

                    b.Property<string>("ParticipantJobTitle");

                    b.Property<string>("ParticipantName");

                    b.Property<int?>("PaymentMethodId");

                    b.Property<string>("RegistrationBy");

                    b.Property<DateTime?>("RegistrationTime");

                    b.Property<int>("Status");

                    b.Property<int>("Type");

                    b.Property<string>("UserId");

                    b.Property<string>("VerificationCode");

                    b.Property<bool>("Verified");

                    b.HasKey("RegistrationId");

                    b.HasIndex("CertificateId");

                    b.HasIndex("EventInfoId");

                    b.HasIndex("PaymentMethodId");

                    b.HasIndex("UserId");

                    b.ToTable("Registrations");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("losol.EventManagement.Domain.Certificate", b =>
                {
                    b.HasOne("losol.EventManagement.Domain.Organization", "IssuingOrganization")
                        .WithMany()
                        .HasForeignKey("IssuingOrganizationId");

                    b.HasOne("losol.EventManagement.Domain.ApplicationUser", "IssuingUser")
                        .WithMany()
                        .HasForeignKey("IssuingUserId");

                    b.HasOne("losol.EventManagement.Domain.ApplicationUser", "RecipientUser")
                        .WithMany()
                        .HasForeignKey("RecipientUserId");
                });

            modelBuilder.Entity("losol.EventManagement.Domain.CertificateEvidence", b =>
                {
                    b.HasOne("losol.EventManagement.Domain.Certificate")
                        .WithMany("Evidence")
                        .HasForeignKey("CertificateId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("losol.EventManagement.Domain.Registration", "Registration")
                        .WithMany()
                        .HasForeignKey("RegistrationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("losol.EventManagement.Domain.EventInfo", b =>
                {
                    b.HasOne("losol.EventManagement.Domain.Organization", "Organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId");

                    b.HasOne("losol.EventManagement.Domain.ApplicationUser", "OrganizerUser")
                        .WithMany()
                        .HasForeignKey("OrganizerUserId");
                });

            modelBuilder.Entity("losol.EventManagement.Domain.MessageLog", b =>
                {
                    b.HasOne("losol.EventManagement.Domain.EventInfo", "EventInfo")
                        .WithMany()
                        .HasForeignKey("EventInfoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("losol.EventManagement.Domain.Order", b =>
                {
                    b.HasOne("losol.EventManagement.Domain.PaymentMethod", "PaymentMethod")
                        .WithMany()
                        .HasForeignKey("PaymentMethodId");

                    b.HasOne("losol.EventManagement.Domain.Registration", "Registration")
                        .WithMany("Orders")
                        .HasForeignKey("RegistrationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("losol.EventManagement.Domain.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("losol.EventManagement.Domain.OrderLine", b =>
                {
                    b.HasOne("losol.EventManagement.Domain.Order", "Order")
                        .WithMany("OrderLines")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("losol.EventManagement.Domain.Product", "Product")
                        .WithMany("OrderLines")
                        .HasForeignKey("ProductId");

                    b.HasOne("losol.EventManagement.Domain.ProductVariant", "ProductVariant")
                        .WithMany("OrderLines")
                        .HasForeignKey("ProductVariantId");

                    b.HasOne("losol.EventManagement.Domain.Order", "RefundOrder")
                        .WithMany()
                        .HasForeignKey("RefundOrderId");
                });

            modelBuilder.Entity("losol.EventManagement.Domain.Product", b =>
                {
                    b.HasOne("losol.EventManagement.Domain.EventInfo", "Eventinfo")
                        .WithMany("Products")
                        .HasForeignKey("EventInfoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("losol.EventManagement.Domain.ProductVariant", b =>
                {
                    b.HasOne("losol.EventManagement.Domain.Product", "Product")
                        .WithMany("ProductVariants")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("losol.EventManagement.Domain.Registration", b =>
                {
                    b.HasOne("losol.EventManagement.Domain.Certificate", "Certificate")
                        .WithMany()
                        .HasForeignKey("CertificateId");

                    b.HasOne("losol.EventManagement.Domain.EventInfo", "EventInfo")
                        .WithMany("Registrations")
                        .HasForeignKey("EventInfoId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("losol.EventManagement.Domain.PaymentMethod", "PaymentMethod")
                        .WithMany()
                        .HasForeignKey("PaymentMethodId");

                    b.HasOne("losol.EventManagement.Domain.ApplicationUser", "User")
                        .WithMany("Registrations")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("losol.EventManagement.Domain.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("losol.EventManagement.Domain.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("losol.EventManagement.Domain.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("losol.EventManagement.Domain.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
