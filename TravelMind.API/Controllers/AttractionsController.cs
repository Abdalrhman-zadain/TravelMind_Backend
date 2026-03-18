using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;
using TravelMind.DataAccess.DTO;
using TravelMind.Business.Business;

namespace TravelMind.API.Controllers
{
    [Route("api/attractions")]
    [ApiController]
    public class AttractionsController : ControllerBase
    {
        // -- GET ALL ---------------------------------------------------
        // GET /api/attractions
        [HttpGet]
        public ActionResult<List<AttractionDTO>> GetAll()
        {
            var list = Attraction.GetAll();
            if (list.Count == 0)
                return NoContent();
            return Ok(list);
        }

        // -- GET BY ID -------------------------------------------------
        // GET /api/attractions/1
        [HttpGet("{id}")]
        public ActionResult<AttractionDTO> GetById(int id)
        {
            Attraction attraction = Attraction.Find(id);
            if (attraction == null)
                return NotFound($"Attraction with ID {id} not found!");
            return Ok(attraction.DTO);
        }

        // -- GET BY CITY -----------------------------------------------
        // GET /api/attractions/city/Petra
        [HttpGet("city/{city}")]
        public ActionResult<List<AttractionDTO>> GetByCity(string city)
        {
            var list = Attraction.GetByCity(city);
            if (list.Count == 0)
                return NotFound($"No attractions found in {city}!");
            return Ok(list);
        }

        // -- GET BY CATEGORY -------------------------------------------
        // GET /api/attractions/category/1
        [HttpGet("category/{categoryId}")]
        public ActionResult<List<AttractionDTO>> GetByCategory(int categoryId)
        {
            var list = Attraction.GetByCategory(categoryId);
            if (list.Count == 0)
                return NotFound($"No attractions found for category {categoryId}!");
            return Ok(list);
        }

        // -- CREATE ----------------------------------------------------
        // POST /api/attractions
        [HttpPost]
        public ActionResult<AttractionDTO> Create(AttractionDTO dto)
        {
            if (string.IsNullOrEmpty(dto.NameEn) || string.IsNullOrEmpty(dto.NameAr))
                return BadRequest("Name in Arabic and English are required!");

            var attraction = new Attraction(dto, Attraction.enMode.AddNew);

            if (attraction.Save())
                return CreatedAtAction(nameof(GetById), new { id = attraction.Id }, attraction.DTO);

            return StatusCode(500, "Failed to add attraction!");
        }

        // -- UPDATE ----------------------------------------------------
        // PUT /api/attractions/1
        [HttpPut("{id}")]
        public ActionResult<AttractionDTO> Update(int id, AttractionDTO dto)
        {
            Attraction attraction = Attraction.Find(id);
            if (attraction == null)
                return NotFound($"Attraction with ID {id} not found!");

            // Update properties
            attraction.CategoryId = dto.CategoryId;
            attraction.NameAr = dto.NameAr;
            attraction.NameEn = dto.NameEn;
            attraction.DescriptionAr = dto.DescriptionAr;
            attraction.DescriptionEn = dto.DescriptionEn;
            attraction.City = dto.City;
            attraction.Latitude = dto.Latitude;
            attraction.Longitude = dto.Longitude;
            attraction.ImageUrl = dto.ImageUrl;
            attraction.OpeningHours = dto.OpeningHours;
            attraction.EntryFee = dto.EntryFee;

            if (attraction.Save())
                return Ok(attraction.DTO);

            return StatusCode(500, "Failed to update attraction!");
        }

        // -- DELETE ----------------------------------------------------
        // DELETE /api/attractions/1
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Attraction attraction = Attraction.Find(id);
            if (attraction == null)
                return NotFound($"Attraction with ID {id} not found!");

            if (Attraction.Delete(id))
                return Ok($"Attraction with ID {id} deleted successfully!");

            return StatusCode(500, "Failed to delete attraction!");
        }
    }
}


