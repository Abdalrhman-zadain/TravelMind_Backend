using Microsoft.Data.SqlClient;
using System.Data;
using TravelMind.DataAccess.DTO;

namespace TravelMind.DataAccess.Data
{
    public class ItineraryData
    {
        private static string GetConnectionString() => DatabaseConfig.GetConnectionString();

        private static ItineraryDTO _mapReader(SqlDataReader reader)
        {
            return new ItineraryDTO(
                reader.GetInt32(reader.GetOrdinal("Id")),
                reader.GetInt32(reader.GetOrdinal("TripId")),
                reader.IsDBNull(reader.GetOrdinal("AttractionId")) ? null : reader.GetInt32(reader.GetOrdinal("AttractionId")),
                reader.GetInt32(reader.GetOrdinal("DayNumber")),
                reader.IsDBNull(reader.GetOrdinal("VisitTime")) ? null : reader.GetString(reader.GetOrdinal("VisitTime")),
                reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes"))
            );
        }

        public static List<ItineraryDTO> GetItinerariesByTripId(int tripId)
        {
            var list = new List<ItineraryDTO>();
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_GetItinerariesByTripId", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TripId", tripId);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        list.Add(_mapReader(reader));
            }
            return list;
        }

        public static ItineraryDTO GetItineraryById(int id)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_GetItineraryById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItineraryId", id);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) return _mapReader(reader);
                    return null;
                }
            }
        }

        public static int AddItinerary(ItineraryDTO dto)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_AddItinerary", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TripId", dto.TripId);
                cmd.Parameters.AddWithValue("@AttractionId", dto.AttractionId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@DayNumber", dto.DayNumber);
                cmd.Parameters.AddWithValue("@VisitTime", dto.VisitTime ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Notes", dto.Notes ?? (object)DBNull.Value);
                var outputId = new SqlParameter("@NewId", SqlDbType.Int)
                { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(outputId);
                conn.Open();
                cmd.ExecuteNonQuery();
                return (int)outputId.Value;
            }
        }

        public static bool UpdateItinerary(ItineraryDTO dto)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_UpdateItinerary", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItineraryId", dto.Id);
                cmd.Parameters.AddWithValue("@AttractionId", dto.AttractionId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@DayNumber", dto.DayNumber);
                cmd.Parameters.AddWithValue("@VisitTime", dto.VisitTime ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Notes", dto.Notes ?? (object)DBNull.Value);
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
        }

        public static bool DeleteItinerary(int id)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_DeleteItinerary", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItineraryId", id);
                conn.Open();
                int rowsAffected = (int)cmd.ExecuteScalar();
                return rowsAffected == 1;
            }
        }
    }
}