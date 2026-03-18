using Microsoft.AspNetCore.Mvc;
using TravelMind.Business.Business;
using TravelMind.DataAccess.DTO;

namespace TravelMind.API.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        // GET /api/reviews/place/Attraction/1
        [HttpGet("place/{placeType}/{placeId}")]
        public ActionResult<List<ReviewDTO>> GetByPlace(string placeType, int placeId)
        {
            var list = Review.GetByPlace(placeType, placeId);
            if (list.Count == 0)
                return NotFound($"No reviews found for {placeType} with ID {placeId}!");
            return Ok(list);
        }

        // GET /api/reviews/user/1
        [HttpGet("user/{userId}")]
        public ActionResult<List<ReviewDTO>> GetByUserId(int userId)
        {
            var list = Review.GetByUserId(userId);
            if (list.Count == 0)
                return NotFound($"No reviews found for user {userId}!");
            return Ok(list);
        }

        // GET /api/reviews/1
        [HttpGet("{id}")]
        public ActionResult<ReviewDTO> GetById(int id)
        {
            Review review = Review.Find(id);
            if (review == null)
                return NotFound($"Review with ID {id} not found!");
            return Ok(review.DTO);
        }

        // POST /api/reviews
        [HttpPost]
        public ActionResult<ReviewDTO> Create(ReviewDTO dto)
        {
            if (dto.Rating < 1 || dto.Rating > 5)
                return BadRequest("Rating must be between 1 and 5!");

            var review = new Review(dto, Review.enMode.AddNew);
            if (review.Save())
                return CreatedAtAction(nameof(GetById), new { id = review.Id }, review.DTO);

            return StatusCode(500, "Failed to add review!");
        }

        // DELETE /api/reviews/1
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Review review = Review.Find(id);
            if (review == null)
                return NotFound($"Review with ID {id} not found!");

            if (Review.Delete(id))
                return Ok($"Review with ID {id} deleted successfully!");

            return StatusCode(500, "Failed to delete review!");
        }
    }
}
