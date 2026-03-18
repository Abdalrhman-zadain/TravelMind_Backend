using TravelMind.DataAccess.Data;
using TravelMind.DataAccess.DTO;

namespace TravelMind.Business.Business
{
    public class Trip
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Destination { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal Budget { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }

        public TripDTO DTO => new TripDTO(
            Id, UserId, Name, Destination,
            StartDate, EndDate, Budget, Notes, CreatedAt
        );

        public Trip(TripDTO dto, enMode mode = enMode.AddNew)
        {
            Id = dto.Id;
            UserId = dto.UserId;
            Name = dto.Name;
            Destination = dto.Destination;
            StartDate = dto.StartDate;
            EndDate = dto.EndDate;
            Budget = dto.Budget;
            Notes = dto.Notes;
            CreatedAt = dto.CreatedAt;
            Mode = mode;
        }

        private bool _AddNew()
        {
            this.Id = TripData.AddTrip(DTO);
            return this.Id != -1;
        }

        private bool _Update()
        {
            return TripData.UpdateTrip(DTO);
        }

        public static List<TripDTO> GetAll()
        {
            return TripData.GetAllTrips();
        }

        public static List<TripDTO> GetByUserId(int userId)
        {
            return TripData.GetTripsByUserId(userId);
        }

        public static Trip Find(int id)
        {
            TripDTO dto = TripData.GetTripById(id);
            if (dto != null)
                return new Trip(dto, enMode.Update);
            return null;
        }

        public static bool Delete(int id)
        {
            return TripData.DeleteTrip(id);
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