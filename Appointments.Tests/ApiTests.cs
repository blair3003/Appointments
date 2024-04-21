using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Tests
{
    public class ApiTests : IClassFixture<AppointmentsFactory>
    {
        private readonly HttpClient _client;
        private readonly AppointmentsFactory _factory;

        public ApiTests(AppointmentsFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppointmentsDbContext>();
            _factory.InitializeDbForTests(dbContext);
        }

        private AppointmentsDbContext GetDbContext()
        {
            var scope = _factory.Services.CreateScope();
            return scope.ServiceProvider.GetRequiredService<AppointmentsDbContext>();
        }

        [Fact]
        public async Task Should_return_200_ok_When_get_appointments()
        {
            var response = await _client.GetAsync("/api/appointments");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Should_add_to_storage_When_post_new_appointment()
        {
            var appointmentData = new
            {
                Title = "Team Meeting",
                Description = "Quarterly planning meeting",
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddHours(2),
                IsAllDay = false
            };

            var response = await _client.PostAsJsonAsync("/api/appointments", appointmentData);
            response.EnsureSuccessStatusCode();

            using var dbContext = GetDbContext();

            var dbEntry = await dbContext.Appointments.FirstOrDefaultAsync(
                appointment => appointment.Title == appointmentData.Title);

            Assert.NotNull(dbEntry);
            Assert.Equal(appointmentData.Title, dbEntry.Title);
            Assert.Equal(appointmentData.Description, dbEntry.Description);
            Assert.Equal(appointmentData.StartTime, dbEntry.StartTime, TimeSpan.FromSeconds(1));
            Assert.Equal(appointmentData.EndTime, dbEntry.EndTime, TimeSpan.FromSeconds(1));
            Assert.Equal(appointmentData.IsAllDay, dbEntry.IsAllDay);
        }
    }
}
