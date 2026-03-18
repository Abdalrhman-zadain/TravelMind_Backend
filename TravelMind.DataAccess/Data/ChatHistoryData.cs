using Microsoft.Data.SqlClient;
using System.Data;
using TravelMind.DataAccess.Data;
using TravelMind.DataAccess.DTO;

namespace TravelMind.DataAccess.Data
{
    public class ChatHistoryData
    {
        private static string GetConnectionString() => DatabaseConfig.GetConnectionString();

        private static ChatHistoryDTO _mapReader(SqlDataReader reader)
        {
            return new ChatHistoryDTO(
                reader.GetInt32(reader.GetOrdinal("Id")),
                reader.GetInt32(reader.GetOrdinal("UserId")),
                reader.GetString(reader.GetOrdinal("Message")),
                reader.GetString(reader.GetOrdinal("Response")),
                reader.GetString(reader.GetOrdinal("Language")),
                reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
            );
        }

        public static List<ChatHistoryDTO> GetChatHistoryByUserId(int userId)
        {
            var list = new List<ChatHistoryDTO>();
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_GetChatHistoryByUserId", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        list.Add(_mapReader(reader));
            }
            return list;
        }

        public static int AddChatMessage(ChatHistoryDTO dto)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_AddChatMessage", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", dto.UserId);
                cmd.Parameters.AddWithValue("@Message", dto.Message);
                cmd.Parameters.AddWithValue("@Response", dto.Response);
                cmd.Parameters.AddWithValue("@Language", dto.Language ?? "en");
                var outputId = new SqlParameter("@NewId", SqlDbType.Int)
                { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(outputId);
                conn.Open();
                cmd.ExecuteNonQuery();
                return (int)outputId.Value;
            }
        }

        public static bool DeleteChatHistoryByUserId(int userId)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_DeleteChatHistoryByUserId", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                conn.Open();
                int rowsAffected = (int)cmd.ExecuteScalar();
                return rowsAffected >= 1;
            }
        }
    }
}
