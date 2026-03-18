using Microsoft.Data.SqlClient;
using System.Data;
using TravelMind.DataAccess.DTO;

namespace TravelMind.DataAccess.Data
{
    public class JournalData
    {
        private static string GetConnectionString() => DatabaseConfig.GetConnectionString();

        private static JournalDTO _mapReader(SqlDataReader reader)
        {
            return new JournalDTO(
                reader.GetInt32(reader.GetOrdinal("Id")),
                reader.GetInt32(reader.GetOrdinal("UserId")),
                reader.IsDBNull(reader.GetOrdinal("TripId")) ? null : reader.GetInt32(reader.GetOrdinal("TripId")),
                reader.GetString(reader.GetOrdinal("Title")),
                reader.GetString(reader.GetOrdinal("Content")),
                reader.IsDBNull(reader.GetOrdinal("Date")) ? null : reader.GetDateTime(reader.GetOrdinal("Date")),
                reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
            );
        }

        public static List<JournalDTO> GetJournalsByUserId(int userId)
        {
            var list = new List<JournalDTO>();
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_GetJournalsByUserId", conn))
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

        public static JournalDTO GetJournalById(int id)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_GetJournalById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@JournalId", id);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) return _mapReader(reader);
                    return null;
                }
            }
        }

        public static int AddJournal(JournalDTO dto)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_AddJournal", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", dto.UserId);
                cmd.Parameters.AddWithValue("@TripId", dto.TripId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Title", dto.Title);
                cmd.Parameters.AddWithValue("@Content", dto.Content);
                cmd.Parameters.AddWithValue("@Date", dto.Date ?? (object)DBNull.Value);
                var outputId = new SqlParameter("@NewId", SqlDbType.Int)
                { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(outputId);
                conn.Open();
                cmd.ExecuteNonQuery();
                return (int)outputId.Value;
            }
        }

        public static bool UpdateJournal(JournalDTO dto)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_UpdateJournal", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@JournalId", dto.Id);
                cmd.Parameters.AddWithValue("@Title", dto.Title);
                cmd.Parameters.AddWithValue("@Content", dto.Content);
                cmd.Parameters.AddWithValue("@Date", dto.Date ?? (object)DBNull.Value);
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
        }

        public static bool DeleteJournal(int id)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_DeleteJournal", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@JournalId", id);
                conn.Open();
                int rowsAffected = (int)cmd.ExecuteScalar();
                return rowsAffected == 1;
            }
        }
    }
}