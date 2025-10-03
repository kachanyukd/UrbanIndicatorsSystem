using System;
using System.Linq;
using UrbanIndicatorsSystem.Models;
using UrbanIndicatorsSystem.Services;
using Xunit;

namespace UrbanIndicatorsSystem.Tests
{
    public class TestTrafficService
    {
        private readonly TrafficService _service;

        public TestTrafficService()
        {
            _service = new TrafficService();
        }

        [Fact]
        public void Constructor_InitializesTrafficDataWithSixRoads()
        {
            
            var result = _service.GetTrafficData();

            
            Assert.NotNull(result);
            Assert.Equal(6, result.Count);
        }

        [Fact]
        public void GetTrafficData_ReturnsCorrectRoadNames()
        {
            
            var expectedRoads = new[] { "Shevchenkivskyi", "Pechersk", "Holosiivskyi", "Solomyanskiy", "Darnytskiy", "Obolonskiy" };

            
            var result = _service.GetTrafficData();

            
            Assert.All(expectedRoads, roadName => 
                Assert.Contains(result, traffic => traffic.RoadName == roadName));
        }

        [Fact]
        public void GetTrafficData_ReturnsInitialTrafficLevels()
        {
            
            var result = _service.GetTrafficData();

            
            Assert.Contains(result, t => t.RoadName == "Shevchenkivskyi" && t.TrafficLevel == "Low");
            Assert.Contains(result, t => t.RoadName == "Pechersk" && t.TrafficLevel == "Comfortable");
            Assert.Contains(result, t => t.RoadName == "Holosiivskyi" && t.TrafficLevel == "Moderate");
            Assert.Contains(result, t => t.RoadName == "Solomyanskiy" && t.TrafficLevel == "High");
            Assert.Contains(result, t => t.RoadName == "Darnytskiy" && t.TrafficLevel == "Medium");
            Assert.Contains(result, t => t.RoadName == "Obolonskiy" && t.TrafficLevel == "Comfortable");
        }

    }
}