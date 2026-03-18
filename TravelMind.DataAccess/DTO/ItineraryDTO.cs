namespace TravelMind.DataAccess.DTO
{
    public class ItineraryDTO
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public int? AttractionId { get; set; }
        public int DayNumber { get; set; }
        public string VisitTime { get; set; }
        public string Notes { get; set; }

        public ItineraryDTO(int id, int tripId, int? attractionId,
            int dayNumber, string visitTime, string notes)
        {
            Id = id;
            TripId = tripId;
            AttractionId = attractionId;
            DayNumber = dayNumber;
            VisitTime = visitTime;
            Notes = notes;
        }
    }
}