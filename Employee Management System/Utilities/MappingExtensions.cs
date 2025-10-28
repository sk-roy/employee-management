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
            return new Employee
            {
                EmployeeId = viewModel.EmployeeId,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                Email = viewModel.Email,
                HireDate = viewModel.HireDate,
                Salary = viewModel.Salary,
                IsActive = viewModel.IsActive,
                IsManager = viewModel.IsManager,
                HierarchyLevel = viewModel.HierarchyLevel,
                DepartmentId = viewModel.DepartmentId,
                ManagerId = viewModel.ManagerId
            };
        }

        public static DepartmentViewModel ToDepartmentViewModel(this Department department)
        {
            return new DepartmentViewModel
            {
                DepartmentId = department.DepartmentId,
                DepartmentName = department.Name,
                Budget = department.Budget,
                Spent = department.Spent
            };
        }
    }
}
