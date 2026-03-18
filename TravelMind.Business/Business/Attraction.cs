using TravelMind.DataAccess;
using TravelMind.DataAccess.Data;
using TravelMind.DataAccess.DTO;

namespace TravelMind.Business.Business
{
    public class Attraction
    {
        // ── MODE ──────────────────────────────────────────────────────
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;

        // ── PROPERTIES ────────────────────────────────────────────────
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

        // ── DTO PROPERTY ──────────────────────────────────────────────
        public AttractionDTO DTO => new AttractionDTO(
            Id, CategoryId, NameAr, NameEn,
            DescriptionAr, DescriptionEn, City,
            Latitude, Longitude, ImageUrl,
            OpeningHours, EntryFee, Rating, CreatedAt
        );

        // ── CONSTRUCTOR ───────────────────────────────────────────────
        public Attraction(AttractionDTO dto, enMode mode = enMode.AddNew)
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
            OpeningHours = dto.OpeningHours;
            EntryFee = dto.EntryFee;
            Rating = dto.Rating;
            CreatedAt = dto.CreatedAt;
            Mode = mode;
        }

        // ── PRIVATE METHODS ───────────────────────────────────────────
        private bool _AddNew()
        {
            this.Id = AttractionData.AddAttraction(DTO);
            return this.Id != -1;
        }

        private bool _Update()
        {
            return AttractionData.UpdateAttraction(DTO);
        }

        // ── PUBLIC STATIC METHODS ─────────────────────────────────────
        public static List<AttractionDTO> GetAll()
        {
            return AttractionData.GetAllAttractions();
        }

        public static List<AttractionDTO> GetByCity(string city)
        {
            return AttractionData.GetAttractionsByCity(city);
        }

        public static List<AttractionDTO> GetByCategory(int categoryId)
        {
            return AttractionData.GetAttractionsByCategory(categoryId);
        }

        public static Attraction Find(int id)
        {
            AttractionDTO dto = AttractionData.GetAttractionById(id);
            if (dto != null)
                return new Attraction(dto, enMode.Update);
            return null;
        }

        public static bool Delete(int id)
        {
            return AttractionData.DeleteAttraction(id);
        }

        // ── SAVE ──────────────────────────────────────────────────────
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