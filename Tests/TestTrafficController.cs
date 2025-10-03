using Microsoft.AspNetCore.Mvc;
using Moq;
using UrbanIndicatorsSystem.Controllers;
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
    }
}