namespace TravelMind.DataAccess.DTO
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PlaceType { get; set; }
        public int PlaceId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        public ReviewDTO(int id, int userId, string placeType, int placeId,
            int rating, string comment, DateTime createdAt)
        {
            Id = id;
            UserId = userId;
            PlaceType = placeType;
            PlaceId = placeId;
            Rating = rating;
            Comment = comment;
            CreatedAt = createdAt;
        }
    }
}