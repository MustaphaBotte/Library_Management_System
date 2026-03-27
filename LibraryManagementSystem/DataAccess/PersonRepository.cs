using LMS.DTOs;
using Microsoft.Data.SqlClient;
using System.Data;
using PersonDto = LMS.DTOs.PersonDto;
using SqlExceptionHandler = LMS.DataAccess.Exceptions.SqlExceptionHandler;

namespace LMS.DataAccess
{
    public class PersonRepository
    {

        public static async Task<int> AddNewPersonAsync(PersonDto personDto)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.GetConnectionString()))
                {
                    using (SqlCommand command = new SqlCommand("SP_InsertPerson", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        var output = new SqlParameter("InsertedID", SqlDbType.Int);
                        output.Direction = ParameterDirection.Output;
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
                          output};

                        command.Parameters.AddRange(parameters);

                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                        return Utils.IsNullOrDBNull(output.Value)?-1:(int)output.Value;
                    }
                }
            }
            catch (SqlException e)
            {
                SqlExceptionHandler.Handle(e);
                throw;
            }
            
        }

        public static async Task<bool> UpdatePersonAsync(PersonDto personDto)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.GetConnectionString()))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdatePerson", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        var output = new SqlParameter("IsSuccess", SqlDbType.Bit);
                        output.Direction = ParameterDirection.Output;
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
                        output};
                        command.Parameters.AddRange(parameters);

                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                        return Utils.IsNullOrDBNull(output.Value) ? false : (bool)output.Value;
                    }
                }
            }
            catch (SqlException e)
            {
                SqlExceptionHandler.Handle(e); // throws the specific exception

                throw;
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
                        command.CommandType = CommandType.StoredProcedure;

                        SqlParameter DeleteStatus = new SqlParameter("IsSuccess", SqlDbType.Bit);
                        DeleteStatus.Direction = ParameterDirection.Output;

                        command.Parameters.AddWithValue("PersonID", (int)PersonID);
                        command.Parameters.Add(DeleteStatus);


                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();

                        return Utils.IsNullOrDBNull(DeleteStatus.Value)? false : (bool)DeleteStatus.Value;

                    }
                }
            }
            catch (SqlException e)
            {
                SqlExceptionHandler.Handle(e);
                throw;
            }          
        }

        public static async Task<PersonDto?> GetPersonAsync(uint PersonID)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.GetConnectionString()))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetPersonByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("PersonID", (int)PersonID);


                        await connection.OpenAsync();
                        using (SqlDataReader Reader = await command.ExecuteReaderAsync())
                        {
                            if (!await Reader.ReadAsync())
                                return null;

                            return new PersonDto(
                                      personID: (uint)Reader["PersonID"],
                                      firstName: (string)Reader["FirstName"],
                                      lastName: (string)Reader["SecondName"],
                                      email: Reader["Email"] as string,
                                      phonenumber: (string)Reader["PhoneNumber"],
                                      dateOfBirth: (DateTime)Reader["DateOfBirth"],
                                      gender: ((string)Reader["Gender"])[0],
                                      createdAt: (DateTime)Reader["CreatedAt"],
                                      updatedAt: (DateTime)Reader["UpdatedAt"],
                                      createdBy: Reader["CreatedBy"] as int?,
                                      countryId: (int)Reader["CountryID"],
                                      profilePicturePath: Reader["ProfilePicturePath"] as string,
                                      isDeleted: (bool)Reader["IsDeleted"]
                                      );

                        }
                    }
                }
            }
            catch (SqlException e)
            {
                SqlExceptionHandler.Handle(e);
                throw;
            }
            
        }

        public static async Task<DataTable?> GetPeopleAsync(int LastId, int Rows=10)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.GetConnectionString()))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetPeopleByLastId", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("LastId", LastId);
                        command.Parameters.AddWithValue("Rows", Rows);

                        await connection.OpenAsync();
                        using(SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                           
                            if (!reader.HasRows)
                                return null;

                            DataTable People = new DataTable();
                            People.Load(reader);
                            return People;
                        }

                    }
                }
            }
            catch (SqlException e)
            {
                SqlExceptionHandler.Handle(e);
                throw;
            }
          
        }

        public static async Task<bool> IsEmailExists(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                throw new ArgumentException("The Email Is Null Or Empty!.");
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.GetConnectionString()))
                {
                    using (SqlCommand command = new SqlCommand("select top 1 1 from people where Email = @email and IsDeleted=0", connection))
                    {

                        command.Parameters.AddWithValue("@email", Email);

                        await connection.OpenAsync();
                        object? Result = await command.ExecuteScalarAsync();
                        if (Result != null)

                        if(int.TryParse(Result.ToString(),out int res))
                              return res == 1;

                        return false;
                    }
                }
            }
            catch (SqlException e)
            {
                SqlExceptionHandler.Handle(e);
                throw;
            }
           
        }
        public static async Task<bool> IsPhoneNumberExists(string PhoneNumber)
        {
            if (string.IsNullOrEmpty(PhoneNumber))
            {
                throw new ArgumentException("The Phone Number Is Null Or Empty!.");
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.GetConnectionString()))
                {
                    using (SqlCommand command = new SqlCommand("select top 1 1 from people where PhoneNumber = @PhoneNumber  and IsDeleted=0", connection))
                    {

                        command.Parameters.AddWithValue("@PhoneNumber", @PhoneNumber);

                        await connection.OpenAsync();
                        object? Result = await command.ExecuteScalarAsync();
                        if (Result != null)

                            if (int.TryParse(Result.ToString(), out int res))
                                return res == 1;

                        return false;
                    }
                }
            }
            catch (SqlException e)
            {
                SqlExceptionHandler.Handle(e);
                throw;
            }
           
        }
        public static async Task<bool> IsPersonExists(uint PersonID)
        {
            
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.GetConnectionString()))
                {
                    using (SqlCommand command = new SqlCommand("select top 1 1 from people where PersonID = @PersonID  and IsDeleted=0", connection))
                    {

                        command.Parameters.AddWithValue("@PersonID", PersonID);


                        await connection.OpenAsync();
                        object? Result = await command.ExecuteScalarAsync();
                        if (Result != null)

                            if (int.TryParse(Result.ToString(), out int res))
                                return res == 1;

                        return false;
                    }
                }
            }
            catch (SqlException e)
            {
                SqlExceptionHandler.Handle(e);
                throw;
            }
           
        }


    }
}
