using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelMind.DataAccess.DTO;

namespace TravelMind.DataAccess.Data
{
    public class HotelData
    {
        private static string GetConnectionString() => DatabaseConfig.GetConnectionString();

        private static HotelDTO _mapReader(SqlDataReader reader)
        {
            return new HotelDTO(
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
                reader.GetInt32(reader.GetOrdinal("Stars")),
                reader.GetDecimal(reader.GetOrdinal("PricePerNight")),
                reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                reader.IsDBNull(reader.GetOrdinal("Website")) ? null : reader.GetString(reader.GetOrdinal("Website")),
                reader.GetDecimal(reader.GetOrdinal("Rating")),
                reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
            );
        }

        public static List<HotelDTO> GetAllHotels()
        {
            var list = new List<HotelDTO>();
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_GetAllHotels", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        list.Add(_mapReader(reader));
            }
            return list;
        }

        public static HotelDTO GetHotelById(int id)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_GetHotelById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HotelId", id);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) return _mapReader(reader);
                    return null;
                }
            }
        }

        public static List<HotelDTO> GetHotelsByCity(string city)
        {
            var list = new List<HotelDTO>();
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_GetHotelsByCity", conn))
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

        public static List<HotelDTO> GetHotelsByStars(int stars)
        {
            var list = new List<HotelDTO>();
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_GetHotelsByStars", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Stars", stars);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        list.Add(_mapReader(reader));
            }
            return list;
        }

        public static int AddHotel(HotelDTO dto)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_AddHotel", conn))
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
                cmd.Parameters.AddWithValue("@Stars", dto.Stars);
                cmd.Parameters.AddWithValue("@PricePerNight", dto.PricePerNight);
                cmd.Parameters.AddWithValue("@Phone", dto.Phone ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Website", dto.Website ?? (object)DBNull.Value);

                var outputId = new SqlParameter("@NewId", SqlDbType.Int)
                { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(outputId);

                conn.Open();
                cmd.ExecuteNonQuery();
                return (int)outputId.Value;
            }
        }

        public static bool UpdateHotel(HotelDTO dto)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_UpdateHotel", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HotelId", dto.Id);
                cmd.Parameters.AddWithValue("@CategoryId", dto.CategoryId);
                cmd.Parameters.AddWithValue("@NameAr", dto.NameAr);
                cmd.Parameters.AddWithValue("@NameEn", dto.NameEn);
                cmd.Parameters.AddWithValue("@DescriptionAr", dto.DescriptionAr ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@DescriptionEn", dto.DescriptionEn ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@City", dto.City);
                cmd.Parameters.AddWithValue("@Latitude", dto.Latitude ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Longitude", dto.Longitude ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ImageUrl", dto.ImageUrl ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Stars", dto.Stars);
                cmd.Parameters.AddWithValue("@PricePerNight", dto.PricePerNight);
                cmd.Parameters.AddWithValue("@Phone", dto.Phone ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Website", dto.Website ?? (object)DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
        }

        public static bool DeleteHotel(int id)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_DeleteHotel", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HotelId", id);
                conn.Open();
                int rowsAffected = (int)cmd.ExecuteScalar();
                return rowsAffected == 1;
            }
        }
    }
}
