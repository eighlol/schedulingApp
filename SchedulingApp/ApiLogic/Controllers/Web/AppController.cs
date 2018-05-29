using Microsoft.AspNetCore.Mvc;
using SchedulingApp.ApiLogic.Repositories;

namespace SchedulingApp.ApiLogic.Controllers.Web
{
    public class AppController : Controller
    {
        private IConferenceRepository _repository;

        public AppController(IConferenceRepository repository)
        {
            _repository = repository;

        }
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
