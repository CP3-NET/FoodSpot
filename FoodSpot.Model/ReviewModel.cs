namespace FoodSpot.Model
{
    public class ReviewModel
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; }
        public int RestaurantId { get; set; }

        public UserModel User { get; set; }
        public RestaurantModel Restaurant { get; set; }
    }
}
