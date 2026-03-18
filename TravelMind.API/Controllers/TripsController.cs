using Microsoft.AspNetCore.Mvc;
using TravelMind.Business.Business;
using TravelMind.DataAccess.DTO;

namespace TravelMind.API.Controllers
{
    [Route("api/trips")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        // GET /api/trips
        [HttpGet]
        public ActionResult<List<TripDTO>> GetAll()
        {
            var list = Trip.GetAll();
            if (list.Count == 0) return NoContent();
            return Ok(list);
        }

        // GET /api/trips/1
        [HttpGet("{id}")]
        public ActionResult<TripDTO> GetById(int id)
        {
            Trip trip = Trip.Find(id);
            if (trip == null)
                return NotFound($"Trip with ID {id} not found!");
            return Ok(trip.DTO);
        }

        // GET /api/trips/user/1
        [HttpGet("user/{userId}")]
        public ActionResult<List<TripDTO>> GetByUserId(int userId)
        {
            var list = Trip.GetByUserId(userId);
            if (list.Count == 0)
                return NotFound($"No trips found for user {userId}!");
            return Ok(list);
        }

        // POST /api/trips
        [HttpPost]
        public ActionResult<TripDTO> Create(TripDTO dto)
        {
            if (string.IsNullOrEmpty(dto.Name) || string.IsNullOrEmpty(dto.Destination))
                return BadRequest("Trip name and destination are required!");

            var trip = new Trip(dto, Trip.enMode.AddNew);
            if (trip.Save())
                return CreatedAtAction(nameof(GetById), new { id = trip.Id }, trip.DTO);

            return StatusCode(500, "Failed to add trip!");
        }

        // PUT /api/trips/1
        [HttpPut("{id}")]
        public ActionResult<TripDTO> Update(int id, TripDTO dto)
        {
            Trip trip = Trip.Find(id);
            if (trip == null)
                return NotFound($"Trip with ID {id} not found!");

            trip.Name = dto.Name;
            trip.Destination = dto.Destination;
            trip.StartDate = dto.StartDate;
            trip.EndDate = dto.EndDate;
            trip.Budget = dto.Budget;
            trip.Notes = dto.Notes;

            if (trip.Save())
                return Ok(trip.DTO);

            return StatusCode(500, "Failed to update trip!");
        }

        // DELETE /api/trips/1
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Trip trip = Trip.Find(id);
            if (trip == null)
                return NotFound($"Trip with ID {id} not found!");

            if (Trip.Delete(id))
                return Ok($"Trip with ID {id} deleted successfully!");

            return StatusCode(500, "Failed to delete trip!");
        }
    }
}
