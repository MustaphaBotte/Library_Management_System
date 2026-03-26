using LMS.DTOs;
using Microsoft.Data.SqlClient;
using System.Data;
using MemberDto = LMS.DTOs.MemberDto;

namespace LMS.DataAccess
{
    public class PersonRepository
    {
        public static async Task<int> AddNewPersonAsync(PersonDto personDto)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString.GetConnectionString()))
            {
                using(SqlCommand command = new SqlCommand("SP_InsertPerson",connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
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
                        // i will log it later
                        return -1;
                    }

                }
               
            }

        }

        public static async Task<bool> UpdatePersonAsync(PersonDto personDto)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString.GetConnectionString()))
            {
                using (SqlCommand command = new SqlCommand("SP_UpdatePerson", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    var output = new SqlParameter("IsSuccess", System.Data.SqlDbType.Bit);
                    output.Direction = System.Data.ParameterDirection.Output;
                    SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("PersonID", (int)personDto.PersonId),
                    new SqlParameter("firstname", personDto.FirstName),
                    new SqlParameter("SecondName", personDto.LastName),
                    new SqlParameter("Email", personDto.Email),
                    new SqlParameter("PhoneNumber", personDto.PhoneNumber),
                    new SqlParameter("DateOfBirth", personDto.@DateOfBirth),
                    new SqlParameter("Gender", personDto.Gender),
                    new SqlParameter("CountryID", personDto.CountryID),
                    new SqlParameter("ProfilePicturePath", personDto.ProfilePicturePath==""?DBNull.Value: personDto.ProfilePicturePath),
                    output
                    };
                    command.Parameters.AddRange(parameters);

                    try
                    {
                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                        return (bool)output.Value;
                    }
                    catch (Exception e)
                    {
                        throw;
                        // i will log it later
                       // return false;
                    }

                }

            }



        }


        public static async Task<bool> DeletePersonAsync(uint PersonID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.GetConnectionString()))
                {
                    using (SqlCommand command = new SqlCommand("SP_DeletePerson", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter DeleteStatus = new SqlParameter("IsSuccess ", SqlDbType.Bit);
                        DeleteStatus.Direction = ParameterDirection.Output;

                        command.Parameters.AddWithValue("PersonID", (int)PersonID);
                        command.Parameters.Add(DeleteStatus);


                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();

                        return (bool)DeleteStatus.Value;

                    }
                }
            }
            catch(Exception e)
            {
                return false;
            }

        }



    }
}
