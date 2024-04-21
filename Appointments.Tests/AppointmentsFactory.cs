using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Tests
{
    public class AppointmentsFactory : WebApplicationFactory<IApiAssemblyMarker>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                // Remove the existing DbContext configuration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppointmentsDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add a new DbContext with SQLite for testing
                services.AddDbContext<AppointmentsDbContext>(options =>
                    options.UseSqlite("Filename=TestAppointments.db"), ServiceLifetime.Scoped);
            });
        }

        public void InitializeDbForTests(AppointmentsDbContext db)
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }
    }
}
