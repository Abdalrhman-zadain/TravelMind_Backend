using TravelMind.DataAccess.Data;
using TravelMind.DataAccess.DTO;

namespace TravelMind.Business.Business
{
    public class Itinerary
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;

        public int Id { get; set; }
        public int TripId { get; set; }
        public int? AttractionId { get; set; }
        public int DayNumber { get; set; }
        public string VisitTime { get; set; }
        public string Notes { get; set; }

        public ItineraryDTO DTO => new ItineraryDTO(
            Id, TripId, AttractionId, DayNumber, VisitTime, Notes
        );

        public Itinerary(ItineraryDTO dto, enMode mode = enMode.AddNew)
        {
            Id = dto.Id;
            TripId = dto.TripId;
            AttractionId = dto.AttractionId;
            DayNumber = dto.DayNumber;
            VisitTime = dto.VisitTime;
            Notes = dto.Notes;
            Mode = mode;
        }

        private bool _AddNew()
        {
            this.Id = ItineraryData.AddItinerary(DTO);
            return this.Id != -1;
        }

        private bool _Update()
        {
            return ItineraryData.UpdateItinerary(DTO);
        }

        public static List<ItineraryDTO> GetByTripId(int tripId)
        {
            return ItineraryData.GetItinerariesByTripId(tripId);
        }

        public static Itinerary Find(int id)
        {
            ItineraryDTO dto = ItineraryData.GetItineraryById(id);
            if (dto != null)
                return new Itinerary(dto, enMode.Update);
            return null;
        }

        public static bool Delete(int id)
        {
            return ItineraryData.DeleteItinerary(id);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNew()) { Mode = enMode.Update; return true; }
                    return false;
                case enMode.Update:
                    return _Update();
            }
            return false;
        }
    }
}
