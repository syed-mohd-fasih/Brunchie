using Brunchie.Models;
using Microsoft.EntityFrameworkCore;

namespace Brunchie.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Specify precision and scale for the Price property
            modelBuilder.Entity<MenuItem>()
                .Property(m => m.Price)
                .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<OrderItem>()
                .Property(m => m.Price)
                .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Order>()
                .Property(m => m.TotalPrice)
                .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<MenuItem>()
                .Property(m => m.Price)
                .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Cart>()
                .Property(m => m.TotalPrice)
                .HasColumnType("decimal(18,2)");

        }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> CompletedOrders { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<CustomerSupportTicket> Tickets { get; set; }
        public DbSet<CustomerSupportTicket> ResolvedTickets { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}
