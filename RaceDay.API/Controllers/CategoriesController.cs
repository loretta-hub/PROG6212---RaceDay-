
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;
using RaceDay.API.Services;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api")]
    [SessionAuth]
    public class CategoriesController : ControllerBase
    {
        private readonly RaceDayDbContext _db;
        public CategoriesController(RaceDayDbContext db) => _db = db;

        [HttpGet("events/{eventId:int}/categories")]
        public async Task<IActionResult> GetForEvent(int eventId)
        {
            if (!await _db.Events.AnyAsync(e => e.EventID == eventId))
                return NotFound("Event not found.");

            return Ok(await _db.Categories.AsNoTracking()
                .Where(c => c.EventsID == eventId).ToListAsync());
        }

        [HttpGet("categories/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _db.Categories.FindAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost("events/{eventId:int}/categories")]
        [SessionAuth("Organiser")]
        public async Task<IActionResult> Create(
            int eventId, CategoryRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ev = await _db.Events.FindAsync(eventId);
            if (ev == null) return NotFound("Event not found.");

            if (ev.OrganiserID !=
                HttpContext.Session.GetInt32("UserID"))
                return Forbid();

            var item = new Category
            {
                EventsID = eventId,
                CategoryName = request.CategoryName,
                Description = request.Description
            };

            _db.Categories.Add(item);
            await _db.SaveChangesAsync();
            return Ok(item);
        }

        [HttpPut("categories/{id:int}")]
        [SessionAuth("Organiser")]
        public async Task<IActionResult> Update(
            int id, CategoryRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var item = await _db.Categories.FindAsync(id);
            if (item == null) return NotFound();

            var ev = await _db.Events.FindAsync(item.EventsID);
            if (ev == null) return NotFound("Event not found.");

            if (ev.OrganiserID !=
                HttpContext.Session.GetInt32("UserID"))
                return Forbid();

            item.CategoryName = request.CategoryName;
            item.Description = request.Description;
            await _db.SaveChangesAsync();
            return Ok(item);
        }

        [HttpDelete("categories/{id:int}")]
        [SessionAuth("Organiser")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.Categories.FindAsync(id);
            if (item == null) return NotFound();

            var ev = await _db.Events.FindAsync(item.EventsID);
            if (ev == null) return NotFound("Event not found.");

            if (ev.OrganiserID !=
                HttpContext.Session.GetInt32("UserID"))
                return Forbid();

            if (await _db.Enrolments.AnyAsync(e => e.CategoryID == id))
                return Conflict("Cannot delete a category with enrolments.");

            _db.Categories.Remove(item);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Category deleted." });
        }
    }

    public class CategoryRequest
    {
        [Required] public string CategoryName { get; set; } = "";
        public string? Description { get; set; }
    }
}
