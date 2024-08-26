namespace Brunchie.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
        public string SpecialInstruction { get; set; } = string.Empty;
    }

    public class Order
    {
        public int Id { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public string VendorId { get; set; } = string.Empty;
        public List<OrderItem>? OrderItems { get; set; }
        public DateTime DateOrdered { get; set; }
        public enum OrderStatus
        {
            Pending,
            Completed,
            Cancelled
        }
        public OrderStatus Status { get; set; }
    }
}