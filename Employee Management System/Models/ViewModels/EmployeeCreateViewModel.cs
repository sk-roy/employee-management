using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Models.ViewModels
{
    public class EmployeeCreateViewModel
    {
        [Key]
        public int EmployeeId { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; } = DateTime.Today;

        [Required]
        [DataType(DataType.Currency)]
        [Range(1, 500000, ErrorMessage = "Salary must be between $1 and $500,000")]
        public decimal Salary { get; set; }

        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Display(Name = "Manager")]
        public int? ManagerId { get; set; }

        public DepartmentViewModel Department { get; set; }

        public ManagerInfo Manager { get; set; }

        public IEnumerable<SelectListItem> DepartmentList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> ManagerList { get; set; } = new List<SelectListItem>();

        public int HierarchyLevel { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
