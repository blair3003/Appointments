using System.Net;

namespace Appointments.Tests
{
    public class ApiTests : IClassFixture<AppointmentsFactory>
    {
        private readonly HttpClient _client;

        public ApiTests(AppointmentsFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Should_return_200_ok_When_get_appointments()
        {
            var response = await _client.GetAsync("/api/appointments");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}