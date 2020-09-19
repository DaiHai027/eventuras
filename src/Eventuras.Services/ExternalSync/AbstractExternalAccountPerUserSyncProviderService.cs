using Eventuras.Domain;
using Eventuras.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Eventuras.Services.ExternalSync
{
    /// <summary>
    /// Manages one external account per user, in contrast
    /// with having one external account per registration.
    /// </summary>
    public abstract class AbstractExternalAccountPerUserSyncProviderService : AbstractExternalSyncProviderService
    {
        private readonly ApplicationDbContext _context;

        protected AbstractExternalAccountPerUserSyncProviderService(ApplicationDbContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public override async Task<ExternalAccount> FindExistingAccountAsync(Registration registration)
        {
            if (registration == null)
            {
                throw new ArgumentNullException(nameof(registration));
            }

            return await _context.ExternalAccounts
                .FirstOrDefaultAsync(a => a.ExternalServiceName == Name &&
                                          a.UserId == registration.UserId);
        }
    }
}
