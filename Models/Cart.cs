namespace Brunchie.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public List<Order> Orders { get; set; }
    }
}
