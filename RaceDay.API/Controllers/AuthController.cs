using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;
using RaceDay.API.Services;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly RaceDayDbContext _context;
        private readonly AuthService _authService;

        public AuthController(
            RaceDayDbContext context,
            AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (user.Role != "Organiser" &&
                user.Role != "Participant")
                return BadRequest("Role must be Organiser or Participant.");

            if (await _context.Users.AnyAsync(
                u => u.Email == user.Email))
                return Conflict("Email is already registered.");

            user.PasswordHash =
                _authService.HashPassword(user.PasswordHash);

            user.CreatedAt = DateTime.UtcNow;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Registration successful.",
                user.UserID,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Role
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(
                u => u.Email == request.Email);

            if (user == null ||
                !_authService.VerifyPassword(
                    user.PasswordHash,
                    request.Password))
            {
                return Unauthorized("Invalid email or password.");
            }

            HttpContext.Session.SetInt32("UserID", user.UserID);
            HttpContext.Session.SetString("Role", user.Role);
            HttpContext.Session.SetString("Email", user.Email);

            return Ok(new
            {
                message = "Login successful.",
                user.UserID,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Role
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Ok(new { message = "Logged out successfully." });
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}