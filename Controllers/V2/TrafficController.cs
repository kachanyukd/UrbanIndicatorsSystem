using Microsoft.AspNetCore.Mvc;
using UrbanIndicatorsSystem.Services;
using Asp.Versioning;

namespace UrbanIndicatorsSystem.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
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
                
                var result = data.Select(d => new 
                { 
                    id = d.Id,
                    roadName = d.RoadName, 
                    trafficLevel = d.TrafficLevel,
                    timestamp = d.Timestamp,
                    areaId = d.AreaId,
                    area = d.Area?.Name
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