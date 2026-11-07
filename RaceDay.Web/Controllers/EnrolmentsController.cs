using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Models;
using RaceDay.Web.Services;

namespace RaceDay.Web.Controllers
{
    public class EnrolmentsController : Controller
    {
        private readonly RaceDayApiService _apiService;

        public EnrolmentsController(RaceDayApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserID");
            string? role = HttpContext.Session.GetString("Role");

            if (userId == null || role != "Organiser")
                return RedirectToAction("Login", "Account");

            try
            {
                string? apiCookie =
                    HttpContext.Session.GetString("ApiCookie");

                var enrolments =
                    await _apiService.GetAsync<List<EnrolmentViewModel>>(
                        "api/enrolments", apiCookie);

                return View(enrolments ?? new List<EnrolmentViewModel>());
            }
            catch (Exception)
            {
                ViewBag.Error =
                    "Unable to load enrolments. Please try again.";

                return View(new List<EnrolmentViewModel>());
            }
        }
    }
}
