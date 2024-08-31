namespace Brunchie.Models
{
    public class MenuItem
    {
        public string Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string MenuId { get; set; } = string.Empty;
    }

    public class Menu
    {
        public string Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string VendorId { get; set; } = string.Empty;
        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}
