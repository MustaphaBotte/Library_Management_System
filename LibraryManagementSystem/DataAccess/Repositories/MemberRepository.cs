namespace LMS.DataAccess.Repositories
{
    public class MemberRepository
    {
        public static async Task<int> AddNewMemberAsync(MemberEntity memberEntity)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.Value))
                {
                    using (SqlCommand command = new SqlCommand("SP_InsertMember", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        var output = new SqlParameter("InsertedID", SqlDbType.Int);
                        output.Direction = ParameterDirection.Output;
                        SqlParameter[] parameters = new SqlParameter[] {

                          new SqlParameter("PersonID   ", memberEntity.PersonId),
                          new SqlParameter("@Username", memberEntity.Username),
                          new SqlParameter("@PasswordHash", memberEntity.PasswordHash),
                          new SqlParameter("@PasswordSalt", memberEntity.PasswordSalt),
                          new SqlParameter("LibraryID", memberEntity.LibraryId),
                          new SqlParameter("Notes",DbUtils.IsNullOrDBNull(memberEntity.Notes)?DBNull.Value:memberEntity.Notes),
                          new SqlParameter("ExpiredAt", memberEntity.ExpiredAt),                         
                          output};

                        command.Parameters.AddRange(parameters);

                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                        return DbUtils.IsNullOrDBNull(output.Value) ? -1 : (int)output.Value;
                    }
                }
            }
            catch (SqlException e)
            {
                SqlExceptionHandler.Handle(e);
                throw;
            }

        }

        public static async Task<bool> UpdateMemberAsync(MemberEntity memberEntity)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.Value))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdateMember", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        var output = new SqlParameter("IsSuccess", SqlDbType.Bit);
                        output.Direction = ParameterDirection.Output;
                        SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("MemberID", (int)memberEntity.MemberId),
                        new SqlParameter("Username", memberEntity.Username),
                        new SqlParameter("PasswordHash", memberEntity.PasswordHash),
                        new SqlParameter("PasswordSalt", memberEntity.PasswordSalt),
                        new SqlParameter("LibraryID", memberEntity.LibraryId),
                        new SqlParameter("Notes", memberEntity.Notes),
                        new SqlParameter("ExpiredAt", memberEntity.ExpiredAt),
                        output};
                        command.Parameters.AddRange(parameters);

                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                        return DbUtils.IsNullOrDBNull(output.Value) ? false : (bool)output.Value;
                    }
                }
            }
            catch (SqlException e)
            {
                SqlExceptionHandler.Handle(e); // throws the specific exception

                throw;
            }
        }

        public static async Task<bool> DeleteMemberAsync(uint MemberID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.Value))
                {
                    using (SqlCommand command = new SqlCommand("SP_DeleteMember", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        SqlParameter DeleteStatus = new SqlParameter("IsSuccess", SqlDbType.Bit);
                        DeleteStatus.Direction = ParameterDirection.Output;

                        command.Parameters.AddWithValue("MemberID", (int)MemberID);
                        command.Parameters.Add(DeleteStatus);


                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();

                        return DbUtils.IsNullOrDBNull(DeleteStatus.Value) ? false : (bool)DeleteStatus.Value;

                    }
                }
            }
            catch (SqlException e)
            {
                SqlExceptionHandler.Handle(e);
                throw;
            }
        }

        public static async Task<MemberEntity?> GetMemberAsync(uint MemberID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.Value))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetMemberByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("MemberID", (int)MemberID);
                        await connection.OpenAsync();
                        using (SqlDataReader Reader = await command.ExecuteReaderAsync())
                        {
                            if (!await Reader.ReadAsync())
                                return null;
                            return new MemberEntity(
                                (uint)Reader["PersonID"],
                                (uint)Reader["MemberID"],
                                (string)Reader["Username"],
                                (string)Reader["PasswordHash"],
                                (string)Reader["PasswordSalt"],
                                (DateTime)Reader["JoinedAt"],
                                (DateTime)Reader["ExpiredAt"],
                                (bool)Reader["IsBanned"],
                                (int)Reader["MembershipStatusID"],
                                Reader["LastBorrowAt"] as DateTime?,
                                (uint)Reader["LibraryID"],
                                Reader["Notes"] as string
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








    }
}
