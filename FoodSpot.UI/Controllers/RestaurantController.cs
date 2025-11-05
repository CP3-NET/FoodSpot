using FoodSpot.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodSpot.UI.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        public async Task<IActionResult> Index()
        {
            var restaurants = await _restaurantService.GetAllRestaurantsAsync();
            return View(restaurants);
        }

        public async Task<IActionResult> Details(int id)
        {
            var restaurant = await _restaurantService.GetRestaurantByIdAsync(id);
            if (restaurant == null)
                return NotFound();

            return View(restaurant);
        }
    }
}
