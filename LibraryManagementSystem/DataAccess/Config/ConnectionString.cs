namespace LMS.DataAccess.Config
{
    class ConnectionString
    {
        private static string _connectionString= @"Server=.\MSSQLSERVER1;Database=LMS; User=sa;Password=123456;TrustServerCertificate=True;";
        public static string Value
        {
            get => _connectionString;
        }
    }
}
