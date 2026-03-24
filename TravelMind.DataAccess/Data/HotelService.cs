using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TravelMind.DataAccess.DTO;

namespace TravelMind.DataAccess.Data
{
    public class HotelService
    {
        private readonly string _connectionString;
        private readonly string _apiKey = "YOUR_API_KEY_HERE"; // Replace with your actual API key

        public HotelService(string connectionString)
        {
            _connectionString = connectionString;
        }


        public async Task<List<HotelApiDTO>> FetchHotelsFromApiAsync()
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-API-KEY", _apiKey);
            var url = "https://api.hotels-api.com/v1/hotels/search?country=Jordan&limit=10";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var root = JsonSerializer.Deserialize<HotelApiResponseDTO>(json, options);
            return root?.Hotels ?? new List<HotelApiDTO>();
        }

        public async Task SaveHotelsToDatabaseAsync(List<HotelApiDTO> hotels)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var createTableCmd = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Hotels' AND xtype='U')
                CREATE TABLE Hotels (
                    Id NVARCHAR(50) PRIMARY KEY,
                    Name NVARCHAR(200),
                    City NVARCHAR(100),
                    Country NVARCHAR(100),
                    Lat FLOAT,
                    Lng FLOAT,
                    Rating FLOAT,
                    Amenities NVARCHAR(MAX)
                )";
            using (var cmd = new SqlCommand(createTableCmd, conn))
            {
                await cmd.ExecuteNonQueryAsync();
            }
            foreach (var hotel in hotels)
            {
                var checkCmd = new SqlCommand("SELECT COUNT(*) FROM Hotels WHERE Id = @Id", conn);
                checkCmd.Parameters.AddWithValue("@Id", hotel.Id);
                var exists = (int)await checkCmd.ExecuteScalarAsync() > 0;
                if (exists) continue;
                var insertCmd = new SqlCommand(@"
                    INSERT INTO Hotels (Id, Name, City, Country, Lat, Lng, Rating, Amenities)
                    VALUES (@Id, @Name, @City, @Country, @Lat, @Lng, @Rating, @Amenities)", conn);
                insertCmd.Parameters.AddWithValue("@Id", hotel.Id);
                insertCmd.Parameters.AddWithValue("@Name", hotel.Name ?? (object)DBNull.Value);
                insertCmd.Parameters.AddWithValue("@City", hotel.City ?? (object)DBNull.Value);
                insertCmd.Parameters.AddWithValue("@Country", hotel.Country ?? (object)DBNull.Value);
                insertCmd.Parameters.AddWithValue("@Lat", hotel.Lat);
                insertCmd.Parameters.AddWithValue("@Lng", hotel.Lng);
                insertCmd.Parameters.AddWithValue("@Rating", hotel.Rating);
                insertCmd.Parameters.AddWithValue("@Amenities", string.Join(",", hotel.Amenities ?? new List<string>()));
                await insertCmd.ExecuteNonQueryAsync();
            }
        }
    }


}
