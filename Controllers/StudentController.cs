using Brunchie.Areas.Identity.Data;
using Brunchie.Data;
using Brunchie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Brunchie.Controllers
{
    [Authorize(Policy = "student")]
    public class StudentController : Controller
    {
        private readonly UserManager<BrunchieUser> _userManager;
        private readonly AppDbContext _appDbContext;

        public StudentController(UserManager<BrunchieUser> userManager, AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: Student/Cart
        public async Task<IActionResult> Cart()
        {
            return View();
        }

        // POST: Student/AddToCart
        [HttpPost]
        public async Task<IActionResult> AddToCart(string menuItemId, int quantity, string specialInstructions)
        {
            return View();
        }

        // POST: Student/PlaceOrder
        [HttpPost]
        public async Task<IActionResult> PlaceOrder()
        {
            return View();
        }
    }
}
