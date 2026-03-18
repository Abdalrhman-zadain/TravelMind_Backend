namespace TravelMind.DataAccess.DTO
{
    public class TripDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Destination { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal Budget { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }

        public TripDTO(int id, int userId, string name, string destination,
            DateTime? startDate, DateTime? endDate, decimal budget,
            string notes, DateTime createdAt)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Destination = destination;
            StartDate = startDate;
            EndDate = endDate;
            Budget = budget;
            Notes = notes;
            CreatedAt = createdAt;
        }
    }
}