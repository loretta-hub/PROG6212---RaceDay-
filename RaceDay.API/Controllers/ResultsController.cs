
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
    public class ResultsController : ControllerBase
    {
        private readonly RaceDayDbContext _db;
        public ResultsController(RaceDayDbContext db) => _db = db;

        [HttpGet("results")]
        public async Task<IActionResult> GetAll()
        {
            var userId = HttpContext.Session.GetInt32("UserID")!.Value;
            var role = HttpContext.Session.GetString("Role");

            var query = _db.Results.AsNoTracking();

            if (role == "Participant")
                query = query.Where(r => _db.Enrolments.Any(en =>
                    en.EnrolmentID == r.EnrollmentID &&
                    en.ParticipantID == userId));
            else if (role == "Organiser")
                query = query.Where(r => _db.Enrolments.Any(en =>
                    en.EnrolmentID == r.EnrollmentID &&
                    _db.Events.Any(ev => ev.EventID == en.EventID &&
                                         ev.OrganiserID == userId)));
            else
                return Forbid();

            return Ok(await query.ToListAsync());
        }

        [HttpGet("results/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _db.Results.FindAsync(id);
            if (result == null) return NotFound();

            if (!await CanAccess(result)) return Forbid();
            return Ok(result);
        }

        [HttpGet("events/{eventId:int}/results")]
        public async Task<IActionResult> GetForEvent(int eventId)
        {
            if (!await _db.Events.AnyAsync(e => e.EventID == eventId))
                return NotFound("Event not found.");

            var userId = HttpContext.Session.GetInt32("UserID")!.Value;
            var role = HttpContext.Session.GetString("Role");

            var query = _db.Results.AsNoTracking().Where(r =>
                _db.Enrolments.Any(en =>
                    en.EnrolmentID == r.EnrollmentID &&
                    en.EventID == eventId));

            if (role == "Organiser")
            {
                if (!await _db.Events.AnyAsync(e =>
                    e.EventID == eventId && e.OrganiserID == userId))
                    return Forbid();
            }
            else if (role == "Participant")
            {
                query = query.Where(r => _db.Enrolments.Any(en =>
                    en.EnrolmentID == r.EnrollmentID &&
                    en.ParticipantID == userId));
            }
            else return Forbid();

            return Ok(await query.ToListAsync());
        }

        [HttpPost("enrolments/{enrolmentId:int}/results")]
        [SessionAuth("Organiser")]
        public async Task<IActionResult> Create(
            int enrolmentId, ResultRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (request.FinishingPosition <= 0)
                return BadRequest("Finishing position must be greater than zero.");

            var enrolment = await _db.Enrolments.FindAsync(enrolmentId);
            if (enrolment == null) return NotFound("Enrolment not found.");

            if (!await OwnsEvent(enrolment.EventID)) return Forbid();

            if (await _db.Results.AnyAsync(r =>
                r.EnrollmentID == enrolmentId))
                return Conflict("A result already exists for this enrolment.");

            var result = new Result
            {
                EnrollmentID = enrolmentId,
                FinishTime = request.FinishTime,
                FinishingPosition = request.FinishingPosition,
                RecordedAt = DateTime.UtcNow
            };

            _db.Results.Add(result);
            await _db.SaveChangesAsync();
            return Ok(result);
        }

        [HttpPut("results/{id:int}")]
        [SessionAuth("Organiser")]
        public async Task<IActionResult> Update(
            int id, ResultRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (request.FinishingPosition <= 0)
                return BadRequest("Finishing position must be greater than zero.");

            var result = await _db.Results.FindAsync(id);
            if (result == null) return NotFound();

            if (!await CanAccess(result)) return Forbid();

            result.FinishTime = request.FinishTime;
            result.FinishingPosition = request.FinishingPosition;
            await _db.SaveChangesAsync();
            return Ok(result);
        }

        [HttpDelete("results/{id:int}")]
        [SessionAuth("Organiser")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _db.Results.FindAsync(id);
            if (result == null) return NotFound();

            if (!await CanAccess(result)) return Forbid();

            _db.Results.Remove(result);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Result deleted." });
        }

        private async Task<bool> OwnsEvent(int eventId)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            return userId != null && await _db.Events.AnyAsync(e =>
                e.EventID == eventId && e.OrganiserID == userId);
        }

        private async Task<bool> CanAccess(Result result)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (role == "Organiser")
                return await _db.Enrolments.AnyAsync(en =>
                    en.EnrolmentID == result.EnrollmentID &&
                    _db.Events.Any(ev => ev.EventID == en.EventID &&
                                         ev.OrganiserID == userId));

            if (role == "Participant")
                return await _db.Enrolments.AnyAsync(en =>
                    en.EnrolmentID == result.EnrollmentID &&
                    en.ParticipantID == userId);

            return false;
        }
    }

    public class ResultRequest
    {
        public TimeSpan FinishTime { get; set; }
        public int FinishingPosition { get; set; }
    }
}

