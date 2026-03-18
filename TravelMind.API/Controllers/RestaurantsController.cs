using Microsoft.AspNetCore.Mvc;
using System;
using TravelMind.Business.Business;
using TravelMind.DataAccess.DTO;

namespace TravelMind.API.Controllers
{
    [Route("api/restaurants")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        // GET /api/restaurants
        [HttpGet]
        public ActionResult<List<RestaurantDTO>> GetAll()
        {
            var list = Restaurant.GetAll();
            if (list.Count == 0) return NoContent();
            return Ok(list);
        }

        // GET /api/restaurants/1
        [HttpGet("{id}")]
        public ActionResult<RestaurantDTO> GetById(int id)
        {
            Restaurant restaurant = Restaurant.Find(id);
            if (restaurant == null)
                return NotFound($"Restaurant with ID {id} not found!");
            return Ok(restaurant.DTO);
        }

        // GET /api/restaurants/city/Amman
        [HttpGet("city/{city}")]
        public ActionResult<List<RestaurantDTO>> GetByCity(string city)
        {
            var list = Restaurant.GetByCity(city);
            if (list.Count == 0)
                return NotFound($"No restaurants found in {city}!");
            return Ok(list);
        }

        // GET /api/restaurants/cuisine/Arabic
        [HttpGet("cuisine/{cuisine}")]
        public ActionResult<List<RestaurantDTO>> GetByCuisine(string cuisine)
        {
            var list = Restaurant.GetByCuisine(cuisine);
            if (list.Count == 0)
                return NotFound($"No {cuisine} restaurants found!");
            return Ok(list);
        }

        // POST /api/restaurants
        [HttpPost]
        public ActionResult<RestaurantDTO> Create(RestaurantDTO dto)
        {
            if (string.IsNullOrEmpty(dto.NameEn) || string.IsNullOrEmpty(dto.NameAr))
                return BadRequest("Name in Arabic and English are required!");

            var restaurant = new Restaurant(dto, Restaurant.enMode.AddNew);
            if (restaurant.Save())
                return CreatedAtAction(nameof(GetById), new { id = restaurant.Id }, restaurant.DTO);

            return StatusCode(500, "Failed to add restaurant!");
        }

        // PUT /api/restaurants/1
        [HttpPut("{id}")]
        public ActionResult<RestaurantDTO> Update(int id, RestaurantDTO dto)
        {
            Restaurant restaurant = Restaurant.Find(id);
            if (restaurant == null)
                return NotFound($"Restaurant with ID {id} not found!");

            restaurant.CategoryId = dto.CategoryId;
            restaurant.NameAr = dto.NameAr;
            restaurant.NameEn = dto.NameEn;
            restaurant.DescriptionAr = dto.DescriptionAr;
            restaurant.DescriptionEn = dto.DescriptionEn;
            restaurant.City = dto.City;
            restaurant.Latitude = dto.Latitude;
            restaurant.Longitude = dto.Longitude;
            restaurant.ImageUrl = dto.ImageUrl;
            restaurant.Cuisine = dto.Cuisine;
            restaurant.PriceRange = dto.PriceRange;
            restaurant.Phone = dto.Phone;

            if (restaurant.Save())
                return Ok(restaurant.DTO);

            return StatusCode(500, "Failed to update restaurant!");
        }

        // DELETE /api/restaurants/1
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Restaurant restaurant = Restaurant.Find(id);
            if (restaurant == null)
                return NotFound($"Restaurant with ID {id} not found!");

            if (Restaurant.Delete(id))
                return Ok($"Restaurant with ID {id} deleted successfully!");

            return StatusCode(500, "Failed to delete restaurant!");
        }
    }
}

