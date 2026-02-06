using LMS.DTOs;
using Microsoft.Data.SqlClient;
using MemberDto = LMS.DTOs.MemberDto;

namespace LMS.DataAccess
{
    public class PersonRepository
    {
        public static async Task<int> AddNewPerson(PersonDto personDto)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString.GetConnectionString()))
            {
                using(SqlCommand command = new SqlCommand("SP_InsertPerson",connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    var output = new SqlParameter("InsertedID",System.Data.SqlDbType.Int);
                    output.Direction = System.Data.ParameterDirection.Output;
                    SqlParameter[] parameters = new SqlParameter[] {

                    new SqlParameter("firstname", personDto.FirstName),
                    new SqlParameter("SecondName", personDto.LastName),
                    new SqlParameter("Email", personDto.Email),
                    new SqlParameter("PhoneNumber", personDto.PhoneNumber),
                    new SqlParameter("DateOfBirth", personDto.@DateOfBirth),
                    new SqlParameter("Gender", personDto.Gender),
                    new SqlParameter("CountryID", personDto.CountryID),
                    new SqlParameter("ProfilePicturePath", personDto.ProfilePicturePath==""?DBNull.Value: personDto.ProfilePicturePath),
                    new SqlParameter("CreatedBy", personDto.CreatedBy == 0 ? DBNull.Value : personDto.CreatedBy),
                    new SqlParameter("IsDeleted", false),
                    output
                    };
                    command.Parameters.AddRange(parameters);

                    try
                    {
                         await connection.OpenAsync();
                         await command.ExecuteNonQueryAsync();
                         return (int)output.Value;
                    }
                    catch (Exception e)
                    {
                        return -1;
                    }

                }
               
            }

        }
    }
}
