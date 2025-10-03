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



        [Fact]
        public void SimulateTraffic_UsesValidTrafficLevels()
        {

            var validLevels = new[] { "Low", "Comfortable", "Moderate", "Medium", "High" };


            _service.SimulateTraffic();
            var result = _service.GetTrafficData();


            Assert.All(result, traffic =>
                Assert.Contains(traffic.TrafficLevel, validLevels));
        }

        [Fact]
        public void SimulateTraffic_DoesNotChangeRoadNames()
        {

            var originalRoadNames = _service.GetTrafficData().Select(t => t.RoadName).ToList();


            _service.SimulateTraffic();
            var updatedRoadNames = _service.GetTrafficData().Select(t => t.RoadName).ToList();


            Assert.Equal(originalRoadNames, updatedRoadNames);
        }
        
        

        [Fact]
        public void GetTrafficData_ReturnsSameInstanceOnMultipleCalls()
        {
            
            var firstCall = _service.GetTrafficData();
            var secondCall = _service.GetTrafficData();

            
            Assert.Same(firstCall, secondCall);
        }
    }
}