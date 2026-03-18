namespace TravelMind.DataAccess.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Type { get; set; }

        public CategoryDTO(int id, string nameAr, string nameEn, string type)
        {
            Id = id;
            NameAr = nameAr;
            NameEn = nameEn;
            Type = type;
        }
    }
}