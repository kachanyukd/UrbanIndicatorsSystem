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
    }
}