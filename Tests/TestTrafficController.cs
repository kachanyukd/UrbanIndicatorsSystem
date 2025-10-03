using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UrbanIndicatorsSystem.Controllers;
using UrbanIndicatorsSystem.Models;
using UrbanIndicatorsSystem.Services;
using Xunit;

namespace UrbanIndicatorsSystem.Tests
{
    public class TestTrafficController
    {
        private readonly Mock<ITrafficService> _mockTrafficService;
        private readonly TrafficController _controller;

        public TestTrafficController()
        {
            _mockTrafficService = new Mock<ITrafficService>();
            _controller = new TrafficController(_mockTrafficService.Object);
        }

        [Fact]
        public void Constructor_InitializesTrafficService()
        {
            var controller = new TrafficController(_mockTrafficService.Object);

            Assert.NotNull(controller);
        }

        [Fact]

        public void GetTrafficData_ReturnsOkResult_WithTrafficData()
        {
            var expectedTrafficData = new List<TrafficData>
            {
                new TrafficData { RoadName = "Test Road", TrafficLevel = "Low"}
            };
            _mockTrafficService.Setup(s => s.GetTrafficData()).Returns(expectedTrafficData);

            var result = _controller.GetTrafficData();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualTrafficData = Assert.IsAssignableFrom<IEnumerable<TrafficData>>(okResult.Value);
            Assert.Equal(expectedTrafficData, actualTrafficData);
            _mockTrafficService.Verify(s => s.GetTrafficData(), Times.Once);
        }

        [Fact]

        public void GetTrafficData_ReturnsOkResult_WithEmptyList_WhenNoData()
        {

            var emptyTrafficData = new List<TrafficData>();
            _mockTrafficService.Setup(s => s.GetTrafficData()).Returns(emptyTrafficData);

            var result = _controller.GetTrafficData();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualTrafficData = Assert.IsAssignableFrom<IEnumerable<TrafficData>>(okResult.Value);
            Assert.Empty(actualTrafficData);
            _mockTrafficService.Verify(s => s.GetTrafficData(), Times.Once);
        }
    }
}