namespace TravelMind.DataAccess.DTO
{
    public class ExpenseDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? TripId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; }
        public DateTime? Date { get; set; }
        public DateTime CreatedAt { get; set; }

        public ExpenseDTO(int id, int userId, int? tripId, string description,
            decimal amount, string category, DateTime? date, DateTime createdAt)
        {
            Id = id;
            UserId = userId;
            TripId = tripId;
            Description = description;
            Amount = amount;
            Category = category;
            Date = date;
            CreatedAt = createdAt;
        }
    }
}