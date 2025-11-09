using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace UrbanIndicatorsSystem.Tests.IntegrationTests
{
    public class TrafficV1ControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public TrafficV1ControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetTrafficData_ReturnsSuccessStatusCode()
        {
            // Act
            var response = await _client.GetAsync("/api/v1.0/traffic");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetTrafficData_ReturnsJsonContent()
        {
            // Act
            var response = await _client.GetAsync("/api/v1.0/traffic");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Contains("roadName", content);
            Assert.Contains("trafficLevel", content);
        }

        [Fact]
        public async Task GetTrafficData_ReturnsSimpleFormat()
        {
            // Act
            var response = await _client.GetAsync("/api/v1.0/traffic");
            var data = await response.Content.ReadFromJsonAsync<List<TrafficV1Response>>();

            // Assert
            Assert.NotNull(data);
            Assert.NotEmpty(data);
            Assert.All(data, item =>
            {
                Assert.NotNull(item.roadName);
                Assert.NotNull(item.trafficLevel);
            });
        }

        [Fact]
        public async Task SimulateTraffic_ReturnsSuccessStatusCode()
        {
            // Act
            var response = await _client.PostAsync("/api/v1.0/traffic/simulate", null);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        private class TrafficV1Response
        {
            public string roadName { get; set; } = string.Empty;
            public string trafficLevel { get; set; } = string.Empty;
        }
    }
}