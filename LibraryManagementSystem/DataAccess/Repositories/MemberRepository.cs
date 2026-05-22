using LMS.DataAccess.DTOs;

namespace LMS.DataAccess.Repositories
{
    public class MemberRepository
    {
        public static async Task<int> AddNewMemberAsync(MemberDTO MemberDTO)
        {
            try
            {
                await using SqlConnection connection = new SqlConnection(ConnectionString.Value);

                await using SqlCommand command = new SqlCommand("SP_InsertMember", connection);
                    
                command.CommandType = CommandType.StoredProcedure;
                var output = new SqlParameter("InsertedID", SqlDbType.Int);
                output.Direction = ParameterDirection.Output;
                SqlParameter[] parameters = new SqlParameter[] {

                  new SqlParameter("PersonID   ", MemberDTO.PersonId),
                  new SqlParameter("LibraryID", MemberDTO.LibraryId),
                  new SqlParameter("Notes",DbUtils.IsNullOrDBNull(MemberDTO.Notes)?DBNull.Value:MemberDTO.Notes),
                  new SqlParameter("ExpiredAt", MemberDTO.ExpiredAt),                         
                  output};

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

        public static async Task<bool> UpdateMemberAsync(MemberDTO MemberDTO)
        {
            try
            {
                await using SqlConnection connection = new SqlConnection(ConnectionString.Value);
                await using SqlCommand command = new SqlCommand("SP_UpdateMember", connection);
                    
                command.CommandType = CommandType.StoredProcedure;
                var output = new SqlParameter("IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output
                };

                SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("MemberID", (int)MemberDTO.MemberId),          
                new SqlParameter("Notes", MemberDTO.Notes),
                new SqlParameter("ExpiredAt", MemberDTO.ExpiredAt),
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

        public static async Task<bool> DeleteMemberAsync(uint MemberID)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(ConnectionString.Value);

                using SqlCommand command = new SqlCommand("SP_DeleteMember", connection);
                    
                 command.CommandType = CommandType.StoredProcedure;

                 SqlParameter DeleteStatus = new SqlParameter("IsSuccess", SqlDbType.Bit);
                 DeleteStatus.Direction = ParameterDirection.Output;

                 command.Parameters.AddWithValue("MemberID", (int)MemberID);
                 command.Parameters.Add(DeleteStatus);


                 await connection.OpenAsync();
                 await command.ExecuteNonQueryAsync();

                 return DbUtils.IsNullOrDBNull(DeleteStatus.Value) ? false : (bool)DeleteStatus.Value;
            }
            catch (SqlException e)
            {
                SqlExceptionHandler.Handle(e);
            }
            return false; 
        }

        public static async Task<MemberDTO?> GetMemberAsync(uint MemberID)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(ConnectionString.Value);

                using SqlCommand command = new SqlCommand("SP_GetMemberByID", connection);
                    
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("MemberID", (int)MemberID);
                await connection.OpenAsync();
                using SqlDataReader Reader = await command.ExecuteReaderAsync();
                
                    if (!await Reader.ReadAsync())
                        return null;

                return new MemberDTO(
                    (uint)Reader["PersonID"],
                    (uint)Reader["MemberID"],                  
                    (DateTime)Reader["ExpiredAt"],
                    (bool)Reader["IsBanned"],
                    (int)Reader["MembershipStatusID"],
                    (uint)Reader["LibraryID"],
                    Reader["Notes"] == DBNull.Value ? "": (string)Reader["Notes"]
                );
            }
            catch (SqlException e)
            {
                SqlExceptionHandler.Handle(e);
            }
            return null;
        }


    }
}
