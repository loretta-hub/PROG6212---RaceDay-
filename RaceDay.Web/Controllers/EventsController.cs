using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            try
            {
                var apiCookie = HttpContext.Session.GetString("ApiCookie");

                var events = await _apiService.GetAsync<List<EventViewModel>>(
                    "api/events", apiCookie);

                ViewBag.Role = HttpContext.Session.GetString("Role");

                return View(events ?? new List<EventViewModel>());
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "Unable to load events. Please try again.";
                return View(new List<EventViewModel>());
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            try
            {
                var apiCookie = HttpContext.Session.GetString("ApiCookie");

                var eventItem = await _apiService.GetAsync<EventViewModel>(
                    $"api/events/{id}", apiCookie);

                if (eventItem == null)
                    return NotFound();

                return View(eventItem);
            }
            catch (HttpRequestException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            var model = new EventViewModel
            {
                EventDate = DateTime.Today.AddDays(1)
            };

            await LoadEventTypes(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventViewModel model)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                await LoadEventTypes(model);
                return View(model);
            }

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
                var apiCookie = HttpContext.Session.GetString("ApiCookie");

                using var response = await _apiService.PostAsync(
                    "api/events", eventData, apiCookie);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError(
                    string.Empty,
                    "The API could not create the event. Check the details and try again.");

                await LoadEventTypes(model);
                return View(model);
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Could not connect to the RaceDay API.");

                await LoadEventTypes(model);
                return View(model);
            }
        }

        // EDIT EVENT

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            try
            {
                var apiCookie = HttpContext.Session.GetString("ApiCookie");

                var eventItem = await _apiService.GetAsync<EventViewModel>(
                    $"api/events/{id}", apiCookie);

                if (eventItem == null)
                    return NotFound();

                await LoadEventTypes(eventItem);

                return View(eventItem);
            }
            catch (HttpRequestException)
            {
                TempData["Error"] = "Unable to load the event for editing.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EventViewModel model)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                await LoadEventTypes(model);
                return View(model);
            }

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
                var apiCookie = HttpContext.Session.GetString("ApiCookie");

                using var response = await _apiService.PutAsync(
                    $"api/events/{id}", eventData, apiCookie);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Event updated successfully.";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(
                    string.Empty,
                    "The API could not update the event. Please check the details.");

                await LoadEventTypes(model);
                return View(model);
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Could not connect to the RaceDay API.");

                await LoadEventTypes(model);
                return View(model);
            }
        }

        // DELETE EVENT

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            try
            {
                var apiCookie = HttpContext.Session.GetString("ApiCookie");

                var eventItem = await _apiService.GetAsync<EventViewModel>(
                    $"api/events/{id}", apiCookie);

                if (eventItem == null)
                    return NotFound();

                return View(eventItem);
            }
            catch (HttpRequestException)
            {
                TempData["Error"] = "Unable to load the event.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            try
            {
                var apiCookie = HttpContext.Session.GetString("ApiCookie");

                using var response = await _apiService.DeleteAsync(
                    $"api/events/{id}", apiCookie);

                if (response.IsSuccessStatusCode)
                    TempData["Success"] = "Event deleted successfully.";
                else
                    TempData["Error"] = "The event could not be deleted.";

                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException)
            {
                TempData["Error"] = "Could not connect to the RaceDay API.";
                return RedirectToAction(nameof(Index));
            }
        }

        private async Task LoadEventTypes(EventViewModel model)
        {
            var apiCookie = HttpContext.Session.GetString("ApiCookie");

            model.EventTypes =
                await _apiService.GetAsync<List<EventTypeOption>>(
                    "api/events/types", apiCookie)
                ?? new List<EventTypeOption>();
        }
    }
}
