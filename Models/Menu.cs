namespace Brunchie.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public MenuItem(int id, string name, string description, string image, decimal price)
        {
            Id = id;
            Name = name;
            Description = description;
            Image = image;
            Price = price;
        }
    }

    public class Menu
    {
        public int Id { get; set; }
        public string VendorId { get; set; } = string.Empty;
        public List<MenuItem> Items { get; set; }
    }
}
