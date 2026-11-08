
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using RaceDay.Web.Models;
using RaceDay.Web.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RaceDay.Web.Controllers
{
    public class EventsController : Controller
    {
        private readonly RaceDayApiService _eventsApi;

        public EventsController(RaceDayApiService apiService)
        {
            _eventsApi = apiService;
        }

        // Display all events
        public async Task<IActionResult> Index()
        {
            var cookie = HttpContext.Session.GetString("ApiCookie");

            try
            {
                var events = await _eventsApi.GetAsync<List<EventViewModel>>(
                    "api/events", cookie);

                ViewBag.Role = HttpContext.Session.GetString("Role");

                return View(events ?? new List<EventViewModel>());
            }
            catch
            {
                ViewBag.Error = "Unable to load events.";
                return View(new List<EventViewModel>());
            }
        }

        // Display event details and categories
        public async Task<IActionResult> Details(int id)
        {
            var cookie = HttpContext.Session.GetString("ApiCookie");

            try
            {
                var eventItem = await _eventsApi.GetAsync<EventViewModel>(
                    $"api/events/{id}", cookie);

                if (eventItem == null)
                    return NotFound();

                eventItem.Categories =
                    await _eventsApi.GetAsync<List<CategoryOption>>(
                        $"api/events/{id}/categories", cookie)
                    ?? new List<CategoryOption>();

                return View(eventItem);
            }
            catch
            {
                return NotFound();
            }
        }

        // Display create form
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            var model = new EventViewModel();
            await LoadEventTypes(model);

            return View(model);
        }

        // Save new event
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

            var cookie = HttpContext.Session.GetString("ApiCookie");

            try
            {
                await _eventsApi.PostAsync("api/events", new
                {
                    model.Name,
                    model.Description,
                    model.EventDate,
                    model.Location,
                    model.Distance,
                    model.EventTypeID
                }, cookie);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Unable to create event.");
                await LoadEventTypes(model);
                return View(model);
            }
        }

        // Display edit form
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            var cookie = HttpContext.Session.GetString("ApiCookie");

            try
            {
                var model = await _eventsApi.GetAsync<EventViewModel>(
                    $"api/events/{id}", cookie);

                if (model == null)
                    return NotFound();

                await LoadEventTypes(model);
                return View(model);
            }
            catch
            {
                return NotFound();
            }
        }

        // Update event
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

            var cookie = HttpContext.Session.GetString("ApiCookie");

            try
            {
                await _eventsApi.PutAsync($"api/events/{id}", new
                {
                    model.Name,
                    model.Description,
                    model.EventDate,
                    model.Location,
                    model.Distance,
                    model.EventTypeID
                }, cookie);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Unable to update event.");
                await LoadEventTypes(model);
                return View(model);
            }
        }

        // Display delete confirmation
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            var cookie = HttpContext.Session.GetString("ApiCookie");

            try
            {
                var model = await _eventsApi.GetAsync<EventViewModel>(
                    $"api/events/{id}", cookie);

                if (model == null)
                    return NotFound();

                return View(model);
            }
            catch
            {
                return NotFound();
            }
        }

        // Delete event
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            var cookie = HttpContext.Session.GetString("ApiCookie");

            try
            {
                await _eventsApi.DeleteAsync($"api/events/{id}", cookie);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Unable to delete event.");
                return RedirectToAction(nameof(Index));
            }
        }

        // Load event types
        private async Task LoadEventTypes(EventViewModel model)
        {
            var cookie = HttpContext.Session.GetString("ApiCookie");

            model.EventTypes =
                await _eventsApi.GetAsync<List<EventTypeOption>>(
                    "api/events/types", cookie)
                ?? new List<EventTypeOption>();
        }
    }
}
