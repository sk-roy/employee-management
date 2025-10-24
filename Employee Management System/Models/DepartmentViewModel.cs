using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Models
{
    public class DepartmentViewModel
    {
        [Key]
        public int DepartmentId { get; set; }

        public required string DepartmentName { get; set; }

        public decimal Budget { get; set; }

        // Navigation property for related Employees
        public ICollection<EmployeeViewModel> Employees { get; set; }
    }
}
