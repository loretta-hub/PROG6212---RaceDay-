
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Models;
using RaceDay.Web.Services;

namespace RaceDay.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly RaceDayApiService _apiService;

        public AccountController(RaceDayApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var registrationData = new
            {
                model.FirstName,
                model.LastName,
                model.Email,
                PasswordHash = model.Password,
                model.Role,
                model.PhoneNumber
            };

            try
            {
                using var response = await _apiService.PostAsync(
                    "api/auth/register",
                    registrationData);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] =
                        "Registration successful. You can now log in.";

                    return RedirectToAction(nameof(Login));
                }

                var error = await response.Content.ReadAsStringAsync();

                ModelState.AddModelError(
                    string.Empty,
                    string.IsNullOrWhiteSpace(error)
                        ? "Registration failed. Please check your details."
                        : error);
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Could not connect to the RaceDay API. Make sure it is running.");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            string email,
            string password)
        {
            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Please enter your email and password.");

                return View();
            }

            try
            {
                var loginData = new
                {
                    Email = email,
                    Password = password
                };

                using var response = await _apiService.PostAsync(
                    "api/auth/login",
                    loginData);

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Login failed. Please check your email and password.");

                    return View();
                }

                using var json = await JsonDocument.ParseAsync(
                    await response.Content.ReadAsStreamAsync());

                var user = json.RootElement;

                HttpContext.Session.SetInt32(
                    "UserID",
                    user.GetProperty("userID").GetInt32());

                HttpContext.Session.SetString(
                    "FirstName",
                    user.GetProperty("firstName").GetString() ?? "");

                HttpContext.Session.SetString(
                    "Email",
                    user.GetProperty("email").GetString() ?? "");

                HttpContext.Session.SetString(
                    "Role",
                    user.GetProperty("role").GetString() ?? "");

                var role = HttpContext.Session.GetString("Role");

                if (role == "Organiser")
                    return RedirectToAction("Index", "Organiser");

                return RedirectToAction("Index", "Participant");
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Could not connect to the RaceDay API. Make sure it is running.");

                return View();
            }
            catch (JsonException)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "The API returned an unexpected response. Please try again.");

                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();

            try
            {
                using var response = await _apiService.PostAsync(
                    "api/auth/logout",
                    new { });

                // The MVC session has already been cleared.
                // The API logout request is best-effort.
            }
            catch (HttpRequestException)
            {
                // The local MVC session remains cleared.
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
