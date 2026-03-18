using Microsoft.Data.SqlClient;
using System.Data;
using TravelMind.DataAccess.DTO;

namespace TravelMind.DataAccess.Data
{
    public class RestaurantData
    {
        private static string GetConnectionString() => DatabaseConfig.GetConnectionString();

        private static RestaurantDTO _mapReader(SqlDataReader reader)
        {
            return new RestaurantDTO(
                reader.GetInt32(reader.GetOrdinal("Id")),
                reader.GetInt32(reader.GetOrdinal("CategoryId")),
                reader.GetString(reader.GetOrdinal("NameAr")),
                reader.GetString(reader.GetOrdinal("NameEn")),
                reader.IsDBNull(reader.GetOrdinal("DescriptionAr")) ? null : reader.GetString(reader.GetOrdinal("DescriptionAr")),
                reader.IsDBNull(reader.GetOrdinal("DescriptionEn")) ? null : reader.GetString(reader.GetOrdinal("DescriptionEn")),
                reader.GetString(reader.GetOrdinal("City")),
                reader.IsDBNull(reader.GetOrdinal("Latitude")) ? null : reader.GetDecimal(reader.GetOrdinal("Latitude")),
                reader.IsDBNull(reader.GetOrdinal("Longitude")) ? null : reader.GetDecimal(reader.GetOrdinal("Longitude")),
                reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString(reader.GetOrdinal("ImageUrl")),
                reader.IsDBNull(reader.GetOrdinal("Cuisine")) ? null : reader.GetString(reader.GetOrdinal("Cuisine")),
                reader.IsDBNull(reader.GetOrdinal("PriceRange")) ? null : reader.GetString(reader.GetOrdinal("PriceRange")),
                reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                reader.GetDecimal(reader.GetOrdinal("Rating")),
                reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
            );
        }

        public static List<RestaurantDTO> GetAllRestaurants()
        {
            var list = new List<RestaurantDTO>();
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_GetAllRestaurants", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        list.Add(_mapReader(reader));
            }
            return list;
        }

        public static RestaurantDTO GetRestaurantById(int id)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_GetRestaurantById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RestaurantId", id);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) return _mapReader(reader);
                    return null;
                }
            }
        }

        public static List<RestaurantDTO> GetRestaurantsByCity(string city)
        {
            var list = new List<RestaurantDTO>();
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_GetRestaurantsByCity", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@City", city);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        list.Add(_mapReader(reader));
            }
            return list;
        }

        public static List<RestaurantDTO> GetRestaurantsByCuisine(string cuisine)
        {
            var list = new List<RestaurantDTO>();
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_GetRestaurantsByCuisine", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Cuisine", cuisine);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        list.Add(_mapReader(reader));
            }
            return list;
        }

        public static int AddRestaurant(RestaurantDTO dto)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_AddRestaurant", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CategoryId", dto.CategoryId);
                cmd.Parameters.AddWithValue("@NameAr", dto.NameAr);
                cmd.Parameters.AddWithValue("@NameEn", dto.NameEn);
                cmd.Parameters.AddWithValue("@DescriptionAr", dto.DescriptionAr ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@DescriptionEn", dto.DescriptionEn ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@City", dto.City);
                cmd.Parameters.AddWithValue("@Latitude", dto.Latitude ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Longitude", dto.Longitude ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ImageUrl", dto.ImageUrl ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Cuisine", dto.Cuisine ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PriceRange", dto.PriceRange ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Phone", dto.Phone ?? (object)DBNull.Value);

                var outputId = new SqlParameter("@NewId", SqlDbType.Int)
                { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(outputId);

                conn.Open();
                cmd.ExecuteNonQuery();
                return (int)outputId.Value;
            }
        }

        public static bool UpdateRestaurant(RestaurantDTO dto)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_UpdateRestaurant", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RestaurantId", dto.Id);
                cmd.Parameters.AddWithValue("@CategoryId", dto.CategoryId);
                cmd.Parameters.AddWithValue("@NameAr", dto.NameAr);
                cmd.Parameters.AddWithValue("@NameEn", dto.NameEn);
                cmd.Parameters.AddWithValue("@DescriptionAr", dto.DescriptionAr ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@DescriptionEn", dto.DescriptionEn ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@City", dto.City);
                cmd.Parameters.AddWithValue("@Latitude", dto.Latitude ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Longitude", dto.Longitude ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ImageUrl", dto.ImageUrl ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Cuisine", dto.Cuisine ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PriceRange", dto.PriceRange ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Phone", dto.Phone ?? (object)DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
        }

        public static bool DeleteRestaurant(int id)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_DeleteRestaurant", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RestaurantId", id);
                conn.Open();
                int rowsAffected = (int)cmd.ExecuteScalar();
                return rowsAffected == 1;
            }
        }
    }
}