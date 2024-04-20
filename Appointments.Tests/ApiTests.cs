using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;

namespace Appointments.Tests
{
    public class ApiTests : IClassFixture<AppointmentsFactory>
    {
        private readonly HttpClient _client;
        private readonly AppointmentsDbContext _dbContext;

        public ApiTests(AppointmentsFactory factory)
        {
            _client = factory.CreateClient();
            _dbContext = factory.CreateAppointmentsDbContext();
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
            var response = await _client.PostAsJsonAsync("/api/appointments", new
            {
                Title = "Test",
            });

            response.EnsureSuccessStatusCode();

            var dbEntry = await _dbContext.Appointments.FirstOrDefaultAsync(appointment => appointment.Title == "Test");

            Assert.NotNull(dbEntry);
        }
    }
}