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

        DbSet<Review> Reviews { get; set; }
    }
}
