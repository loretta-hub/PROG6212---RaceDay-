using Microsoft.AspNetCore.Mvc;
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

        // Display enrolments belonging to the logged-in organiser.
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            var role = HttpContext.Session.GetString("Role");

            // Only organisers can access this page.
            if (userId == null || role != "Organiser")
            {
                return RedirectToAction("Login", "Account");
            }

            var apiCookie = HttpContext.Session.GetString("ApiCookie");

            try
            {
                var enrolments =
                    await _apiService.GetAsync<List<EnrolmentViewModel>>(
                        "api/enrolments", apiCookie);

                return View(enrolments ?? new List<EnrolmentViewModel>());
            }
            catch (HttpRequestException)
            {
                ViewBag.Error =
                    "Unable to load enrolments. Please check that the API is running.";

                return View(new List<EnrolmentViewModel>());
            }
        }
    }

    public class EnrolmentViewModel
    {
        public int EnrolmentID { get; set; }

        public int ParticipantID { get; set; }

        public int EventID { get; set; }

        public int CategoryID { get; set; }

        public DateTime EnrolmentDate { get; set; }
    }
}