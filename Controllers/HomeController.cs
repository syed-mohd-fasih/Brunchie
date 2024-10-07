using Brunchie.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Brunchie.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (User.Identity != null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (User.IsInRole("Student"))
                    {
                        return RedirectToAction("Index", "Student");
                    }
                    else if (User.IsInRole("Vendor"))
                    {
                        return RedirectToAction("Index", "Vendor");
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Feedback()
        {
            return View();
        }

        [HttpPost]
        public Task<IActionResult> Feedback(IActionResult result)
        {
            return null;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
