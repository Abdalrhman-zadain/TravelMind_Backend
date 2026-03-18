using Microsoft.Extensions.Configuration;

namespace TravelMind.DataAccess
{
    public static class DatabaseConfig
    {
        private static string _connectionString = null;

        public static void Initialize(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? "Data Source=.;Initial Catalog=TravelMindDB_New;Integrated Security=True;TrustServerCertificate=True;";
        }

        public static string GetConnectionString()
        {
            return _connectionString ?? "Data Source=.;Initial Catalog=TravelMindDB_New;Integrated Security=True;TrustServerCertificate=True;";
        }
    }
}
