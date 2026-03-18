using Microsoft.AspNetCore.Mvc;
using TravelMind.Business.Business;
using TravelMind.DataAccess.DTO;

namespace TravelMind.API.Controllers
{
    [Route("api/itineraries")]
    [ApiController]
    public class ItinerariesController : ControllerBase
    {
        // GET /api/itineraries/trip/1
        [HttpGet("trip/{tripId}")]
        public ActionResult<List<ItineraryDTO>> GetByTripId(int tripId)
        {
            var list = Itinerary.GetByTripId(tripId);
            if (list.Count == 0)
                return NotFound($"No itinerary found for trip {tripId}!");
            return Ok(list);
        }

        // GET /api/itineraries/1
        [HttpGet("{id}")]
        public ActionResult<ItineraryDTO> GetById(int id)
        {
            Itinerary itinerary = Itinerary.Find(id);
            if (itinerary == null)
                return NotFound($"Itinerary with ID {id} not found!");
            return Ok(itinerary.DTO);
        }

        // POST /api/itineraries
        [HttpPost]
        public ActionResult<ItineraryDTO> Create(ItineraryDTO dto)
        {
            if (dto.TripId == 0)
                return BadRequest("TripId is required!");

            var itinerary = new Itinerary(dto, Itinerary.enMode.AddNew);
            if (itinerary.Save())
                return CreatedAtAction(nameof(GetById), new { id = itinerary.Id }, itinerary.DTO);

            return StatusCode(500, "Failed to add itinerary!");
        }

        // PUT /api/itineraries/1
        [HttpPut("{id}")]
        public ActionResult<ItineraryDTO> Update(int id, ItineraryDTO dto)
        {
            Itinerary itinerary = Itinerary.Find(id);
            if (itinerary == null)
                return NotFound($"Itinerary with ID {id} not found!");

            itinerary.AttractionId = dto.AttractionId;
            itinerary.DayNumber = dto.DayNumber;
            itinerary.VisitTime = dto.VisitTime;
            itinerary.Notes = dto.Notes;

            if (itinerary.Save())
                return Ok(itinerary.DTO);

            return StatusCode(500, "Failed to update itinerary!");
        }

        // DELETE /api/itineraries/1
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Itinerary itinerary = Itinerary.Find(id);
            if (itinerary == null)
                return NotFound($"Itinerary with ID {id} not found!");

            if (Itinerary.Delete(id))
                return Ok($"Itinerary with ID {id} deleted successfully!");

            return StatusCode(500, "Failed to delete itinerary!");
        }
    }
}
