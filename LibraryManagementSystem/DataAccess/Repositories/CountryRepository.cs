namespace LMS.DataAccess.Repositories
{
    public class CountryRepository
    {
        public static async Task<List<CountryEntiry>?> GetAllCountries(uint CountryId)
        {
            using(SqlConnection connection = new SqlConnection(ConnectionString.Value))
            {
                using (SqlCommand command = new SqlCommand("select * from countries"))
                {
                    await connection.OpenAsync();
                    SqlDataReader Reader =await command.ExecuteReaderAsync();
                    List<CountryEntiry> Countries = new List<CountryEntiry>();

                    while (Reader.Read())
                    {
                        Countries.Add(new CountryEntiry((uint)Reader["CountryID"], (string)Reader["CountryName"]));
                    }
                    return Countries.Count > 0 ? Countries : null;
                }
            }
        }
        public static async Task<CountryEntiry?> GetCountryById(uint CountryId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString.Value))
            {
                using (SqlCommand command = new SqlCommand("select * from countries where countryId =@countryId "))
                {
                    command.Parameters.AddWithValue("@countryId", CountryId);
                    await connection.OpenAsync();
                    SqlDataReader Reader = await command.ExecuteReaderAsync();

                    if(Reader.Read())
                         return (new CountryEntiry((uint)Reader["CountryID"],(string)Reader["CountryName"]));
                    
                    return null;
                }
            }
        }
    }
}
