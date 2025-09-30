using Microsoft.AspNetCore.Mvc;
using UrbanIndicatorsSystem.Services;

namespace UrbanIndicatorsSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AreaController : ControllerBase
    {
        private readonly IAreaService _areaService;

        public AreaController(IAreaService areaService)
        {
            _areaService = areaService;
        }

        [HttpGet]
        public IActionResult GetAreas()
        {
            return Ok(_areaService.GetAreas());
        }
    }
}
