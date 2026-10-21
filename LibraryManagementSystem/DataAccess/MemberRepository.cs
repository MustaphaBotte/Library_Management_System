using LMS.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace LMS.DataAccess
{
    public class MemberRepository : BaseRepository
    {
        public MemberRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<int> AddAsync(Person person)
        {
         
                using var connection = CreateConnection();

                using var command = new SqlCommand("sp_RegisterMember", connection);

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@FirstName", person.FirstName);
                command.Parameters.AddWithValue("@LastName", person.LastName);
                command.Parameters.AddWithValue("@Email", person.Email);
                command.Parameters.AddWithValue("@PasswordHash", person.PasswordHash);

                var newMemberIdParam = new SqlParameter("@NewMemberID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(newMemberIdParam);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();

                return (int)newMemberIdParam.Value;
        }
        public async Task<Member?> GetByEmail(string email)
        {
            using var connection = CreateConnection();

            string query = @"SELECT
                                  M.MemberID,M.MembershipStatus,                             
                                  P.PersonID,
                                  P.FirstName,
                                  P.LastName,
                                  P.Email,
                                  P.ProfilePicturePath,
                                  P.PasswordHash,
                                  P.CreatedAt,
                                  P.DeletedAt
                                  FROM PEOPLE P
                                      INNER JOIN MEMBERS M
                                          ON P.PersonID = M.PersonID                                
                                  WHERE P.Email = @Email";
            using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Email", email);
            await connection.OpenAsync();

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return new Member
            {
                MemberID = (int)reader["MemberID"],
                MembershipStatus = (string)reader["MembershipStatus"],
                PersonID = (int)reader["PersonID"],
                Person = new Person
                {
                    PersonID = (int)reader["PersonID"],
                    FirstName = (string)reader["FirstName"],
                    LastName = (string)reader["LastName"],
                    Email = (string)reader["Email"],
                    ProfilePicturePath = (string)reader["ProfilePicturePath"],
                    PasswordHash = (string)reader["PasswordHash"],
                    CreatedAt = (DateTime)reader["CreatedAt"],
                    DeletedAt = reader["DeletedAt"]==DBNull.Value ? null
    :                                                (DateTime)reader["DeletedAt"]
                }
            };
        }
    }
}