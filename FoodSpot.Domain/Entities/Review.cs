namespace FoodSpot.Domain.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int Rating { get; set; } // 1-5 estrelas
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; }
        public int RestaurantId { get; set; }

        public User User { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}
