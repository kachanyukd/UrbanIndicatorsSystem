using Microsoft.AspNetCore.Mvc;
using UrbanIndicatorsSystem.Models;
using UrbanIndicatorsSystem.Services;

namespace UrbanIndicatorsSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrafficController : ControllerBase
    {
        private readonly ITrafficService _trafficService;

        public TrafficController(ITrafficService trafficService)
        {
            _trafficService = trafficService;
        }

        [HttpGet("{area}")]
        public IActionResult GetTraffic(string area)
        {
            var trafficData = _trafficService.GetTrafficForArea(area);
            return Ok(trafficData);
        }
    }
}
