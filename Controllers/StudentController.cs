using Brunchie.Areas.Identity.Data;
using Brunchie.Data;
using Brunchie.Models;
using Brunchie.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System.Security.Claims;

namespace Brunchie.Controllers
{
    [Authorize(Policy = "student")]
    public class StudentController : Controller
    {
        private readonly ILogger<StudentController> _logger;
        private readonly UserManager<BrunchieUser> _userManager;
        private readonly AppDbContext _appDbContext;

        public StudentController(UserManager<BrunchieUser> userManager, AppDbContext appDbContext, ILogger<StudentController> logger)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation($"{nameof(Index)}: Fetching all Menus...");
            var menus = await _appDbContext.Menus
                .Include(m => m.MenuItems)
                .ToListAsync();

            List<MenuItem> menuItems = new List<MenuItem>();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogError($"{nameof(Index)}: User not found");
                return NotFound();
            }
            var userId = user.Id;
            var userCampus = user.CampusName;

            _logger.LogInformation($"{nameof(Index)}: Picking Campus specific MenuItems");
            foreach (var menu in menus)
            {
                var vendor = await _userManager.FindByIdAsync(menu.VendorId);
                if (vendor == null)
                {
                    continue;
                }
                var vendorCampus = vendor.CampusName;
                if (vendorCampus == userCampus)
                {
                    menuItems.AddRange(menu.MenuItems);
                }
            }

            var userCart = await _appDbContext.Carts
                .Where(c => c.StudentId == userId)
                .Include(c => c.Orders)
                .ThenInclude(o => o.OrderItems)
                .FirstOrDefaultAsync();

            _logger.LogInformation($"{nameof(Index)}: Getting User Cart...");
            if (userCart == null)
            {
                _logger.LogWarning($"{nameof(Index)}: Creating New Cart for User");
                userCart = new Cart
                {
                    StudentId = user.Id,
                    Orders = new List<Order>()
                };

                _appDbContext.Carts.Add(userCart);
            }

