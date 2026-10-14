
using Microsoft.AspNetCore.Mvc;
using RaceDay.API.Services;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api/test-access")]
    public class TestAccessController : ControllerBase
    {
        [HttpGet("participant")]
        [SessionAuth("Participant")]
        public IActionResult ParticipantOnly()
        {
            return Ok("Participant access granted.");
        }

        [HttpGet("organiser")]
        [SessionAuth("Organiser")]
        public IActionResult OrganiserOnly()
        {
            return Ok("Organiser access granted.");
        }
    }
}
