
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using RaceDay.Web.Services;
using System.Net.Http;

namespace RaceDay.Web.Controllers
{
    public class OrganiserController : Controller
    {
        private readonly RaceDayApiService _api;

        public OrganiserController(RaceDayApiService api)
        {
            _api = api;
        }

        // ORGANISER DASHBOARD
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null || role != "Organiser")
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.FirstName =
                HttpContext.Session.GetString("FirstName");

            var apiCookie =
                HttpContext.Session.GetString("ApiCookie");

            var dashboard = new OrganiserDashboardViewModel();

            try
            {
                // Retrieve events from the API.
                var events = await _api.GetAsync<List<DashboardEventDto>>(
                    "api/events", apiCookie)
                    ?? new List<DashboardEventDto>();

                // Retrieve only this organiser's events.
                var organiserEvents = events
                    .Where(e => e.OrganiserID == userId.Value)
                    .ToList();

                // Retrieve enrolments from the API.
                var enrolments =
                    await _api.GetAsync<List<DashboardEnrolmentDto>>(
                        "api/enrolments", apiCookie)
                    ?? new List<DashboardEnrolmentDto>();

                dashboard.TotalEvents = organiserEvents.Count;

                // Count enrolments for this organiser's events.
                var organiserEventIds = organiserEvents
                    .Select(e => e.EventID)
                    .ToHashSet();

                dashboard.TotalEnrolments = enrolments
                    .Count(e => organiserEventIds.Contains(e.EventID));

                // Retrieve the next five upcoming events.
                dashboard.UpcomingEvents = organiserEvents
                    .Where(e => e.EventDate.Date >= DateTime.Today)
                    .OrderBy(e => e.EventDate)
                    .Take(5)
                    .ToList();
            }
            catch (HttpRequestException)
            {
                ViewBag.Error =
                    "Could not load dashboard information. Please check that the RaceDay API is running.";
            }
            catch (Exception)
            {
                ViewBag.Error =
                    "An unexpected error occurred while loading the dashboard.";
            }

            return View(dashboard);
        }

        // UPLOAD EVENT BANNER
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadEventBanner(
            int eventId, IFormFile file)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null || role != "Organiser")
            {
                return RedirectToAction("Login", "Account");
            }

            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Please select an image.";
                return RedirectToAction(nameof(Index));
            }

            if (file.Length > 5 * 1024 * 1024)
            {
                TempData["Error"] =
                    "The image must not exceed 5 MB.";
                return RedirectToAction(nameof(Index));
            }

            var apiCookie =
                HttpContext.Session.GetString("ApiCookie");

            try
            {
                using var response = await _api.UploadFileAsync(
                    $"api/events/{eventId}/banner",
                    file,
                    apiCookie);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] =
                        "Event banner uploaded successfully.";
                }
                else
                {
                    TempData["Error"] =
                        "Unable to upload the banner. Please check your event and image.";
                }
            }
            catch
            {
                TempData["Error"] =
                    "An error occurred while uploading the event banner.";
            }

            return RedirectToAction(nameof(Index));
        }
    }

    // DASHBOARD VIEW MODEL
    public class OrganiserDashboardViewModel
    {
        public int TotalEvents { get; set; }

        public int TotalEnrolments { get; set; }

        public List<DashboardEventDto> UpcomingEvents { get; set; }
            = new List<DashboardEventDto>();
    }

    // EVENT INFORMATION RETURNED BY THE API
    public class DashboardEventDto
    {
        public int EventID { get; set; }

        public int OrganiserID { get; set; }

        public string Name { get; set; } = "";

        public DateTime EventDate { get; set; }

        public string Location { get; set; } = "";

        public decimal Distance { get; set; }
    }

    // ENROLMENT INFORMATION RETURNED BY THE API
    public class DashboardEnrolmentDto
    {
        public int EnrolmentID { get; set; }

        public int EventID { get; set; }
    }
}
