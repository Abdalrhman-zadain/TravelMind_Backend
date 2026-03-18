using Microsoft.AspNetCore.Mvc;
using TravelMind.Business.Business;
using TravelMind.DataAccess.DTO;

namespace TravelMind.API.Controllers
{
    [Route("api/journals")]
    [ApiController]
    public class JournalsController : ControllerBase
    {
        // GET /api/journals/user/1
        [HttpGet("user/{userId}")]
        public ActionResult<List<JournalDTO>> GetByUserId(int userId)
        {
            var list = Journal.GetByUserId(userId);
            if (list.Count == 0)
                return NotFound($"No journals found for user {userId}!");
            return Ok(list);
        }

        // GET /api/journals/1
        [HttpGet("{id}")]
        public ActionResult<JournalDTO> GetById(int id)
        {
            Journal journal = Journal.Find(id);
            if (journal == null)
                return NotFound($"Journal with ID {id} not found!");
            return Ok(journal.DTO);
        }

        // POST /api/journals
        [HttpPost]
        public ActionResult<JournalDTO> Create(JournalDTO dto)
        {
            if (string.IsNullOrEmpty(dto.Title) || string.IsNullOrEmpty(dto.Content))
                return BadRequest("Title and content are required!");

            var journal = new Journal(dto, Journal.enMode.AddNew);
            if (journal.Save())
                return CreatedAtAction(nameof(GetById), new { id = journal.Id }, journal.DTO);

            return StatusCode(500, "Failed to add journal!");
        }

        // PUT /api/journals/1
        [HttpPut("{id}")]
        public ActionResult<JournalDTO> Update(int id, JournalDTO dto)
        {
            Journal journal = Journal.Find(id);
            if (journal == null)
                return NotFound($"Journal with ID {id} not found!");

            journal.Title = dto.Title;
            journal.Content = dto.Content;
            journal.Date = dto.Date;

            if (journal.Save())
                return Ok(journal.DTO);

            return StatusCode(500, "Failed to update journal!");
        }

        // DELETE /api/journals/1
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Journal journal = Journal.Find(id);
            if (journal == null)
                return NotFound($"Journal with ID {id} not found!");

            if (Journal.Delete(id))
                return Ok($"Journal with ID {id} deleted successfully!");

            return StatusCode(500, "Failed to delete journal!");
        }
    }
}
