using System.Collections.Generic;

namespace TravelMind.DataAccess.DTO
{
    public class HotelApiDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public double Rating { get; set; }
        public List<string> Amenities { get; set; }
    }

    public class HotelApiResponseDTO
    {
        public List<HotelApiDTO> Hotels { get; set; }
    }
}
