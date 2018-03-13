﻿using System.Threading.Tasks;
using losol.EventManagement.Domain;

namespace losol.EventManagement.Services
{
	public interface IRegistrationService
	{
		Task<Registration> GetAsync(int id);
		Task<Registration> GetAsync(string userId, int eventId);
		Task<Registration> GetWithEventInfoAsync(int id);

		Task<int> CreateRegistrationForUser(Registration registration);
		Task<int> SetRegistrationAsVerified(int id);
	}
}