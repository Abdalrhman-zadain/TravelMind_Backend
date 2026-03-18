using TravelMind.DataAccess;
using TravelMind.DataAccess.Data;
using TravelMind.DataAccess.DTO;

namespace TravelMind.Business.Business
{
    public class Hotel
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;

        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public string City { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string ImageUrl { get; set; }
        public int Stars { get; set; }
        public decimal PricePerNight { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public decimal Rating { get; set; }
        public DateTime CreatedAt { get; set; }

        public HotelDTO DTO => new HotelDTO(
            Id, CategoryId, NameAr, NameEn,
            DescriptionAr, DescriptionEn, City,
            Latitude, Longitude, ImageUrl,
            Stars, PricePerNight, Phone, Website,
            Rating, CreatedAt
        );

        public Hotel(HotelDTO dto, enMode mode = enMode.AddNew)
        {
            Id = dto.Id;
            CategoryId = dto.CategoryId;
            NameAr = dto.NameAr;
            NameEn = dto.NameEn;
            DescriptionAr = dto.DescriptionAr;
            DescriptionEn = dto.DescriptionEn;
            City = dto.City;
            Latitude = dto.Latitude;
            Longitude = dto.Longitude;
            ImageUrl = dto.ImageUrl;
            Stars = dto.Stars;
            PricePerNight = dto.PricePerNight;
            Phone = dto.Phone;
            Website = dto.Website;
            Rating = dto.Rating;
            CreatedAt = dto.CreatedAt;
            Mode = mode;
        }

        private bool _AddNew()
        {
            this.Id = HotelData.AddHotel(DTO);
            return this.Id != -1;
        }

        private bool _Update()
        {
            return HotelData.UpdateHotel(DTO);
        }

        public static List<HotelDTO> GetAll()
        {
            return HotelData.GetAllHotels();
        }

        public static List<HotelDTO> GetByCity(string city)
        {
            return HotelData.GetHotelsByCity(city);
        }

        public static List<HotelDTO> GetByStars(int stars)
        {
            return HotelData.GetHotelsByStars(stars);
        }

        public static Hotel Find(int id)
        {
            HotelDTO dto = HotelData.GetHotelById(id);
            if (dto != null)
                return new Hotel(dto, enMode.Update);
            return null;
        }

        public static bool Delete(int id)
        {
            return HotelData.DeleteHotel(id);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNew())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;
                case enMode.Update:
                    return _Update();
            }
            return false;
        }
    }
}