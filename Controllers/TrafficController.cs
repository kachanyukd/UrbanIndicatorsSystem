using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public IActionResult GetTrafficData()
        {
            return Ok(_trafficService.GetTrafficData());
        }
    }
}
