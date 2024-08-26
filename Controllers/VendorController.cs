using Microsoft.AspNetCore.Mvc;

namespace Brunchie.Controllers
{
    public class VendorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
