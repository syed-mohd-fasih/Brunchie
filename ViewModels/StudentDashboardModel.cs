using Brunchie.Models;

namespace Brunchie.ViewModels
{
    public class StudentDashboardModel
    {
        public string CampusName { get; set; } = string.Empty;
        public Cart StudentCart { get; set; }
        public List<Menu> CampusMenu { get; set; }

    }
}
