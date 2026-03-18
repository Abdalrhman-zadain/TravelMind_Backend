using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelMind.DataAccess.DTO
{
    public class AttractionDTO
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
        public string OpeningHours { get; set; }
        public decimal EntryFee { get; set; }
        public decimal Rating { get; set; }
        public DateTime CreatedAt { get; set; }

        public AttractionDTO(int id, int categoryId, string nameAr, string nameEn,
            string descriptionAr, string descriptionEn, string city,
            decimal? latitude, decimal? longitude, string imageUrl,
            string openingHours, decimal entryFee, decimal rating, DateTime createdAt)
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
            OpeningHours = openingHours;
            EntryFee = entryFee;
            Rating = rating;
            CreatedAt = createdAt;
        }
    }
}
