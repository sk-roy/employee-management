using Employee_Management_System.Models;
using Employee_Management_System.Models.ViewModels;

namespace Employee_Management_System.Services
{
    public interface IDataRepository
    {
        Task<IEnumerable<EmployeeViewModel>> GetAllEmployeesAsync();
        Task<int> AddEmployeeAsync(Employee model);
        Task<EmployeeViewModel> GetEmployeeByIdAsync(int id);
        Task<bool> UpdateEmployeeAsync(EmployeeViewModel model);
        Task<bool> DeactivateEmployeeAsync(int id);
        Task<IEnumerable<EmployeeViewModel>> GetEmployeesByDepartmentAsync(int departmentId);
        Task<IEnumerable<Employee>> GetManagersAsync();

        Task<IEnumerable<Department>> GetAllDepartmentsAsync();
        Task<Department> GetDepartmentByIdAsync(int id);
        Task<int> AddDepartmentAsync(Department department);
        Task<bool> UpdateDepartmentAsync(Department department);
    }
}
