using FoodSpot.Data;
using FoodSpot.Model;
using Microsoft.EntityFrameworkCore;

namespace FoodSpot.Business.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly ApplicationDbContext _context;

        public RestaurantService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RestaurantModel>> GetAllRestaurantsAsync()
        {
            return await _context.Restaurants
                .Include(r => r.Category)
                .OrderByDescending(r => r.AverageRating)
                .ToListAsync();
        }

        public async Task<RestaurantModel> GetRestaurantByIdAsync(int id)
        {
            return await _context.Restaurants
                .Include(r => r.Category)
                .Include(r => r.Reviews)
                .ThenInclude(rev => rev.User)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<RestaurantModel> CreateRestaurantAsync(RestaurantModel restaurant)
        {
            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();
            return restaurant;
        }

        public async Task<RestaurantModel> UpdateRestaurantAsync(RestaurantModel restaurant)
        {
            _context.Restaurants.Update(restaurant);
            await _context.SaveChangesAsync();
            return restaurant;
        }

        public async Task<bool> DeleteRestaurantAsync(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null) return false;

            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<RestaurantModel>> GetTopRestaurantsAsync(int count = 5)
        {
            return await _context.Restaurants
                .Include(r => r.Category)
                .OrderByDescending(r => r.AverageRating)
                .Take(count)
                .ToListAsync();
        }
    }
}
