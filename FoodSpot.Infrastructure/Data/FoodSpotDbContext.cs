using Microsoft.EntityFrameworkCore;
using FoodSpot.Domain.Entities;

namespace FoodSpot.Infrastructure.Data
{
    public class FoodSpotDbContext : DbContext
    {
        public FoodSpotDbContext(DbContextOptions<FoodSpotDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


        }
    }
}
