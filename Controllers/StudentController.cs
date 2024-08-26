using Microsoft.AspNetCore.Mvc;

namespace Brunchie.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
