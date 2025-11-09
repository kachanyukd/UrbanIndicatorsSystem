using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace UrbanIndicatorsSystem.Tests.IntegrationTests
{
    public class TrafficV2ControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public TrafficV2ControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetTrafficData_ReturnsSuccessStatusCode()
        {
            // Act
            var response = await _client.GetAsync("/api/v2.0/traffic");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetTrafficData_ReturnsDetailedFormat()
        {
            // Act
            var response = await _client.GetAsync("/api/v2.0/traffic");
            var data = await response.Content.ReadFromJsonAsync<List<TrafficV2Response>>();

            // Assert
            Assert.NotNull(data);
            Assert.NotEmpty(data);
            Assert.All(data, item =>
            {
                Assert.NotNull(item.roadName);
                Assert.NotNull(item.trafficLevel);
                Assert.True(item.id > 0);
                Assert.NotEqual(default(DateTime), item.timestamp);
            });
        }

        [Fact]
        public async Task GetTrafficData_IncludesAreaInformation()
        {
            // Act
            var response = await _client.GetAsync("/api/v2.0/traffic");
            var data = await response.Content.ReadFromJsonAsync<List<TrafficV2Response>>();

            // Assert
            Assert.NotNull(data);
            Assert.All(data, item =>
            {
                Assert.True(item.areaId > 0);
            });
        }

        private class TrafficV2Response
        {
            public int id { get; set; }
            public string roadName { get; set; } = string.Empty;
            public string trafficLevel { get; set; } = string.Empty;
            public DateTime timestamp { get; set; }
            public int areaId { get; set; }
            public string? area { get; set; }
        }
    }
}