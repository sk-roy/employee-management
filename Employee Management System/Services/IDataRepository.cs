using Employee_Management_System.Models;

namespace Employee_Management_System.Services
{
    public interface IDataRepository
    {        Task<IEnumerable<EmployeeViewModel>> GetAllEmployeesAsync();
    }
}
