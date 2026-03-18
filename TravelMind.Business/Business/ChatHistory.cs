using TravelMind.Business.Business;
using TravelMind.DataAccess.Data;
using TravelMind.DataAccess.DTO;

namespace TravelMind.Business.Business
{
    public class ChatHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public string Response { get; set; }
        public string Language { get; set; }
        public DateTime CreatedAt { get; set; }

        public ChatHistoryDTO DTO => new ChatHistoryDTO(
            Id, UserId, Message, Response, Language, CreatedAt
        );

        public ChatHistory(ChatHistoryDTO dto)
        {
            Id = dto.Id;
            UserId = dto.UserId;
            Message = dto.Message;
            Response = dto.Response;
            Language = dto.Language;
            CreatedAt = dto.CreatedAt;
        }

        public static List<ChatHistoryDTO> GetByUserId(int userId)
        {
            return ChatHistoryData.GetChatHistoryByUserId(userId);
        }

        public static int AddMessage(ChatHistoryDTO dto)
        {
            return ChatHistoryData.AddChatMessage(dto);
        }

        public static bool ClearHistory(int userId)
        {
            return ChatHistoryData.DeleteChatHistoryByUserId(userId);
        }
    }
}