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
            // Act
            var result = _service.GetTrafficData();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.Count);
        }
    }
}