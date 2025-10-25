using Microsoft.AspNetCore.Mvc;
using UrbanIndicatorsSystem.Services;
using System.Threading.Tasks;

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
        public async Task<IActionResult> GetTrafficData()
        {
            var data = await _trafficService.GetTrafficData();
            return Ok(data);
        }
    }
}