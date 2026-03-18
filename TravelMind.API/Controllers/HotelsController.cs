using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelMind.Business.Business;
using TravelMind.DataAccess.DTO;

namespace TravelMind.API.Controllers
{
    [Route("api/hotels")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        // GET /api/hotels
        [HttpGet]
        public ActionResult<List<HotelDTO>> GetAll()
        {
            var list = Hotel.GetAll();
            if (list.Count == 0) return NoContent();
            return Ok(list);
        }

        // GET /api/hotels/1
        [HttpGet("{id}")]
        public ActionResult<HotelDTO> GetById(int id)
        {
            Hotel hotel = Hotel.Find(id);
            if (hotel == null)
                return NotFound($"Hotel with ID {id} not found!");
            return Ok(hotel.DTO);
        }

        // GET /api/hotels/city/Amman
        [HttpGet("city/{city}")]
        public ActionResult<List<HotelDTO>> GetByCity(string city)
        {
            var list = Hotel.GetByCity(city);
            if (list.Count == 0)
                return NotFound($"No hotels found in {city}!");
            return Ok(list);
        }

        // GET /api/hotels/stars/5
        [HttpGet("stars/{stars}")]
        public ActionResult<List<HotelDTO>> GetByStars(int stars)
        {
            var list = Hotel.GetByStars(stars);
            if (list.Count == 0)
                return NotFound($"No {stars}-star hotels found!");
            return Ok(list);
        }

        // POST /api/hotels
        [HttpPost]
        public ActionResult<HotelDTO> Create(HotelDTO dto)
        {
            if (string.IsNullOrEmpty(dto.NameEn) || string.IsNullOrEmpty(dto.NameAr))
                return BadRequest("Name in Arabic and English are required!");

            var hotel = new Hotel(dto, Hotel.enMode.AddNew);
            if (hotel.Save())
                return CreatedAtAction(nameof(GetById), new { id = hotel.Id }, hotel.DTO);

            return StatusCode(500, "Failed to add hotel!");
        }

        // PUT /api/hotels/1
        [HttpPut("{id}")]
        public ActionResult<HotelDTO> Update(int id, HotelDTO dto)
        {
            Hotel hotel = Hotel.Find(id);
            if (hotel == null)
                return NotFound($"Hotel with ID {id} not found!");

            hotel.CategoryId = dto.CategoryId;
            hotel.NameAr = dto.NameAr;
            hotel.NameEn = dto.NameEn;
            hotel.DescriptionAr = dto.DescriptionAr;
            hotel.DescriptionEn = dto.DescriptionEn;
            hotel.City = dto.City;
            hotel.Latitude = dto.Latitude;
            hotel.Longitude = dto.Longitude;
            hotel.ImageUrl = dto.ImageUrl;
            hotel.Stars = dto.Stars;
            hotel.PricePerNight = dto.PricePerNight;
            hotel.Phone = dto.Phone;
            hotel.Website = dto.Website;

            if (hotel.Save())
                return Ok(hotel.DTO);

            return StatusCode(500, "Failed to update hotel!");
        }

        // DELETE /api/hotels/1
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Hotel hotel = Hotel.Find(id);
            if (hotel == null)
                return NotFound($"Hotel with ID {id} not found!");

            if (Hotel.Delete(id))
                return Ok($"Hotel with ID {id} deleted successfully!");

            return StatusCode(500, "Failed to delete hotel!");
        }
    }
}

