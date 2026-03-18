using TravelMind.DataAccess.Data;
using TravelMind.DataAccess.DTO;

namespace TravelMind.Business.Business
{
    public class Category
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;

        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Type { get; set; }

        public CategoryDTO DTO => new CategoryDTO(Id, NameAr, NameEn, Type);

        public Category(CategoryDTO dto, enMode mode = enMode.AddNew)
        {
            Id = dto.Id;
            NameAr = dto.NameAr;
            NameEn = dto.NameEn;
            Type = dto.Type;
            Mode = mode;
        }

        private bool _AddNew()
        {
            this.Id = CategoryData.AddCategory(DTO);
            return this.Id != -1;
        }

        private bool _Update()
        {
            return CategoryData.UpdateCategory(DTO);
        }

        public static List<CategoryDTO> GetAll()
        {
            return CategoryData.GetAllCategories();
        }

        public static List<CategoryDTO> GetByType(string type)
        {
            return CategoryData.GetCategoriesByType(type);
        }

        public static Category Find(int id)
        {
            CategoryDTO dto = CategoryData.GetCategoryById(id);
            if (dto != null)
                return new Category(dto, enMode.Update);
            return null;
        }

        public static bool Delete(int id)
        {
            return CategoryData.DeleteCategory(id);
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