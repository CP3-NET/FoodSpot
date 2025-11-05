namespace FoodSpot.Model
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<RestaurantModel> Restaurants { get; set; } = new List<RestaurantModel>();
    }
}
