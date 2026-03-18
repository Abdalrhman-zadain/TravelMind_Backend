using Microsoft.Data.SqlClient;
using System.Data;
using TravelMind.DataAccess.DTO;

namespace TravelMind.DataAccess.Data
{
    public class TripData
    {
        private static string GetConnectionString() => DatabaseConfig.GetConnectionString();

        private static TripDTO _mapReader(SqlDataReader reader)
        {
            return new TripDTO(
                reader.GetInt32(reader.GetOrdinal("Id")),
                reader.GetInt32(reader.GetOrdinal("UserId")),
                reader.GetString(reader.GetOrdinal("Name")),
                reader.GetString(reader.GetOrdinal("Destination")),
                reader.IsDBNull(reader.GetOrdinal("StartDate")) ? null : reader.GetDateTime(reader.GetOrdinal("StartDate")),
                reader.IsDBNull(reader.GetOrdinal("EndDate")) ? null : reader.GetDateTime(reader.GetOrdinal("EndDate")),
                reader.GetDecimal(reader.GetOrdinal("Budget")),
                reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
            );
        }

        public static List<TripDTO> GetAllTrips()
        {
            var list = new List<TripDTO>();
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_GetAllTrips", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        list.Add(_mapReader(reader));
            }
            return list;
        }

        public static TripDTO GetTripById(int id)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_GetTripById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TripId", id);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) return _mapReader(reader);
                    return null;
                }
            }
        }

        public static List<TripDTO> GetTripsByUserId(int userId)
        {
            var list = new List<TripDTO>();
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_GetTripsByUserId", conn))
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

        public static int AddTrip(TripDTO dto)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_AddTrip", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", dto.UserId);
                cmd.Parameters.AddWithValue("@Name", dto.Name);
                cmd.Parameters.AddWithValue("@Destination", dto.Destination);
                cmd.Parameters.AddWithValue("@StartDate", dto.StartDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@EndDate", dto.EndDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Budget", dto.Budget);
                cmd.Parameters.AddWithValue("@Notes", dto.Notes ?? (object)DBNull.Value);
                var outputId = new SqlParameter("@NewId", SqlDbType.Int)
                { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(outputId);
                conn.Open();
                cmd.ExecuteNonQuery();
                return (int)outputId.Value;
            }
        }

        public static bool UpdateTrip(TripDTO dto)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_UpdateTrip", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TripId", dto.Id);
                cmd.Parameters.AddWithValue("@Name", dto.Name);
                cmd.Parameters.AddWithValue("@Destination", dto.Destination);
                cmd.Parameters.AddWithValue("@StartDate", dto.StartDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@EndDate", dto.EndDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Budget", dto.Budget);
                cmd.Parameters.AddWithValue("@Notes", dto.Notes ?? (object)DBNull.Value);
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
        }

        public static bool DeleteTrip(int id)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_DeleteTrip", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TripId", id);
                conn.Open();
                int rowsAffected = (int)cmd.ExecuteScalar();
                return rowsAffected == 1;
            }
        }
    }
}