            _logger.LogInformation($"{nameof(Index)}: Fetching Active User orders...");
            var activeOrders = _appDbContext.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.StudentId == userId && o.Status == Order.OrderStatus.Received)
                .ToList();

            _logger.LogInformation($"{nameof(Index)}: Sending data to View...");
            var model = new StudentDashboardModel
            {
                CampusName = userCampus,
                CampusMenuItems = menuItems,
                StudentCart = userCart,
                ActiveOrders = activeOrders
            };

            return View(model);
        }

        // GET: Student/Checkout
        public IActionResult Checkout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = _appDbContext.Carts
                .Include(c => c.Orders)
                .ThenInclude(o => o.OrderItems)
                .FirstOrDefault(c => c.StudentId == userId);

            return View(model);
        }

        // POST: Student/UpdateCart
        [HttpPost]
        public async Task<IActionResult> UpdateCart(string itemId, int quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _appDbContext.Carts
                .Include(c => c.Orders)
                .ThenInclude(o => o.OrderItems)
                .FirstOrDefaultAsync(c => c.StudentId == userId);

            if (cart != null)
            {
                var orderItem = cart.Orders
                    .SelectMany(o => o.OrderItems)
                    .FirstOrDefault(oi => oi.Id == itemId);


                if (orderItem != null && quantity > 0)
                {
                    var order = cart.Orders.FirstOrDefault(o => o.Id == orderItem.OrderId);
                    if(order == null)
                    {
                        _logger.LogError($"{nameof(UpdateCart)}: Order not found for orderitem...");
                        return NotFound();
                    }

                    orderItem.Quantity = quantity;
                    orderItem.Price = quantity * orderItem.PricePerPiece;

                    order.TotalPrice = order.OrderItems.Sum(oi => oi.Price);
                    cart.TotalPrice = cart.Orders.Sum(o => o.TotalPrice);

                    await _appDbContext.SaveChangesAsync();
                }
            }

            return RedirectToAction(nameof(Checkout));
        }


        // POST: Student/RemoveItem
        [HttpPost]
        public async Task<IActionResult> RemoveItem(string itemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _appDbContext.Carts
                .Include(c => c.Orders)
                .ThenInclude(o => o.OrderItems)
                .FirstOrDefaultAsync(c => c.StudentId == userId);

            if (cart != null)
            {
                var orderItem = cart.Orders
                    .SelectMany(o => o.OrderItems)
                    .FirstOrDefault(oi => oi.Id == itemId);

                if (orderItem != null)
                {
                    var order = cart.Orders.FirstOrDefault(o => o.OrderItems.Contains(orderItem));
                    if (order != null)
                    {
                        order.OrderItems.Remove(orderItem);
                        _appDbContext.OrderItems.Remove(orderItem);

                        if (order.OrderItems.Count == 0)
                        {
                            cart.Orders.Remove(order);
                            _appDbContext.Orders.Remove(order);
                        }

                        await _appDbContext.SaveChangesAsync();
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Student/PlaceOrder
        [HttpPost]
        public async Task<IActionResult> PlaceOrder()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = _appDbContext.Carts
                .Include(c => c.Orders)
                .ThenInclude(o => o.OrderItems)
                .FirstOrDefault(c => c.StudentId == userId);

            if (cart != null && cart.Orders.Any())
            {
                // Create a list to hold the orders that will be processed
                List<Order> processedOrders = new List<Order>();

                foreach (var order in cart.Orders)
                {
                    order.Status = Order.OrderStatus.Received;
                    order.DateOrdered = DateTime.Now;
                    order.TotalPrice = order.OrderItems.Sum(oi => oi.Price);

                    // Add the processed order to the list
                    processedOrders.Add(order);

                    // Clear the order items from the cart (if necessary)
                    // Note: This step is not strictly necessary since the orders are being processed separately
                }

                // Clear the orders in the cart
                cart.Orders.Clear();

                // Save changes to update the cart with cleared orders
                await _appDbContext.SaveChangesAsync();

                //// Add the processed orders to the Orders table
                //await _appDbContext.Orders.AddRangeAsync(processedOrders);

                //// Save the changes to the database to store the new orders
                //await _appDbContext.SaveChangesAsync();
            }

            return RedirectToAction(nameof(OrderConfirmation));
        }


        // GET: Student/OrderConfirmation
        public IActionResult OrderConfirmation()
        {
            return View();
        }


        // GET: Student/AddToCart
        [HttpGet]
        public async Task<IActionResult> AddToCart(string menuItemId)
        {
            if (string.IsNullOrEmpty(menuItemId))
            {
                _logger.LogError($"{nameof(AddToCart)}: Empty Menu Id, cant add to Cart");
                return NotFound();
            }

            _logger.LogInformation($"{nameof(AddToCart)}: Getting Menu Item from Db...");
            var menuItem = await _appDbContext.MenuItems
                .FirstOrDefaultAsync(m => m.Id == menuItemId);
            if (menuItem == null)
            {
                _logger.LogError($"{nameof(AddToCart)}: Menu Item not found in Db");
                return NotFound();
            }

            return View(menuItem);
        }


        // POST: Student/AddToCart
        [HttpPost]
        public async Task<IActionResult> AddToCart(string menuItemId, int quantity, string specialInstructions)
        {
            if (string.IsNullOrEmpty(menuItemId) || quantity <= 0)
            {
                _logger.LogError($"{nameof(AddToCart)}: Invalid Add To Cart Request");
                return BadRequest("Invalid item or quantity.");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogError($"{nameof(AddToCart)}: User not Found...");
                return Unauthorized();
            }

            _logger.LogInformation($"{nameof(AddToCart)}: Fetching Menu Item from Db...");
            var menuItem = await _appDbContext.MenuItems.FirstOrDefaultAsync(m => m.Id == menuItemId);
            if (menuItem == null)
            {
                _logger.LogError($"{nameof(AddToCart)}: Menu Item not found in Db...");
                return NotFound("Menu item not found.");
            }

            _logger.LogInformation($"{nameof(AddToCart)}: Fetching User Cart from Db...");
            var cart = await _appDbContext.Carts
                .Include(c => c.Orders)
                .ThenInclude(o => o.OrderItems)
                .FirstOrDefaultAsync(c => c.StudentId == user.Id);

            if (cart == null)
            {
                _logger.LogWarning($"{nameof(AddToCart)}: Creating New cart for user...");
                cart = new Cart
                {
                    StudentId = user.Id,
                    Orders = new List<Order>()
                };

                _appDbContext.Carts.Add(cart);
            }

            _logger.LogInformation($"{nameof(AddToCart)}: Fetching Parent Menu for Item to get VendorId");
            var itemMenu = await _appDbContext.Menus
                .Where(m => m.Id == menuItem.MenuId)
                .FirstOrDefaultAsync();
            if (itemMenu == null)
            {
                _logger.LogError($"{nameof(AddToCart)}: Parent Menu to Item not found");
                return NotFound();
            }
            var itemVendorId = itemMenu.VendorId;

            _logger.LogInformation($"{nameof(AddToCart)}: Fetching Order from Db");
            var existingOrder = cart.Orders.FirstOrDefault(o => o.VendorId == itemVendorId && o.Status == Order.OrderStatus.Pending);

            if (existingOrder != null)
            {
                _logger.LogWarning($"{nameof(AddToCart)}: Order Already Exists for user...");

                // Check if the OrderItem for the MenuItem already exists in the Order
                var existingOrderItem = existingOrder.OrderItems.FirstOrDefault(oi => oi.Id == menuItemId);

                if (existingOrderItem != null)
                {
                    _logger.LogWarning($"{nameof(AddToCart)}: Order Item Already Exists for user...");
                    // Update the existing OrderItem
                    existingOrderItem.Quantity += quantity;
                    existingOrderItem.PricePerPiece = menuItem.Price;
                    existingOrderItem.Price += menuItem.Price * quantity;

                    if (!string.IsNullOrEmpty(specialInstructions))
                    {
                        // Append new instructions to existing ones
                        if (string.IsNullOrEmpty(existingOrderItem.SpecialInstruction))
                        {
                            existingOrderItem.SpecialInstruction = specialInstructions;
                        }
                        else
                        {
                            existingOrderItem.SpecialInstruction += $" | {specialInstructions}";
                        }
                    }
                }
                else
                {
                    _logger.LogInformation($"{nameof(AddToCart)}: Creating new order item for user...");
                    // Create a new OrderItem and add it to the existing Order
                    var newOrderItem = new OrderItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = menuItem.Name,
                        Quantity = quantity,
                        PricePerPiece = menuItem.Price,
                        Price = menuItem.Price * quantity,
                        SpecialInstruction = specialInstructions ?? string.Empty
                    };
                    existingOrder.OrderItems.Add(newOrderItem);
                }
                _logger.LogInformation($"{nameof(AddToCart)}: Fixing Total Order Price...");

                // Update the Order's TotalPrice
                existingOrder.TotalPrice = existingOrder.OrderItems.Sum(oi => oi.Price);
            }
            else
            {
                _logger.LogInformation($"{nameof(AddToCart)}: Creating new order for user...");
                // Create a new Order for the Vendor
                var newOrder = new Order
                {
                    Id = Guid.NewGuid().ToString(),
                    StudentId = user.Id,
                    VendorId = itemVendorId,
                    Status = Order.OrderStatus.Pending,
                    DateOrdered = DateTime.Now,
                    PickupTime = DateTime.Now.AddMinutes(30), // Adjust as needed
                    OrderItems = new List<OrderItem>()
                };

                _logger.LogInformation($"{nameof(AddToCart)}: Creating new order item for user...");
                // Create a new OrderItem
                var newOrderItem = new OrderItem
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = menuItem.Name,
                    Quantity = quantity,
                    PricePerPiece = menuItem.Price,
                    Price = menuItem.Price * quantity,
                    SpecialInstruction = specialInstructions ?? string.Empty
                };

                // Add the OrderItem to the new Order
                newOrder.OrderItems.Add(newOrderItem);

                // Set the TotalPrice for the new Order
                newOrder.TotalPrice = newOrderItem.Price;

                // Add the new Order to the Cart
                cart.Orders.Add(newOrder);
            }

            _logger.LogInformation($"{nameof(AddToCart)}: Saving Changes to Db...");
            // Save the changes to the database
            await _appDbContext.SaveChangesAsync();

            // Optionally, add a success message or handle redirection as needed
            return RedirectToAction(nameof(Index));
        }
    }
}
