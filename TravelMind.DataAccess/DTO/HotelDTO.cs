using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TravelMind.DataAccess.DTO
{
    public class HotelDTO
    {
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

        public HotelDTO(int id, int categoryId, string nameAr, string nameEn,
            string descriptionAr, string descriptionEn, string city,
            decimal? latitude, decimal? longitude, string imageUrl,
            int stars, decimal pricePerNight, string phone, string website,
            decimal rating, DateTime createdAt)
        {
            Id = id;
            CategoryId = categoryId;
            NameAr = nameAr;
            NameEn = nameEn;
            DescriptionAr = descriptionAr;
            DescriptionEn = descriptionEn;
            City = city;
            Latitude = latitude;
            Longitude = longitude;
            ImageUrl = imageUrl;
            Stars = stars;
            PricePerNight = pricePerNight;
            Phone = phone;
            Website = website;
            Rating = rating;
            CreatedAt = createdAt;
        }
    }
}
