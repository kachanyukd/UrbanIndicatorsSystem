using Microsoft.AspNetCore.Mvc;
using UrbanIndicatorsSystem.Services;
using Asp.Versioning;

namespace UrbanIndicatorsSystem.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
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
            try
            {
                var data = await _trafficService.GetTrafficData();
                
                if (data == null || !data.Any())
                {
                    return Ok(new List<object>());
                }
                
                // V1: повертаємо простий список
                var result = data.Select(d => new 
                { 
                    roadName = d.RoadName, 
                    trafficLevel = d.TrafficLevel 
                }).ToList();
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("simulate")]
        public async Task<IActionResult> Simulate()
        {
            try
            {
                await _trafficService.SimulateTraffic();
                return Ok(new { message = "Traffic data simulated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}