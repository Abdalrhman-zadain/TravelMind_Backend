using TravelMind.DataAccess;

namespace TravelMind.DataAccess.DTO
{
    public class ChatHistoryDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public string Response { get; set; }
        public string Language { get; set; }
        public DateTime CreatedAt { get; set; }

        public ChatHistoryDTO(int id, int userId, string message,
            string response, string language, DateTime createdAt)
        {
            Id = id;
            UserId = userId;
            Message = message;
            Response = response;
            Language = language;
            CreatedAt = createdAt;
        }
    }
}