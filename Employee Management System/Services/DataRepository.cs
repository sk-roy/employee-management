using Dapper;
using Employee_Management_System.Models;
using Employee_Management_System.Models.ViewModels;
using Employee_Management_System.Utilities.Exceptions;
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

        // --- Employee CRUD Implementations ---

        // AddEmployee
        public async Task<int> AddEmployeeAsync(Employee model)
        {
            try {
                await using var connection = new SqlConnection(_connectionString);

                var parameters = new DynamicParameters();
                parameters.Add("@FirstName", model.FirstName);
                parameters.Add("@LastName", model.LastName);
                parameters.Add("@Email", model.Email);
                parameters.Add("@Salary", model.Salary);
                parameters.Add("@HireDate", model.HireDate);
                parameters.Add("@DepartmentId", model.DepartmentId);
                parameters.Add("@IsManager", model.IsManager);

                if (model.ManagerId != null)
                {
                    var manager = await GetEmployeeByIdAsync((int)model.ManagerId);
                    parameters.Add("@ManagerId", model.ManagerId);
                    parameters.Add("@HiererKeyLevel", manager.HiererKeyLevel + 1);
                }

                var newId = await connection.ExecuteScalarAsync<int>(
                    "dbo.SP_AddEmployee",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return newId;
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("Employee salary exceeds"))
                {
                    throw new BudgetExceededException(ex.Message);
                }
                throw new Exception();
            }
        }

        // UpdateEmployee
        public async Task<bool> UpdateEmployeeAsync(EmployeeViewModel model)
        {
            try {
                await using var connection = new SqlConnection(_connectionString);

                var parameters = new DynamicParameters();
                parameters.Add("@EmployeeId", model.EmployeeId);
                parameters.Add("@FirstName", model.FirstName);
                parameters.Add("@LastName", model.LastName);
                parameters.Add("@Email", model.Email);
                parameters.Add("@Salary", model.Salary);
                parameters.Add("@HireDate", model.HireDate);
                parameters.Add("@IsActive", model.IsActive);
                parameters.Add("@DepartmentId", model.DepartmentId);
                parameters.Add("@ManagerId", model.ManagerId);
                parameters.Add("@IsManager", model.IsManager);
                parameters.Add("@HiererKeyLevel", model.HiererKeyLevel);
                parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                await connection.ExecuteAsync(
                    "dbo.SP_UpdateEmployee",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                int rowsAffected = parameters.Get<int>("@ReturnValue");
                return rowsAffected > 0;
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("Employee salary exceeds"))
                {
                    throw new BudgetExceededException(ex.Message);
                }
                throw new Exception();
            }
        }

        // DeactivateEmployee
        public async Task<bool> DeactivateEmployeeAsync(int id)
        {
            try {
                await using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@EmployeeId", id);

                parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                await connection.ExecuteAsync(
                    "dbo.SP_DeactivateEmployee",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                int rowsAffected = parameters.Get<int>("@ReturnValue");
                return rowsAffected > 0;
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("Employee salary exceeds"))
                {
                    throw new BudgetExceededException(ex.Message);
                }
                throw new Exception();
            }
        }

        // GetEmployeeById
        public async Task<EmployeeViewModel> GetEmployeeByIdAsync(int id)
        {
            await using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@EmployeeId", id);

            var employees = await connection.QueryAsync<EmployeeViewModel>(
                "dbo.SP_GetEmployeeById",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return employees.FirstOrDefault();
        }

        // GetEmployeesByDepartment
        public async Task<IEnumerable<EmployeeViewModel>> GetEmployeesByDepartmentAsync(int departmentId)
        {
            await using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@DepartmentId", departmentId);

            var employees = await connection.QueryAsync<EmployeeViewModel>(
                "dbo.SP_GetEmployeesByDepartment",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            // GroupBy is not strictly necessary here, but ensures unique employees if the SP were complex
            return employees.GroupBy(e => e.EmployeeId).Select(g => g.First());
        }

        // GetManagers (Used for Edit/Create dropdowns)
        public async Task<IEnumerable<Employee>> GetManagersAsync()
        {
            await using var connection = new SqlConnection(_connectionString);

            // Maps EmployeeId, FirstName, and LastName directly to ManagerInfo
            return await connection.QueryAsync<Employee>(
                "dbo.SP_GetManagers",
                commandType: CommandType.StoredProcedure
            );
        }


        public async Task<IEnumerable<EmployeeViewModel>> GetAllEmployeesAsync()
        {
            await using var connection = new SqlConnection(_connectionString);

            var employees = await connection.QueryAsync<EmployeeViewModel>(
                "dbo.SP_GetAllEmployees",
                commandType: CommandType.StoredProcedure
            );

            return employees;
        }


        // --- Department CRUD Implementations ---

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            await using var connection = new SqlConnection(_connectionString);

            var departments = await connection.QueryAsync<Department>(
                "dbo.SP_GetAllDepartments", // Stored Procedure Name
                commandType: CommandType.StoredProcedure
            );

            return departments;
        }

        public async Task<Department> GetDepartmentByIdAsync(int id)
        {
            await using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@DepartmentId", id);

            var department = await connection.QueryFirstOrDefaultAsync<Department>(
                "dbo.SP_GetDepartmentById",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            return department;
        }

        public async Task<int> AddDepartmentAsync(Department department)
        {
            await using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@Name", department.Name);
            parameters.Add("@Budget", department.Budget);

            var newId = await connection.ExecuteScalarAsync<int>(
                "dbo.SP_AddDepartment",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return newId;
        }

        public async Task<bool> UpdateDepartmentAsync(Department department)
        {
            await using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();

            parameters.Add("@DepartmentId", department.DepartmentId);
            parameters.Add("@Name", department.Name);
            parameters.Add("@Budget", department.Budget);
            parameters.Add("@Spent", department.Spent);

            parameters.Add(
                "@ReturnValue",
                dbType: DbType.Int32,
                direction: ParameterDirection.ReturnValue
            );

            await connection.ExecuteAsync(
                "dbo.SP_UpdateDepartment",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            int rowsAffected = parameters.Get<int>("@ReturnValue");

            return rowsAffected > 0;
        }
    }
}
