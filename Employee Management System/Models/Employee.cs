using Employee_Management_System.Models.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee_Management_System.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        [Required]
        [StringLength(50)]
        public required string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        public DateTime HireDate { get; set; }
        [Required]
        public decimal Salary { get; set; }
        public bool IsManager { get; set; }
        public bool IsActive { get; set; }
        public int? HierarchyLevel { get; set; }

        public int? DepartmentId { get; set; }
        
        public int? ManagerId { get; set; }
        public string? FullManagerChain { get; set; }

    }
}
