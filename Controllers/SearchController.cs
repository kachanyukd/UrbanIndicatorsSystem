using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrbanIndicatorsSystem.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace UrbanIndicatorsSystem.Controllers
{
    [ApiController]
    [Route("api/traffic/search")]
    public class SearchController : ControllerBase
    {
        private readonly TrafficDbContext _db;

        public SearchController(TrafficDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Search(
            [FromQuery] string? areaIds,           
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromQuery] string? startsWith,
            [FromQuery] string? endsWith,
            [FromQuery] string? roadName,        
            [FromQuery] string? trafficLevel
        )
        {
            var query = _db.TrafficData
                .Include(t => t.Area)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(roadName))
            {
                var rn = roadName.Trim().ToLower();
                query = query.Where(t => t.RoadName.ToLower() == rn);
            }

            if (!string.IsNullOrWhiteSpace(trafficLevel))
            {
                var tl = trafficLevel.Trim().ToLower();
                query = query.Where(t => t.TrafficLevel.ToLower() == tl);
            }

            if (from.HasValue)
                query = query.Where(t => t.Timestamp >= from.Value);
            if (to.HasValue)
                query = query.Where(t => t.Timestamp <= to.Value);

            if (!string.IsNullOrWhiteSpace(startsWith))
            {
                var sw = startsWith.Trim().ToLower();
                query = query.Where(t => t.RoadName.ToLower().StartsWith(sw));
            }

            if (!string.IsNullOrWhiteSpace(endsWith))
            {
                var ew = endsWith.Trim().ToLower();
                query = query.Where(t => t.RoadName.ToLower().EndsWith(ew));
            }

            if (!string.IsNullOrWhiteSpace(areaIds))
            {
                var ids = areaIds.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                 .Select(s => int.Parse(s.Trim()))
                                 .ToList();
                query = query.Where(t => ids.Contains(t.AreaId));
            }

            var result = await query
                .OrderByDescending(t => t.Timestamp)
                .ToListAsync();

            return Ok(result);
        }
    }
}