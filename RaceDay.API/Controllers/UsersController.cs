
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Services;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    [SessionAuth]
    public class UsersController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public UsersController(RaceDayDbContext context)
        {
            _context = context;
        }

        // GET: api/users/profile
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
                return Unauthorized("Please log in first.");

            var user = await _context.Users.FindAsync(userId.Value);

            if (user == null)
                return NotFound("User not found.");

            // Never return the password hash.
            return Ok(new
            {
                user.UserID,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Role,
                user.PhoneNumber,
                user.CreatedAt
            });
        }

        // PUT: api/users/profile
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile(
            ProfileUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
                return Unauthorized("Please log in first.");

            var user = await _context.Users.FindAsync(userId.Value);

            if (user == null)
                return NotFound("User not found.");

            bool emailExists = await _context.Users.AnyAsync(u =>
                u.Email == request.Email &&
                u.UserID != userId.Value);

            if (emailExists)
                return Conflict("That email is already registered.");

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Profile updated successfully.",
                user.UserID,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Role,
                user.PhoneNumber
            });
        }
    }

    public class ProfileUpdateRequest
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }
    }
}
