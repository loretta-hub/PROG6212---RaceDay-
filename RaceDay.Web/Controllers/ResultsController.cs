using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RaceDay.Web.Models;
using RaceDay.Web.Services;

namespace RaceDay.Web.Controllers
{
    public class ResultsController : Controller
    {
        private readonly RaceDayApiService _apiService;

        public ResultsController(RaceDayApiService apiService)
        {
            _apiService = apiService;
        }

        private bool IsOrganiser()
        {
            return HttpContext.Session.GetInt32("UserID") != null
                && HttpContext.Session.GetString("Role") == "Organiser";
        }

        private async Task LoadEnrolments(string? apiCookie)
        {
            var enrolments =
                await _apiService.GetAsync<List<EnrolmentViewModel>>(
                    "api/enrolments", apiCookie);

            ViewBag.Enrolments = new SelectList(
                enrolments ?? new List<EnrolmentViewModel>(),
                "EnrolmentID",
                "EnrolmentID");
        }

        public async Task<IActionResult> Index()
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            try
            {
                string? apiCookie =
                    HttpContext.Session.GetString("ApiCookie");

                var results =
                    await _apiService.GetAsync<List<ResultViewModel>>(
                        "api/results", apiCookie);

                return View(results ?? new List<ResultViewModel>());
            }
            catch
            {
                ViewBag.Error = "Unable to load results. Please try again.";
                return View(new List<ResultViewModel>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            try
            {
                string? apiCookie =
                    HttpContext.Session.GetString("ApiCookie");

                await LoadEnrolments(apiCookie);

                return View();
            }
            catch
            {
                ViewBag.Error = "Unable to load enrolments. Please try again.";
                ViewBag.Enrolments = new SelectList(
                    new List<EnrolmentViewModel>(), "EnrolmentID", "EnrolmentID");

                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            int enrolmentId,
            TimeSpan finishTime,
            int finishingPosition)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            string? apiCookie =
                HttpContext.Session.GetString("ApiCookie");

            if (enrolmentId <= 0 ||
                finishTime <= TimeSpan.Zero ||
                finishingPosition <= 0)
            {
                ViewBag.Error =
                    "Please select an enrolment and enter a valid finish time and position.";

                await LoadEnrolments(apiCookie);
                return View();
            }

            try
            {
                var request = new
                {
                    FinishTime = finishTime,
                    FinishingPosition = finishingPosition
                };

                var response = await _apiService.PostAsync(
                    $"api/enrolments/{enrolmentId}/results",
                    request,
                    apiCookie);

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Error =
                        "The result could not be saved. Please check the enrolment and try again.";

                    await LoadEnrolments(apiCookie);
                    return View();
                }

                TempData["Success"] = "Participant result saved successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.Error =
                    "An error occurred while saving the result.";

                await LoadEnrolments(apiCookie);
                return View();
            }
        }
    }
}
