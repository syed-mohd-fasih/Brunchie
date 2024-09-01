using Brunchie.Models;

namespace Brunchie.ViewModels
{
    public class StudentDashboardModel
    {
        public string CampusName { get; set; } = string.Empty;
        public Cart? StudentCart { get; set; }
        public List<MenuItem>? CampusMenuItems { get; set; }
        public List<Order>? ActiveOrders { get; set; }
    }
}
