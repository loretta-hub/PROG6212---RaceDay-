
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using RaceDay.Web.Models;
using RaceDay.Web.Services;

namespace RaceDay.Web.Controllers
{
    public class EventsController : Controller
    {
        private readonly RaceDayApiService _apiService;

        public EventsController(RaceDayApiService apiService)
        {
            _apiService = apiService;
        }

        // Display all events
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var events = await _apiService.GetAsync<List<EventViewModel>>(
                    "api/events");

                ViewBag.Role = HttpContext.Session.GetString("Role");

                return View(events ?? new List<EventViewModel>());
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "Unable to load events. Please try again.";
                return View(new List<EventViewModel>());
            }
        }

        // Display one event
        public async Task<IActionResult> Details(int id)
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var eventItem = await _apiService.GetAsync<EventViewModel>(
                    $"api/events/{id}");

                if (eventItem == null)
                    return NotFound();

                return View(eventItem);
            }
            catch (HttpRequestException)
            {
                return NotFound();
            }
        }

        // Show the create event form
        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            return View(new EventViewModel
            {
                EventDate = DateTime.Today.AddDays(1)
            });
        }

        // Send a new event to the API
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventViewModel model)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
                return View(model);

            var eventData = new
            {
                model.Name,
                model.Description,
                model.EventDate,
                model.Location,
                model.Distance,
                model.EventTypeID
            };

            try
            {
                using var response = await _apiService.PostAsync(
                    "api/events", eventData);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError(
                    string.Empty,
                    "The API could not create the event. Check the details and try again.");

                return View(model);
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Could not connect to the RaceDay API.");

                return View(model);
            }
        }
    }
}
