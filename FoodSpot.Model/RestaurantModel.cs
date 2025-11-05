namespace FoodSpot.Model
{
    public class RestaurantModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public CategoryModel Category { get; set; }
        public ICollection<ReviewModel> Reviews { get; set; } = new List<ReviewModel>();
    }
}
