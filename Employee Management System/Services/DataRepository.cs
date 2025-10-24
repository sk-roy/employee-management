using Dapper;
using Employee_Management_System.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Employee_Management_System.Services
{
    public class DataRepository: IDataRepository
    {
        private readonly string _connectionString;

        public DataRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }


        public async Task<IEnumerable<EmployeeViewModel>> GetAllEmployeesAsync()
        {
            await using var connection = new SqlConnection(_connectionString);

            // Dapper's multi-mapping now handles Employee, Department, and ManagerInfo
            var employees = await connection.QueryAsync<EmployeeViewModel, DepartmentViewModel, ManagerInfo, EmployeeViewModel>(
                "dbo.GetAllEmployees",
                (employee, department, manager) =>
                {
                    employee.Department = department;

                    if (manager != null)
                    {
                        employee.Manager = new ManagerInfo
                        {
                            ManagerId = manager.ManagerId,
                            ManagerFirstName = manager.ManagerFirstName,
                            ManagerLastName = manager.ManagerLastName
                        };
                    }

                    return employee;
                },
                // Split occurs where the DepartmentId column and then ManagerId column appears in the result set
                splitOn: "DepartmentId, ManagerId",
                commandType: CommandType.StoredProcedure
            );

            return employees;
        }

        // Example for adding an employee using a stored procedure
        public async Task AddEmployeeAsync(EmployeeViewModel employee)
        {
            // Assuming you have a stored procedure like dbo.InsertEmployee
            var parameters = new DynamicParameters();
            parameters.Add("@FirstName", employee.FirstName);
            parameters.Add("@LastName", employee.LastName);
            parameters.Add("@Email", employee.Email);
            parameters.Add("@HireDate", employee.HireDate);
            parameters.Add("@Salary", employee.Salary);
            parameters.Add("@DepartmentId", employee.DepartmentId);

            await using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "dbo.InsertEmployee",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
