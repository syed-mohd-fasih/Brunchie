using System.Drawing;

namespace Brunchie.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string VendorId { get; set; } = string.Empty;
        public string StudentId { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime DatePosted { get; set; }
    }
}
