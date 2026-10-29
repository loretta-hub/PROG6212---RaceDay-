
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
                var response = await _apiService.PostAsync<object, JsonElement>(
                    "api/auth/register",
                    registrationData);

                TempData["Success"] = "Registration successful. You can now log in.";

                return RedirectToAction(nameof(Login));
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Could not connect to the RaceDay API. Make sure the API is running.");

                return View(model);
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Registration could not be completed. Please check your details and try again.");

                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
    }
}

