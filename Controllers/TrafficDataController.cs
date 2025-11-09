using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrbanIndicatorsSystem.Data;
using UrbanIndicatorsSystem.Models;

namespace UrbanIndicatorsSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrafficDataController : ControllerBase
    {
        private readonly TrafficDbContext _context;

        public TrafficDataController(TrafficDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrafficData>>> GetAll()
        {
            return await _context.TrafficData.Include(t => t.Area).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TrafficData>> GetById(int id)
        {
            var item = await _context.TrafficData.Include(t => t.Area).FirstOrDefaultAsync(t => t.Id == id);
            if (item == null) return NotFound();
            return item;
        }

        [HttpPost]
        public async Task<ActionResult<TrafficData>> Create(TrafficData item)
        {
            _context.TrafficData.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TrafficData item)
        {
            if (id != item.Id) return BadRequest();
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.TrafficData.FindAsync(id);
            if (item == null) return NotFound();
            _context.TrafficData.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}