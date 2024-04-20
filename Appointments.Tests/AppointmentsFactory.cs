using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointments.Tests
{
    public class AppointmentsFactory : WebApplicationFactory<IApiAssemblyMarker>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddDbContextFactory<AppointmentsDbContext>(options =>
                {
                    options.UseInMemoryDatabase("Appointments");
                });
            });
        }

        public AppointmentsDbContext CreateAppointmentsDbContext()
        {
            var db = Services.GetRequiredService<IDbContextFactory<AppointmentsDbContext>>().CreateDbContext();
            db.Database.EnsureCreated();
            return db;
        }
    }
}
