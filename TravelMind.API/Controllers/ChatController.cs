using Microsoft.AspNetCore.Mvc;
using TravelMind.Business.Business;
using TravelMind.DataAccess.DTO;

namespace TravelMind.API.Controllers
{
    [Route("api/chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        // GET /api/chat/user/1
        [HttpGet("user/{userId}")]
        public ActionResult<List<ChatHistoryDTO>> GetHistory(int userId)
        {
            var list = ChatHistory.GetByUserId(userId);
            if (list.Count == 0)
                return NotFound($"No chat history found for user {userId}!");
            return Ok(list);
        }

        // POST /api/chat
        [HttpPost]
        public ActionResult<ChatHistoryDTO> SendMessage(ChatHistoryDTO dto)
        {
            if (string.IsNullOrEmpty(dto.Message))
                return BadRequest("Message cannot be empty!");

            int newId = ChatHistory.AddMessage(dto);
            if (newId != -1)
                return Ok(new { id = newId, message = "Message saved successfully!" });

            return StatusCode(500, "Failed to save message!");
        }

        // DELETE /api/chat/user/1
        [HttpDelete("user/{userId}")]
        public ActionResult ClearHistory(int userId)
        {
            if (ChatHistory.ClearHistory(userId))
                return Ok($"Chat history cleared for user {userId}!");

            return StatusCode(500, "Failed to clear chat history!");
        }
    }
}

