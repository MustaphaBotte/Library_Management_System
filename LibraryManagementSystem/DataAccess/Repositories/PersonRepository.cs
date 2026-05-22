using LMS.DataAccess.DTOs;

namespace LMS.DataAccess.Repositories
{
    public class PersonRepository
    {
        
        public static async Task<int> AddNewPersonAsync(PersonDTO PersonDTO)
        {
            try
            {
                await using SqlConnection connection = new SqlConnection(ConnectionString.Value);

                await using SqlCommand command = new SqlCommand("SP_InsertPerson", connection);

                command.CommandType = CommandType.StoredProcedure;
                var output = new SqlParameter("InsertedID", SqlDbType.Int) { Direction = ParameterDirection.Output };

                SqlParameter[] parameters = new SqlParameter[] {

                new SqlParameter("firstname", PersonDTO.FirstName),
                new SqlParameter("SecondName", PersonDTO.SecondName),
                new SqlParameter("Email", PersonDTO.Email),
                new SqlParameter("PhoneNumber", PersonDTO.PhoneNumber),
                 new SqlParameter("PasswordHash", PersonDTO.PasswordHash),
                new SqlParameter("DateOfBirth", PersonDTO.@DateOfBirth),
                new SqlParameter("Gender", PersonDTO.Gender),
                new SqlParameter("CountryID", PersonDTO.CountryID),
                new SqlParameter("ProfilePicturePath",string.IsNullOrEmpty(PersonDTO.ProfilePicturePath)? DBNull.Value : PersonDTO.ProfilePicturePath),
                new SqlParameter("CreatedBy", PersonDTO.CreatedBy == -1 ? DBNull.Value : PersonDTO.CreatedBy),
                output
                };

                command.Parameters.AddRange(parameters);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                return DbUtils.IsNullOrDBNull(output.Value) ? -1 : (int)output.Value;


            }
            catch (SqlException e)
            {
                SqlExceptionHandler.Handle(e);
              
            }
            return -1;
        }

        public static async Task<bool> UpdatePersonAsync(PersonDTO PersonDTO)
        {
            try
            {
                await using SqlConnection connection = new SqlConnection(ConnectionString.Value);

                await using SqlCommand command = new SqlCommand("SP_UpdatePerson", connection);
                    
                command.CommandType = CommandType.StoredProcedure;
                var output = new SqlParameter("IsSuccess", SqlDbType.Bit);
                output.Direction = ParameterDirection.Output;
                SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("PersonID", (int)PersonDTO.PersonId),
                new SqlParameter("firstname", PersonDTO.FirstName),
                new SqlParameter("SecondName", PersonDTO.SecondName),
                new SqlParameter("Email", PersonDTO.Email),
                new SqlParameter("@PasswordHash", PersonDTO.PasswordHash),
                new SqlParameter("PhoneNumber", PersonDTO.PhoneNumber),
                new SqlParameter("DateOfBirth", PersonDTO.@DateOfBirth),
                new SqlParameter("Gender", PersonDTO.Gender),
                new SqlParameter("CountryID", PersonDTO.CountryID),
                output};
                command.Parameters.AddRange(parameters);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                return DbUtils.IsNullOrDBNull(output.Value) ? false : (bool)output.Value;
 
            }
            catch (SqlException e)
            {
                SqlExceptionHandler.Handle(e); // throws the specific exception
            }  
            return false;
        }

        public static async Task<bool> DeletePersonAsync(uint PersonID)
        {
            try
            {
                  using SqlConnection connection = new SqlConnection(ConnectionString.Value);
                 
                  using SqlCommand command = new SqlCommand("SP_DeletePerson", connection);
                    
                  command.CommandType = CommandType.StoredProcedure;

                  SqlParameter DeleteStatus = new SqlParameter("IsSuccess", SqlDbType.Bit);
                  DeleteStatus.Direction = ParameterDirection.Output;

                  command.Parameters.AddWithValue("PersonID", (int)PersonID);
                  command.Parameters.Add(DeleteStatus);


                  await connection.OpenAsync();
                  await command.ExecuteNonQueryAsync();

                  return DbUtils.IsNullOrDBNull(DeleteStatus.Value)? false : (bool)DeleteStatus.Value;
            }
            catch (SqlException e)
            {
                SqlExceptionHandler.Handle(e);
            }
            return false;
        }

