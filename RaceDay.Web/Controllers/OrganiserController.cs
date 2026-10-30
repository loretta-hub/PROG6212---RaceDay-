
using Microsoft.AspNetCore.Mvc;

namespace RaceDay.Web.Controllers
{
    public class OrganiserController : Controller
    {
        public IActionResult Index()
        {
            // Check whether the user is logged in.
            var userId = HttpContext.Session.GetInt32("UserID");
            var role = HttpContext.Session.GetString("Role");

            // Only Organisers may access this dashboard.
            if (userId == null || role != "Organiser")
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.FirstName =
                HttpContext.Session.GetString("FirstName");

            return View();
        }
    }
}
