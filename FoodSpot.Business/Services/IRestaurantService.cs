using FoodSpot.Model;

namespace FoodSpot.Business.Services
{
    public interface IRestaurantService
    {
        Task<IEnumerable<RestaurantModel>> GetAllRestaurantsAsync();
        Task<RestaurantModel> GetRestaurantByIdAsync(int id);
        Task<RestaurantModel> CreateRestaurantAsync(RestaurantModel restaurant);
        Task<RestaurantModel> UpdateRestaurantAsync(RestaurantModel restaurant);
        Task<bool> DeleteRestaurantAsync(int id);
        Task<IEnumerable<RestaurantModel>> GetTopRestaurantsAsync(int count = 5);
    }
}
