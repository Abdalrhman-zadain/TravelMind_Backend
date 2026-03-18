using TravelMind.DataAccess;
using TravelMind.DataAccess.Data;
using TravelMind.DataAccess.DTO;

namespace TravelMind.Business.Business
{
    public class Restaurant
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
        public string Cuisine { get; set; }
        public string PriceRange { get; set; }
        public string Phone { get; set; }
        public decimal Rating { get; set; }
        public DateTime CreatedAt { get; set; }

        public RestaurantDTO DTO => new RestaurantDTO(
            Id, CategoryId, NameAr, NameEn,
            DescriptionAr, DescriptionEn, City,
            Latitude, Longitude, ImageUrl,
            Cuisine, PriceRange, Phone,
            Rating, CreatedAt
        );

        public Restaurant(RestaurantDTO dto, enMode mode = enMode.AddNew)
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
            Cuisine = dto.Cuisine;
            PriceRange = dto.PriceRange;
            Phone = dto.Phone;
            Rating = dto.Rating;
            CreatedAt = dto.CreatedAt;
            Mode = mode;
        }

        private bool _AddNew()
        {
            this.Id = RestaurantData.AddRestaurant(DTO);
            return this.Id != -1;
        }

        private bool _Update()
        {
            return RestaurantData.UpdateRestaurant(DTO);
        }

        public static List<RestaurantDTO> GetAll()
        {
            return RestaurantData.GetAllRestaurants();
        }

        public static List<RestaurantDTO> GetByCity(string city)
        {
            return RestaurantData.GetRestaurantsByCity(city);
        }

        public static List<RestaurantDTO> GetByCuisine(string cuisine)
        {
            return RestaurantData.GetRestaurantsByCuisine(cuisine);
        }

        public static Restaurant Find(int id)
        {
            RestaurantDTO dto = RestaurantData.GetRestaurantById(id);
            if (dto != null)
                return new Restaurant(dto, enMode.Update);
            return null;
        }

        public static bool Delete(int id)
        {
            return RestaurantData.DeleteRestaurant(id);
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