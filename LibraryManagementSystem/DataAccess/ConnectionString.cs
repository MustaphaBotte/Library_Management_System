namespace LMS.DataAccess
{
    class ConnectionString
    {
        private static string _ConnectionString= @"Server=.\MSSQLSERVER1;Database=LMS; User=sa;Password=123456;TrustServerCertificate=True;";
        public static string GetConnectionString()
        {
            return _ConnectionString;
        }
    }
}
