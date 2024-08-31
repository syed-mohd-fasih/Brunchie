using Brunchie.Models;

namespace Brunchie.ViewModels
{
    public class VendorDashboardModel
    {
        // Orders placed today
        public List<Order>? TodayOrders { get; set; }
        public List<Order>? CompletedOrders { get; set; }

        // Menu items managed by the vendor
        public List<Menu>? Menus { get; set; }

        // Analytics
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }

        // Customer reviews
        public List<Review>? CustomerReviews { get; set; }
    }
}
