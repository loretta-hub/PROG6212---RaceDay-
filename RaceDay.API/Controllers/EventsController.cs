
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;
using RaceDay.API.Services;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api/events")]
    [SessionAuth]
    public class EventsController : ControllerBase
    {
        private readonly RaceDayDbContext _db;
        public EventsController(RaceDayDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _db.Events.AsNoTracking().ToListAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _db.Events.FindAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost]
        [SessionAuth("Organiser")]
        public async Task<IActionResult> Create(EventRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (request.Distance <= 0)
                return BadRequest("Distance must be greater than zero.");

            if (!await _db.EventTypes.AnyAsync(
                x => x.EventTypeID == request.EventTypeID))
                return BadRequest("Invalid event type.");

            var item = new Event
            {
                OrganiserID = HttpContext.Session.GetInt32("UserID")!.Value,
                EventTypeID = request.EventTypeID,
                Name = request.Name,
                Description = request.Description,
                EventDate = request.EventDate,
                Location = request.Location,
                Distance = request.Distance,
                CreatedAt = DateTime.UtcNow
            };

            _db.Events.Add(item);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById),
                new { id = item.EventID }, item);
        }

        [HttpPut("{id:int}")]
        [SessionAuth("Organiser")]
        public async Task<IActionResult> Update(
            int id, EventRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var item = await _db.Events.FindAsync(id);
            if (item == null) return NotFound();

            var userId = HttpContext.Session.GetInt32("UserID");
            if (item.OrganiserID != userId)
                return Forbid();

            if (request.Distance <= 0)
                return BadRequest("Distance must be greater than zero.");

            if (!await _db.EventTypes.AnyAsync(
                x => x.EventTypeID == request.EventTypeID))
                return BadRequest("Invalid event type.");

            item.Name = request.Name;
            item.Description = request.Description;
            item.EventDate = request.EventDate;
            item.Location = request.Location;
            item.Distance = request.Distance;
            item.EventTypeID = request.EventTypeID;

            await _db.SaveChangesAsync();
            return Ok(item);
        }

        [HttpDelete("{id:int}")]
        [SessionAuth("Organiser")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.Events.FindAsync(id);
            if (item == null) return NotFound();

            var userId = HttpContext.Session.GetInt32("UserID");
            if (item.OrganiserID != userId) return Forbid();

            bool hasCategories = await _db.Categories
                .AnyAsync(c => c.EventsID == id);
            bool hasEnrolments = await _db.Enrolments
                .AnyAsync(e => e.EventID == id);

            if (hasCategories || hasEnrolments)
                return Conflict(
                    "Cannot delete an event with categories or enrolments.");

            _db.Events.Remove(item);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Event deleted." });
        }
    }

    public class EventRequest
    {
        [Required] public string Name { get; set; } = "";
        public string? Description { get; set; }
        public DateTime EventDate { get; set; }
        [Required] public string Location { get; set; } = "";
        public decimal Distance { get; set; }
        public int EventTypeID { get; set; }
    }
}
