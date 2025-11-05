namespace FoodSpot.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Restaurant> Restaurants { get; set; } = new List<Restaurant>();
    }
}
