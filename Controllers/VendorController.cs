using Brunchie.Areas.Identity.Data;
using Brunchie.Data;
using Brunchie.Models;
using Brunchie.Services;
using Brunchie.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Brunchie.Controllers
{
    [Authorize(Policy = "vendor")]
    public class VendorController : Controller
    {
        public ILogger<VendorController> _logger { get; }
        public UserManager<BrunchieUser> _userManager { get; }
        public VendorService _vendorService { get; }
        public StudentService _studentService { get; }
        public AppDbContext _appDbContext { get; }

        public VendorController(
            ILogger<VendorController> logger,
            UserManager<BrunchieUser> userManager,
            VendorService vendorService,
            StudentService studentService,
            AppDbContext appDbContext)
        {
            _logger = logger;
            _userManager = userManager;
            _vendorService = vendorService;
            _studentService = studentService;
            _appDbContext = appDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return NotFound();
            }
            var userId = user.Id;
            var userCampus = user.CampusName;

            VendorDashboardModel model = new VendorDashboardModel
            {
                CampusName = userCampus,
                TotalOrders = 500,
                TotalRevenue = 6473,
            };
            
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            var userId = user.Id;

            var orders = await _appDbContext.Orders
                .Where(o => o.VendorId == userId && o.Status == Order.OrderStatus.Received)
                .Include(o => o.OrderItems)
                .ToListAsync();
            var completedOrders = await _appDbContext.CompletedOrders
                .Where(o => o.VendorId == userId && o.Status == Order.OrderStatus.Completed)
                .Include(o => o.OrderItems)
                .ToListAsync();

            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> Menus()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            var userId = user.Id;

            var menus = await _appDbContext.Menus
                .Where(m => m.VendorId == userId)
                .Include(m => m.MenuItems)
                .ToListAsync();

            return View(menus);
        }

        [HttpGet]
        public async Task<IActionResult> OrderDetails(string orderId)
        {
            if (string.IsNullOrWhiteSpace(orderId))
            {
                _logger.LogWarning($"{nameof(OrderDetails)}: Order ID is null or empty");
                return BadRequest("Order ID is required.");
            }

            var order = await _appDbContext.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                _logger.LogError($"{nameof(OrderDetails)}: Order with ID {orderId} not found");
                return NotFound();
            }

            return View(order);
        }


        [HttpGet]
        public IActionResult Analytics()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Reviews()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            var userId = user.Id;

            var reviews = await _appDbContext.Reviews
                .Where(r => r.VendorId == userId)
                .ToListAsync();

            return View();
        }

        [HttpGet]
        public IActionResult AddMenu()
        {
            var model = new Menu();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddMenu(string MenuName, string[] ItemNames, string[] ItemDescriptions, string[] ItemImages, decimal[] ItemPrices)
        {
            if (ItemNames.Length != ItemDescriptions.Length || ItemNames.Length != ItemImages.Length || ItemNames.Length != ItemPrices.Length)
            {
                ModelState.AddModelError("", "All menu item fields must be provided.");
                return View();
            }

            var vendorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(vendorId == null)
            {
                return NotFound();
            }

            var menu = new Menu
            {
                Id = Guid.NewGuid().ToString(),
                Name = MenuName,
                VendorId = vendorId,
                MenuItems = new List<MenuItem>()
            };

            for (int i = 0; i < ItemNames.Length; i++)
            {
                menu.MenuItems.Add(new MenuItem
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = ItemNames[i],
                    Description = ItemDescriptions[i],
                    Image = ItemImages[i],
                    Price = ItemPrices[i]
                });
            }

            _appDbContext.Menus.Add(menu);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Edit Menu (GET)
        [HttpGet]
        public async Task<IActionResult> EditMenu(string id)
        {
            _logger.LogInformation($"{nameof(EditMenu)}: HttpGet was accessed");
            var menu = await _appDbContext.Menus.Include(m => m.MenuItems).FirstOrDefaultAsync(m => m.Id == id);
            if (menu == null)
            {
                _logger.LogError($"{nameof(EditMenu)}: Menu Not found");
                return NotFound();
            }

            return View(menu);
        }

        // Edit Menu (POST)
        [HttpPost]
        public async Task<IActionResult> EditMenu(string id, string[] ItemNames, string[] ItemDescriptions, string[] ItemImages, decimal[] ItemPrices)
        {
            _logger.LogInformation($"{nameof(EditMenu)}: HttpPost was accessed");
            var menu = await _appDbContext.Menus.Include(m => m.MenuItems).FirstOrDefaultAsync(m => m.Id == id);
            if (menu == null)
            {
                _logger.LogError($"{nameof(EditMenu)}: Menu Not found");
                return NotFound();
            }

            menu.MenuItems.Clear();
            for (int i = 0; i < ItemNames.Length; i++)
            {
                menu.MenuItems.Add(new MenuItem
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = ItemNames[i],
                    Description = ItemDescriptions[i],
                    Image = ItemImages[i],
                    Price = ItemPrices[i]
                });
            }

            _appDbContext.Update(menu);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMenu(string id)
        {
            var menu = await _appDbContext.Menus.Include(m => m.MenuItems).FirstOrDefaultAsync(m => m.Id == id);

            if (menu == null)
            {
                return NotFound();
            }

            // Delete related MenuItems first
            _appDbContext.MenuItems.RemoveRange(menu.MenuItems);
            _appDbContext.Menus.Remove(menu);

            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(string id, Order.OrderStatus status)
        {
            var order = await _appDbContext.Orders.FindAsync(id);
            if (order != null)
            {
                order.Status = status;

                if(status == Order.OrderStatus.Completed)
                {
                    await _appDbContext.CompletedOrders.AddRangeAsync(order);
                    _appDbContext.Orders.Remove(order);
                }

                _appDbContext.Orders.Update(order);
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                _logger.LogError($"{nameof(UpdateOrderStatus)}: Order not found");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
