using Microsoft.AspNetCore.Mvc;
using Moq;
using UrbanIndicatorsSystem.Controllers;
using UrbanIndicatorsSystem.Models;
using UrbanIndicatorsSystem.Services;
using Xunit;

namespace UrbanIndicatorsSystem.Tests
{
    public class TestAreaController
    {
        private readonly Mock<IAreaService> _mockAreaService;
        private readonly AreaController _controller;

        public TestAreaController()
        {
            _mockAreaService = new Mock<IAreaService>();
            _controller = new AreaController(_mockAreaService.Object);
        }


        [Fact]
        public void Constructor_InitializesAreaService()
        {
            // Arrange & Act
            var controller = new AreaController(_mockAreaService.Object);

            // Assert
            Assert.NotNull(controller);
        }
    
        [Fact]
        public void GetAreas_ReturnsOkResult_WithAreas()
        {
            
            var expectedAreas = new List<Area>
            {
                new Area { Id = 1, Name = "Test Area" }
            };
            _mockAreaService.Setup(s => s.GetAreas()).Returns(expectedAreas);

            
            var result = _controller.GetAreas();

            
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedAreas, okResult.Value);
            _mockAreaService.Verify(s => s.GetAreas(), Times.Once);
        }
    }
}