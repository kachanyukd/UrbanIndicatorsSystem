using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrbanIndicatorsSystem.Data;
using UrbanIndicatorsSystem.Models;

namespace UrbanIndicatorsSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AreaController : ControllerBase
    {
        private readonly TrafficDbContext _context;

        public AreaController(TrafficDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Area>>> GetAll()
        {
            return await _context.Areas.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Area>> GetById(int id)
        {
            var area = await _context.Areas.FindAsync(id);
            if (area == null) return NotFound();
            return area;
        }

        [HttpPost]
        public async Task<ActionResult<Area>> Create(Area area)
        {
            _context.Areas.Add(area);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = area.Id }, area);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Area area)
        {
            if (id != area.Id) return BadRequest();
            _context.Entry(area).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var area = await _context.Areas.FindAsync(id);
            if (area == null) return NotFound();
            _context.Areas.Remove(area);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}