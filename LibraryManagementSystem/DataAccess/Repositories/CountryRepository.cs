using LMS.DataAccess.DTOs;

namespace LMS.DataAccess.Repositories
{
    public class CountryRepository
    {
        public static async Task<List<CountryDTO>?> GetAllCountries(uint CountryId)
        {
            await using(SqlConnection connection = new SqlConnection(ConnectionString.Value))
            {
                await using(SqlCommand command = new SqlCommand("select * from countries"))
                {
                    await connection.OpenAsync();
                    await using SqlDataReader Reader =await command.ExecuteReaderAsync();
                    List<CountryDTO> Countries = new List<CountryDTO>();
                    if (!Reader.HasRows)
                        return null;

                    while (await Reader.ReadAsync())
                    {
                        Countries.Add(new CountryDTO((uint)Reader["CountryID"], (string)Reader["CountryName"]));
                    }
                    return Countries.Count > 0 ? Countries : null;
                }
            }
        }
        public static async Task<CountryDTO?> GetCountryById(uint CountryId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString.Value))
            {
                using (SqlCommand command = new SqlCommand("select * from countries where countryId =@countryId "))
                {
                    command.Parameters.AddWithValue("@countryId", CountryId);
                    await connection.OpenAsync();
                    await using SqlDataReader Reader = await command.ExecuteReaderAsync();

                    if(await Reader.ReadAsync())
                         return (new CountryDTO((uint)Reader["CountryID"],(string)Reader["CountryName"]));
                    
                    return null;
                }
            }
        }
    }
}
