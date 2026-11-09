
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
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
        private readonly BlobStorageService _blobStorage;

        public UsersController(
            RaceDayDbContext context,
            BlobStorageService blobStorage)
        {
            _context = context;
            _blobStorage = blobStorage;
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

            return Ok(new
            {
                user.UserID,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Role,
                user.PhoneNumber,
                user.ProfilePictureUrl,
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
                user.PhoneNumber,
                user.ProfilePictureUrl
            });
        }

        // POST: api/users/profile-picture
        [HttpPost("profile-picture")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<IActionResult> UploadProfilePicture(
            IFormFile file)
        {
            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
                return Unauthorized("Please log in first.");

            string? role = HttpContext.Session.GetString("Role");

            if (role != "Participant")
                return Forbid();

            if (file == null || file.Length == 0)
                return BadRequest("Please select an image.");

            var user = await _context.Users.FindAsync(userId.Value);

            if (user == null)
                return NotFound("User not found.");

            try
            {
                string imageUrl =
                    await _blobStorage.UploadImageAsync(
                        file, "profile-pictures");

                user.ProfilePictureUrl = imageUrl;

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Profile picture uploaded successfully.",
                    user.ProfilePictureUrl
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(
                    500, "Unable to upload the profile picture.");
            }
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
