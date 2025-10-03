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
    }
}