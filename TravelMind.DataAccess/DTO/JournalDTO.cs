namespace TravelMind.DataAccess.DTO
{
    public class JournalDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? TripId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime? Date { get; set; }
        public DateTime CreatedAt { get; set; }

        public JournalDTO(int id, int userId, int? tripId, string title,
            string content, DateTime? date, DateTime createdAt)
        {
            Id = id;
            UserId = userId;
            TripId = tripId;
            Title = title;
            Content = content;
            Date = date;
            CreatedAt = createdAt;
        }
    }
}