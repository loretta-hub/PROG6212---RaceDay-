
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using RaceDay.Web.Models;
using RaceDay.Web.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RaceDay.Web.Controllers
{
    public class ParticipantController : Controller
    {
        private readonly RaceDayApiService _apiService;

        public ParticipantController(RaceDayApiService apiService)
        {
            _apiService = apiService;
        }

        // PARTICIPANT DASHBOARD
        public IActionResult Index()
        {
            if (!IsParticipant())
                return RedirectToAction("Login", "Account");

            ViewBag.FirstName =
                HttpContext.Session.GetString("FirstName");

            return View();
        }

        // DISPLAY PARTICIPANT PROFILE
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            if (!IsParticipant())
                return RedirectToAction("Login", "Account");

            var cookie = HttpContext.Session.GetString("ApiCookie");

            try
            {
                var profile =
                    await _apiService.GetAsync<ProfileViewModel>(
                        "api/users/profile", cookie);

                if (profile == null)
                    return NotFound();

                return View(profile);
            }
            catch
            {
                ViewBag.Error = "Unable to load your profile.";
                return View(new ProfileViewModel());
            }
        }

        // UPDATE PARTICIPANT PROFILE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (!IsParticipant())
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
                return View(model);

            var cookie = HttpContext.Session.GetString("ApiCookie");

            try
            {
                await _apiService.PutAsync(
                    "api/users/profile",
                    new
                    {
                        model.FirstName,
                        model.LastName,
                        model.Email,
                        model.PhoneNumber
                    },
                    cookie);

                HttpContext.Session.SetString(
                    "FirstName", model.FirstName);

                TempData["Success"] =
                    "Your profile was updated successfully.";

                return RedirectToAction(nameof(Profile));
            }
            catch
            {
                ModelState.AddModelError(
                    "", "Unable to update your profile.");

                return View(model);
            }
        }

        // BROWSE AVAILABLE EVENTS
        [HttpGet]
        public async Task<IActionResult> BrowseEvents()
        {
            if (!IsParticipant())
                return RedirectToAction("Login", "Account");

            var cookie = HttpContext.Session.GetString("ApiCookie");

            try
            {
                var events =
                    await _apiService.GetAsync<List<EventViewModel>>(
                        "api/events", cookie);

                return View(events ?? new List<EventViewModel>());
            }
            catch
            {
                ViewBag.Error =
                    "Unable to load events. Please try again later.";

                return View(new List<EventViewModel>());
            }
        }

        // VIEW EVENT DETAILS AND CATEGORIES
        [HttpGet]
        public async Task<IActionResult> EventDetails(int id)
        {
            if (!IsParticipant())
                return RedirectToAction("Login", "Account");

            var cookie = HttpContext.Session.GetString("ApiCookie");

            try
            {
                var eventDetails =
                    await _apiService.GetAsync<EventViewModel>(
                        $"api/events/{id}", cookie);

                if (eventDetails == null)
                    return NotFound();

                eventDetails.Categories =
                    await _apiService.GetAsync<List<CategoryOption>>(
                        $"api/events/{id}/categories", cookie)
                    ?? new List<CategoryOption>();

                return View(eventDetails);
            }
            catch
            {
                return NotFound();
            }
        }

        // ENROL IN AN EVENT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enrol(
            int eventId, int categoryId)
        {
            if (!IsParticipant())
                return RedirectToAction("Login", "Account");

            var cookie = HttpContext.Session.GetString("ApiCookie");

            try
            {
                await _apiService.PostAsync(
                    $"api/events/{eventId}/enrolments",
                    new { CategoryID = categoryId },
                    cookie);

                TempData["Success"] =
                    "You have successfully enrolled in this event!";

                return RedirectToAction(
                    nameof(EventDetails), new { id = eventId });
            }
            catch
            {
                TempData["Error"] =
                    "Enrolment failed. You may already be enrolled or the selected category may be invalid.";

                return RedirectToAction(
                    nameof(EventDetails), new { id = eventId });
            }
        }

        // DISPLAY MY ENROLMENTS
        [HttpGet]
        public async Task<IActionResult> MyEnrolments()
        {
            if (!IsParticipant())
                return RedirectToAction("Login", "Account");

            var cookie = HttpContext.Session.GetString("ApiCookie");

            try
            {
                var enrolments =
                    await _apiService.GetAsync<List<EnrolmentViewModel>>(
                        "api/enrolments", cookie);

                return View(enrolments ?? new List<EnrolmentViewModel>());
            }
            catch
            {
                ViewBag.Error =
                    "Unable to load your enrolments. Please try again.";

                return View(new List<EnrolmentViewModel>());
            }
        }

        // CANCEL AN ENROLMENT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelEnrolment(int id)
        {
            if (!IsParticipant())
                return RedirectToAction("Login", "Account");

            var cookie = HttpContext.Session.GetString("ApiCookie");

            try
            {
                await _apiService.DeleteAsync(
                    $"api/enrolments/{id}", cookie);

                TempData["Success"] =
                    "Your enrolment was cancelled successfully.";
            }
            catch
            {
                TempData["Error"] =
                    "Unable to cancel this enrolment. It may already have a recorded result.";

            }

            return RedirectToAction(nameof(MyEnrolments));
        }

        // DISPLAY MY RESULTS
        [HttpGet]
        public async Task<IActionResult> MyResults()
        {
            if (!IsParticipant())
                return RedirectToAction("Login", "Account");

            var cookie = HttpContext.Session.GetString("ApiCookie");

            try
            {
                var results =
                    await _apiService.GetAsync<List<ResultViewModel>>(
                        "api/results", cookie);

                return View(results ?? new List<ResultViewModel>());
            }
            catch
            {
                ViewBag.Error =
                    "Unable to load your results. Please try again.";

                return View(new List<ResultViewModel>());
            }
        }

        // CHECK PARTICIPANT ACCESS
        private bool IsParticipant()
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            var role = HttpContext.Session.GetString("Role");

            return userId != null && role == "Participant";
        }
    }
}
