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
    public class AttractionData
    {
        private static string GetConnectionString() => DatabaseConfig.GetConnectionString();

        private static AttractionDTO _mapReader(SqlDataReader reader)
        {
            return new AttractionDTO(
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
                reader.IsDBNull(reader.GetOrdinal("OpeningHours")) ? null : reader.GetString(reader.GetOrdinal("OpeningHours")),
                reader.GetDecimal(reader.GetOrdinal("EntryFee")),
                reader.GetDecimal(reader.GetOrdinal("Rating")),
                reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
            );
        }

        public static List<AttractionDTO> GetAllAttractions()
        {
            var list = new List<AttractionDTO>();

            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_GetAllAttractions", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        list.Add(_mapReader(reader));
                }
            }
            return list;
        }

        public static AttractionDTO GetAttractionById(int id)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_GetAttractionById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AttractionId", id);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        return _mapReader(reader);
                    return null;
                }
            }
        }

        public static List<AttractionDTO> GetAttractionsByCity(string city)
        {
            var list = new List<AttractionDTO>();

            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_GetAttractionsByCity", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@City", city);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        list.Add(_mapReader(reader));
                }
            }
            return list;
        }

        public static List<AttractionDTO> GetAttractionsByCategory(int categoryId)
        {
            var list = new List<AttractionDTO>();

            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_GetAttractionsByCategory", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        list.Add(_mapReader(reader));
                }
            }
            return list;
        }

        public static int AddAttraction(AttractionDTO dto)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_AddAttraction", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CategoryId", dto.CategoryId);
                cmd.Parameters.AddWithValue("@NameAr", dto.NameAr);
                cmd.Parameters.AddWithValue("@NameEn", dto.NameEn);
                //Here, if dto.DescriptionAr is null, then (object)DBNull.Value is used instead.
                //This is important for database operations, as SQL expects DBNull.Value for null fields
                cmd.Parameters.AddWithValue("@DescriptionAr", dto.DescriptionAr ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@DescriptionEn", dto.DescriptionEn ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@City", dto.City);
                cmd.Parameters.AddWithValue("@Latitude", dto.Latitude ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Longitude", dto.Longitude ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ImageUrl", dto.ImageUrl ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@OpeningHours", dto.OpeningHours ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@EntryFee", dto.EntryFee);

                var outputId = new SqlParameter("@NewId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputId);

                conn.Open();
                cmd.ExecuteNonQuery();

                return (int)outputId.Value;
            }
        }

        public static bool UpdateAttraction(AttractionDTO dto)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_UpdateAttraction", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@AttractionId", dto.Id);
                cmd.Parameters.AddWithValue("@CategoryId", dto.CategoryId);
                cmd.Parameters.AddWithValue("@NameAr", dto.NameAr);
                cmd.Parameters.AddWithValue("@NameEn", dto.NameEn);
                cmd.Parameters.AddWithValue("@DescriptionAr", dto.DescriptionAr ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@DescriptionEn", dto.DescriptionEn ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@City", dto.City);
                cmd.Parameters.AddWithValue("@Latitude", dto.Latitude ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Longitude", dto.Longitude ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ImageUrl", dto.ImageUrl ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@OpeningHours", dto.OpeningHours ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@EntryFee", dto.EntryFee);

                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
        }

        public static bool DeleteAttraction(int id)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_DeleteAttraction", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AttractionId", id);

                conn.Open();
                int rowsAffected = (int)cmd.ExecuteScalar();
                return rowsAffected == 1;
            }
        }
    }
}


