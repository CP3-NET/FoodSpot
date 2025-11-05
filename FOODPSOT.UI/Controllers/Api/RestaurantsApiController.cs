using FoodSpot.Business.Services;
using FoodSpot.Model;
using Microsoft.AspNetCore.Mvc;

namespace FoodSpot.UI.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsApiController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantsApiController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        // GET: api/RestaurantsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestaurantModel>>> GetRestaurants()
        {
            var restaurants = await _restaurantService.GetAllRestaurantsAsync();
            return Ok(restaurants);
        }

        // GET: api/RestaurantsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RestaurantModel>> GetRestaurant(int id)
        {
            var restaurant = await _restaurantService.GetRestaurantByIdAsync(id);

            if (restaurant == null)
                return NotFound(new { message = "Restaurante não encontrado" });

            return Ok(restaurant);
        }

        // POST: api/RestaurantsApi
        [HttpPost]
        public async Task<ActionResult<RestaurantModel>> CreateRestaurant(RestaurantModel restaurant)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _restaurantService.CreateRestaurantAsync(restaurant);
            return CreatedAtAction(nameof(GetRestaurant), new { id = created.Id }, created);
        }

        // PUT: api/RestaurantsApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRestaurant(int id, RestaurantModel restaurant)
        {
            if (id != restaurant.Id)
                return BadRequest(new { message = "ID não corresponde" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _restaurantService.GetRestaurantByIdAsync(id);
            if (existing == null)
                return NotFound(new { message = "Restaurante não encontrado" });

            await _restaurantService.UpdateRestaurantAsync(restaurant);
            return NoContent();
        }

        // DELETE: api/RestaurantsApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestaurant(int id)
        {
            var restaurant = await _restaurantService.GetRestaurantByIdAsync(id);
            if (restaurant == null)
                return NotFound(new { message = "Restaurante não encontrado" });

            await _restaurantService.DeleteRestaurantAsync(id);
            return NoContent();
        }

        // GET: api/RestaurantsApi/top/5
        [HttpGet("top/{count}")]
        public async Task<ActionResult<IEnumerable<RestaurantModel>>> GetTopRestaurants(int count = 5)
        {
            var restaurants = await _restaurantService.GetTopRestaurantsAsync(count);
            return Ok(restaurants);
        }
    }
}
