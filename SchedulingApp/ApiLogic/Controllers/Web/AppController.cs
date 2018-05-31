using Microsoft.AspNetCore.Mvc;

namespace SchedulingApp.ApiLogic.Controllers.Web
{
    public class AppController : Controller
    {
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Auth");
            }
            return View();
        }
    }
}
