using Microsoft.Data.SqlClient;
using System.Data;
using TravelMind.DataAccess.DTO;

namespace TravelMind.DataAccess.Data
{
    public class ReviewData
    {
        private static string GetConnectionString() => DatabaseConfig.GetConnectionString();

        private static ReviewDTO _mapReader(SqlDataReader reader)
        {
            return new ReviewDTO(
                reader.GetInt32(reader.GetOrdinal("Id")),
                reader.GetInt32(reader.GetOrdinal("UserId")),
                reader.GetString(reader.GetOrdinal("PlaceType")),
                reader.GetInt32(reader.GetOrdinal("PlaceId")),
                reader.GetInt32(reader.GetOrdinal("Rating")),
                reader.IsDBNull(reader.GetOrdinal("Comment")) ? null : reader.GetString(reader.GetOrdinal("Comment")),
                reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
            );
        }

        public static List<ReviewDTO> GetReviewsByPlace(string placeType, int placeId)
        {
            var list = new List<ReviewDTO>();
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_GetReviewsByPlace", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlaceType", placeType);
                cmd.Parameters.AddWithValue("@PlaceId", placeId);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        list.Add(_mapReader(reader));
            }
            return list;
        }

        public static List<ReviewDTO> GetReviewsByUserId(int userId)
        {
            var list = new List<ReviewDTO>();
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_GetReviewsByUserId", conn))
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

        public static ReviewDTO GetReviewById(int id)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_GetReviewById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ReviewId", id);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) return _mapReader(reader);
                    return null;
                }
            }
        }

        public static int AddReview(ReviewDTO dto)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_AddReview", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", dto.UserId);
                cmd.Parameters.AddWithValue("@PlaceType", dto.PlaceType);
                cmd.Parameters.AddWithValue("@PlaceId", dto.PlaceId);
                cmd.Parameters.AddWithValue("@Rating", dto.Rating);
                cmd.Parameters.AddWithValue("@Comment", dto.Comment ?? (object)DBNull.Value);
                var outputId = new SqlParameter("@NewId", SqlDbType.Int)
                { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(outputId);
                conn.Open();
                cmd.ExecuteNonQuery();
                return (int)outputId.Value;
            }
        }

        public static bool DeleteReview(int id)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_DeleteReview", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ReviewId", id);
                conn.Open();
                int rowsAffected = (int)cmd.ExecuteScalar();
                return rowsAffected == 1;
            }
        }
    }
}