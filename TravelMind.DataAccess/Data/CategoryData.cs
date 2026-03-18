using Microsoft.Data.SqlClient;
using System.Data;
using TravelMind.DataAccess.DTO;

namespace TravelMind.DataAccess.Data
{
    public class CategoryData
    {
        private static string GetConnectionString() => DatabaseConfig.GetConnectionString();

        private static CategoryDTO _mapReader(SqlDataReader reader)
        {
            return new CategoryDTO(
                reader.GetInt32(reader.GetOrdinal("Id")),
                reader.GetString(reader.GetOrdinal("NameAr")),
                reader.GetString(reader.GetOrdinal("NameEn")),
                reader.GetString(reader.GetOrdinal("Type"))
            );
        }

        public static List<CategoryDTO> GetAllCategories()
        {
            var list = new List<CategoryDTO>();
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_GetAllCategories", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        list.Add(_mapReader(reader));
            }
            return list;
        }

        public static CategoryDTO GetCategoryById(int id)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_GetCategoryById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CategoryId", id);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) return _mapReader(reader);
                    return null;
                }
            }
        }

        public static List<CategoryDTO> GetCategoriesByType(string type)
        {
            var list = new List<CategoryDTO>();
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_GetCategoriesByType", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        list.Add(_mapReader(reader));
            }
            return list;
        }

        public static int AddCategory(CategoryDTO dto)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_AddCategory", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NameAr", dto.NameAr);
                cmd.Parameters.AddWithValue("@NameEn", dto.NameEn);
                cmd.Parameters.AddWithValue("@Type", dto.Type);
                var outputId = new SqlParameter("@NewId", SqlDbType.Int)
                { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(outputId);
                conn.Open();
                cmd.ExecuteNonQuery();
                return (int)outputId.Value;
            }
        }

        public static bool UpdateCategory(CategoryDTO dto)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_UpdateCategory", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CategoryId", dto.Id);
                cmd.Parameters.AddWithValue("@NameAr", dto.NameAr);
                cmd.Parameters.AddWithValue("@NameEn", dto.NameEn);
                cmd.Parameters.AddWithValue("@Type", dto.Type);
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
        }

        public static bool DeleteCategory(int id)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("sp_DeleteCategory", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CategoryId", id);
                conn.Open();
                int rowsAffected = (int)cmd.ExecuteScalar();
                return rowsAffected == 1;
            }
        }
    }
}