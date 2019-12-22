using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

using losol.EventManagement.Domain;
using losol.EventManagement.Infrastructure;
using losol.EventManagement.Services.DbInitializers;


namespace losol.EventManagement.IntegrationTests
{
    public class TestDbInitializer : BaseDbInitializer
    {
        private static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 1);

        public TestDbInitializer(ApplicationDbContext db, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IOptions<DbInitializerOptions> config)
            : base(db, roleManager, userManager, config)
        { }

        public override async Task SeedAsync()
        {
            await SemaphoreSlim.WaitAsync();
            try
            {
                if (_db.Database.EnsureCreated())
                {
                    await base.SeedAsync();
                    _db.EventInfos.AddRange(SeedData.Events);
                    await _db.SaveChangesAsync();
                }
            }
            finally
            {
                SemaphoreSlim.Release();
            }
        }
    }
}
