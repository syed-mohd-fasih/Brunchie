using Brunchie.Models;

namespace Brunchie.ViewModels
{
    public class VendorDashboardModel
    {
        public string CampusName { get; set; } = string.Empty;

        // Orders placed today
        public int? TodayOrders { get; set; }
        public int? CompletedOrders { get; set; }

        // Analytics
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
