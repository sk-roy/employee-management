using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public decimal Budget { get; set; }

        // Navigation property for related Employees
        public ICollection<EmployeeViewModel> Employees { get; set; }
    }
}
