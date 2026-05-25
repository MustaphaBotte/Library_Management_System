using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class EmployeeRepository
    {
         public static async Task<int> AddNewEmployeeAsync(EmployeeDTO employeeDTO)
            {
                try
                {
                    await using SqlConnection connection = new SqlConnection(ConnectionString.Value);

                    await using SqlCommand command = new SqlCommand("SP_InsertNewEmployee", connection);

                    command.CommandType = CommandType.StoredProcedure;
                    var output = new SqlParameter("InsertedID", SqlDbType.Int);
                    output.Direction = ParameterDirection.Output;
                    SqlParameter[] parameters = new SqlParameter[] {

                    new SqlParameter("PersonID   ", employeeDTO.PersonId),
                    new SqlParameter("LibraryID", employeeDTO.LibraryId),
                    new SqlParameter("ManagerID", employeeDTO.ManagerId),
                    new SqlParameter("HireDate", employeeDTO.HireDate),
                    new SqlParameter("ContractEndDate", employeeDTO.ContractEndDate),
                    new SqlParameter("IsActive", employeeDTO.IsActive),
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

            public static async Task<bool> UpdateEmployeeAsync(EmployeeDTO employeeDTO)
            {
                try
                {
                    await using SqlConnection connection = new SqlConnection(ConnectionString.Value);
                    await using SqlCommand command = new SqlCommand("sp_UpdateEmployee", connection);

                    command.CommandType = CommandType.StoredProcedure;

                SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("EmployeeID", employeeDTO.EmployeeId),
                    new SqlParameter("HireDate", employeeDTO.HireDate),
                    new SqlParameter("HireDate", employeeDTO.HireDate),
                    new SqlParameter("ContractEndDate", employeeDTO.ContractEndDate),
                    };
                    command.Parameters.AddRange(parameters);

                    await connection.OpenAsync();
                    return (await command.ExecuteNonQueryAsync())>0;
                }
                catch (SqlException e)
                {
                    SqlExceptionHandler.Handle(e); // throws the specific exception
                }
                return false;
            }

            public static async Task<bool> DeleteEmployeeAsync(uint EmployeeId)
            {
                try
                {
                    using SqlConnection connection = new SqlConnection(ConnectionString.Value);

                    using SqlCommand command = new SqlCommand("SP_DeleteEmployee", connection);

                    command.CommandType = CommandType.StoredProcedure;

                    SqlParameter DeleteStatus = new SqlParameter("IsSuccess", SqlDbType.Bit);
                    DeleteStatus.Direction = ParameterDirection.Output;

                    command.Parameters.AddWithValue("@EmployeeID  ", (int)EmployeeId);
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

            public static async Task<EmployeeDTO?> GetEmployeeAsync(uint MemberID)
            {
                try
                {
                    using SqlConnection connection = new SqlConnection(ConnectionString.Value);

                    using SqlCommand command = new SqlCommand("SP_GetEmployeeByID", connection);

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("EmployeeID", (int)MemberID);
                    await connection.OpenAsync();
                    using SqlDataReader Reader = await command.ExecuteReaderAsync();

                    if (!await Reader.ReadAsync())
                        return null;

                    return new EmployeeDTO(
                        (uint)Reader["PersonID"],
                        (uint)Reader["EmployeeID"],
                        (uint)Reader["ManagerID"],
                        (uint)Reader["LibraryId"],
                        (DateTime)Reader["HireDate"],
                        (DateTime)Reader["ContractEndDate"],
                        (bool)Reader["IsActive"]
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

