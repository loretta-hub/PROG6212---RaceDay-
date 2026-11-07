using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Models;
using RaceDay.Web.Services;

namespace RaceDay.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly RaceDayApiService _apiService;

        public CategoriesController(RaceDayApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserID");
            string? role = HttpContext.Session.GetString("Role");

            if (userId == null || role != "Organiser")
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                string? apiCookie =
                    HttpContext.Session.GetString("ApiCookie");

                // Get all events using the existing EventViewModel.
                var events =
                    await _apiService.GetAsync<List<EventViewModel>>(
                        "api/events", apiCookie);

                var categories = new List<CategoryViewModel>();

                if (events != null)
                {
                    // Only retrieve categories for this organiser's events.
                    foreach (var ev in events.Where(e => e.OrganiserID == userId.Value))
                    {
                        var eventCategories =
                            await _apiService.GetAsync<List<CategoryViewModel>>(
                                $"api/events/{ev.EventID}/categories",
                                apiCookie);

                        if (eventCategories != null)
                        {
                            foreach (var category in eventCategories)
                            {
                                category.EventName = ev.Name;
                                categories.Add(category);
                            }
                        }
                    }
                }

                return View(categories);
            }
            catch (Exception)
            {
                ViewBag.Error =
                    "Could not load categories. Please check your API connection and try again.";

                return View(new List<CategoryViewModel>());
            }
        }
    }

    public class CategoryViewModel
    {
        public int CategoryID { get; set; }

        public int EventsID { get; set; }

        public string? CategoryName { get; set; }

        public string? Description { get; set; }

        public string? EventName { get; set; }
    }
}
