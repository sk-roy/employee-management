using Employee_Management_System.Models;
using Employee_Management_System.Models.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee_Management_System.Utilities
{
    public static class MappingExtensions
    {
        public static Employee ToEmployeeModel(this EmployeeViewModel viewModel)
        {
            // Note: We intentionally skip mapping navigation properties (Department, Manager)
            // as the Model is typically used for database operations via Dapper, 
            // and the FKs (DepartmentId, ManagerId) are sufficient.

            return new Employee
            {
                EmployeeId = viewModel.EmployeeId,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                Email = viewModel.Email,
                HireDate = viewModel.HireDate,
                Salary = viewModel.Salary,
                IsActive = viewModel.IsActive,
                HiererKeyLevel = viewModel.HiererKeyLevel,
                DepartmentId = viewModel.DepartmentId,
                ManagerId = viewModel.ManagerId
            };
        }
    }
}
