using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Models.ViewModels
{
    public class DepartmentViewModel
    {
        [Key]
        public int DepartmentId { get; set; }

        public required string DepartmentName { get; set; }

        public decimal Budget { get; set; }
        public decimal Spent { get; set; }

        public IEnumerable<EmployeeViewModel> Employees { get; set; } = new List<EmployeeViewModel>();
    }
}
