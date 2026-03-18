using Microsoft.AspNetCore.Mvc;
using TravelMind.Business.Business;
using TravelMind.DataAccess.DTO;

namespace TravelMind.API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        // GET /api/categories
        [HttpGet]
        public ActionResult<List<CategoryDTO>> GetAll()
        {
            var list = Category.GetAll();
            if (list.Count == 0) return NoContent();
            return Ok(list);
        }

        // GET /api/categories/1
        [HttpGet("{id}")]
        public ActionResult<CategoryDTO> GetById(int id)
        {
            Category category = Category.Find(id);
            if (category == null)
                return NotFound($"Category with ID {id} not found!");
            return Ok(category.DTO);
        }

        // GET /api/categories/type/Attraction
        [HttpGet("type/{type}")]
        public ActionResult<List<CategoryDTO>> GetByType(string type)
        {
            var list = Category.GetByType(type);
            if (list.Count == 0)
                return NotFound($"No categories found for type {type}!");
            return Ok(list);
        }

        // POST /api/categories
        [HttpPost]
        public ActionResult<CategoryDTO> Create(CategoryDTO dto)
        {
            if (string.IsNullOrEmpty(dto.NameEn) || string.IsNullOrEmpty(dto.NameAr))
                return BadRequest("Name in Arabic and English are required!");

            var category = new Category(dto, Category.enMode.AddNew);
            if (category.Save())
                return CreatedAtAction(nameof(GetById), new { id = category.Id }, category.DTO);

            return StatusCode(500, "Failed to add category!");
        }

        // PUT /api/categories/1
        [HttpPut("{id}")]
        public ActionResult<CategoryDTO> Update(int id, CategoryDTO dto)
        {
            Category category = Category.Find(id);
            if (category == null)
                return NotFound($"Category with ID {id} not found!");

            category.NameAr = dto.NameAr;
            category.NameEn = dto.NameEn;
            category.Type = dto.Type;

            if (category.Save())
                return Ok(category.DTO);

            return StatusCode(500, "Failed to update category!");
        }

        // DELETE /api/categories/1
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Category category = Category.Find(id);
            if (category == null)
                return NotFound($"Category with ID {id} not found!");

            if (Category.Delete(id))
                return Ok($"Category with ID {id} deleted successfully!");

            return StatusCode(500, "Failed to delete category!");
        }
    }
}
