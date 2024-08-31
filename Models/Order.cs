namespace Brunchie.Models
{
    public class OrderItem
    {
        public string Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string SpecialInstruction { get; set; } = string.Empty;
    }

    public class Order
    {
        public string Id { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public string VendorId { get; set; } = string.Empty;
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public decimal TotalPrice { get; set; }
        public DateTime DateOrdered { get; set; }
        public DateTime PickupTime { get; set; }
        public enum OrderStatus
        {
            Pending,
            Received,
            Completed,
            Cancelled
        }
        public OrderStatus Status { get; set; }
        public int CartId { get; set; }
    }
}