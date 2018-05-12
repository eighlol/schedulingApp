using Conference.Models;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conference.Controllers.Web
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