        public static async Task<PersonDTO?> GetPersonAsync(uint PersonID)
        {

            try
            {
                await using SqlConnection connection = new SqlConnection(ConnectionString.Value);

                await using SqlCommand command = new SqlCommand("SP_GetPersonByID", connection);

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("PersonID", (int)PersonID);


                await connection.OpenAsync();
                using SqlDataReader Reader = await command.ExecuteReaderAsync();

                if (!await Reader.ReadAsync())
                    return null;

                return new PersonDTO(
                              personID: (uint)Reader["PersonID"],
                              firstName: (string)Reader["FirstName"],
                              secondName: (string)Reader["SecondName"],
                              email: Reader["Email"] as string,
                              phonenumber: (string)Reader["PhoneNumber"],
                              PasswordHash: (string)Reader["PasswordHash"],
                              dateOfBirth: (DateTime)Reader["DateOfBirth"],
                              gender: ((string)Reader["Gender"])[0],
                              createdAt: (DateTime)Reader["CreatedAt"],
                              updatedAt: (DateTime)Reader["UpdatedAt"],
                              createdBy: Reader["CreatedBy"] as int?,
                              countryId: (uint)Reader["CountryID"],
                              profilePicturePath: Reader["ProfilePicturePath"] as string,
                              isDeleted: (bool)Reader["IsDeleted"]
                              );
            }

            catch (SqlException e)
            {
                SqlExceptionHandler.Handle(e);
            }
            return null;
        }

        public static async Task<List<PersonDTO>?> GetPeopleAsync(int LastId, int Rows=10)
        {
            try
            {
                await using SqlConnection connection = new SqlConnection(ConnectionString.Value);

                await using SqlCommand command = new SqlCommand("SP_GetPeopleByLastId", connection);
                    
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("LastId", LastId);
                command.Parameters.AddWithValue("Rows", Rows);

                await connection.OpenAsync();
                await using SqlDataReader Reader = await command.ExecuteReaderAsync();
                
                if (!Reader.HasRows)
                        return null;

                var PeopleList = new List<PersonDTO>();

                while (Reader.Read())
                {
                    PeopleList.Add(new PersonDTO(
                             personID: (uint)Reader["PersonID"],
                             firstName: (string)Reader["FirstName"],
                             secondName: (string)Reader["SecondName"],
                             email: Reader["Email"] as string,
                             phonenumber: (string)Reader["PhoneNumber"],
                             PasswordHash: (string)Reader["PasswordHash"],
                             dateOfBirth: (DateTime)Reader["DateOfBirth"],
                             gender: ((string)Reader["Gender"])[0],
                             createdAt: (DateTime)Reader["CreatedAt"],
                             updatedAt: (DateTime)Reader["UpdatedAt"],
                             createdBy: Reader["CreatedBy"] as int?,
                             countryId: (uint)Reader["CountryID"],
                             profilePicturePath: Reader["ProfilePicturePath"] as string,
                             isDeleted: (bool)Reader["IsDeleted"]
                             ));
                }
                return PeopleList;
            }
            catch (SqlException e)
            {
                SqlExceptionHandler.Handle(e);
            }
            return null;       
        }


        private static async Task<bool> ExistsAsync(string Identifier , object Value )
        {
            if (string.IsNullOrEmpty(Identifier))
            {
                throw new ArgumentException("The Identifier Is Null Or Empty!.");
            }
            try
            {
                await using SqlConnection connection = new SqlConnection(ConnectionString.Value);

                await using SqlCommand command = new SqlCommand($"select top 1 1 from people where {Identifier} = @value and IsDeleted=0", connection);

                command.Parameters.AddWithValue(Identifier, Value);

                await connection.OpenAsync();
                object? Result = await command.ExecuteScalarAsync();
                if (Result != null)

                    if (int.TryParse(Result.ToString(), out int res))
                        return res == 1;

                return false;

            }
            catch (SqlException e)
            {
                SqlExceptionHandler.Handle(e);
            }
            return false;
        }
        public static async Task<bool> IsEmailExists(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                throw new ArgumentException("The Email Is Null Or Empty!.");
            }
            return await ExistsAsync("Email", Email);
        }
        public static async Task<bool> IsPhoneNumberExists(string PhoneNumber)
        {
            if (string.IsNullOrEmpty(PhoneNumber))
            {
                throw new ArgumentException("The Phone Number Is Null Or Empty!.");
            }

            return await ExistsAsync("PhoneNumber", PhoneNumber);

        }
        public static async Task<bool> IsPersonExists(uint PersonID)=>await ExistsAsync("PersonID", PersonID); 
        


    }
}
