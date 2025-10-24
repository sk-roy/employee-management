using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee_Management_System.Models
{
    public class EmployeeViewModel
    {
        [Key]
        public int EmployeeId { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public DateTime HireDate { get; set; }
        [Required]
        public decimal Salary { get; set; }

        public int? DepartmentId { get; set; }
        public int? ManagerId { get; set; }

        [ForeignKey("DepartmentId")]
        public DepartmentViewModel Department { get; set; }

        public ManagerInfo Manager { get; set; }

    }
}
