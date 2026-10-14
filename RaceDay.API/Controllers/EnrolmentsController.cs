
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
    public class EnrolmentsController : ControllerBase
    {
        private readonly RaceDayDbContext _db;
        public EnrolmentsController(RaceDayDbContext db) => _db = db;

        [HttpGet("enrolments")]
        public async Task<IActionResult> GetAll()
        {
            var userId = HttpContext.Session.GetInt32("UserID")!.Value;
            var role = HttpContext.Session.GetString("Role");

            var query = _db.Enrolments.AsNoTracking();

            if (role == "Participant")
                query = query.Where(e => e.ParticipantID == userId);
            else if (role == "Organiser")
                query = query.Where(e => _db.Events.Any(ev =>
                    ev.EventID == e.EventID && ev.OrganiserID == userId));
            else
                return Forbid();

            return Ok(await query.ToListAsync());
        }

        [HttpGet("events/{eventId:int}/enrolments")]
        [SessionAuth("Organiser")]
        public async Task<IActionResult> GetForEvent(int eventId)
        {
            var ev = await _db.Events.FindAsync(eventId);
            if (ev == null) return NotFound("Event not found.");

            if (ev.OrganiserID !=
                HttpContext.Session.GetInt32("UserID"))
                return Forbid();

            return Ok(await _db.Enrolments.AsNoTracking()
                .Where(e => e.EventID == eventId).ToListAsync());
        }

        [HttpGet("enrolments/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _db.Enrolments.FindAsync(id);
            if (item == null) return NotFound();

            if (!await CanAccess(item)) return Forbid();
            return Ok(item);
        }

        [HttpPost("events/{eventId:int}/enrolments")]
        [SessionAuth("Participant")]
        public async Task<IActionResult> Enrol(
            int eventId, EnrolmentRequest request)
        {
            var participantId =
                HttpContext.Session.GetInt32("UserID")!.Value;

            if (!await _db.Events.AnyAsync(e => e.EventID == eventId))
                return NotFound("Event not found.");

            var category = await _db.Categories.FindAsync(request.CategoryID);
            if (category == null || category.EventsID != eventId)
                return BadRequest("Choose a category belonging to this event.");

            if (await _db.Enrolments.AnyAsync(e =>
                e.ParticipantID == participantId && e.EventID == eventId))
                return Conflict("You are already enrolled in this event.");

            var item = new Enrolment
            {
                ParticipantID = participantId,
                EventID = eventId,
                CategoryID = request.CategoryID,
                EnrolmentDate = DateTime.UtcNow
            };

            _db.Enrolments.Add(item);
            await _db.SaveChangesAsync();
            return Ok(item);
        }

        [HttpDelete("enrolments/{id:int}")]
        public async Task<IActionResult> Cancel(int id)
        {
            var item = await _db.Enrolments.FindAsync(id);
            if (item == null) return NotFound();

            var userId = HttpContext.Session.GetInt32("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (role == "Participant" && item.ParticipantID != userId)
                return Forbid();

            if (role == "Organiser")
            {
                var ev = await _db.Events.FindAsync(item.EventID);
                if (ev == null || ev.OrganiserID != userId)
                    return Forbid();
            }
            else if (role != "Participant")
                return Forbid();

            if (await _db.Results.AnyAsync(r =>
                r.EnrollmentID == item.EnrolmentID))
                return Conflict("Cannot cancel an enrolment that has a result.");

            _db.Enrolments.Remove(item);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Enrolment cancelled." });
        }

        private async Task<bool> CanAccess(Enrolment item)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (role == "Participant")
                return item.ParticipantID == userId;

            if (role == "Organiser")
                return await _db.Events.AnyAsync(e =>
                    e.EventID == item.EventID && e.OrganiserID == userId);

            return false;
        }
    }

    public class EnrolmentRequest
    {
        public int CategoryID { get; set; }
    }
}
