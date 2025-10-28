using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee_Management_System.Models.ViewModels
{
    public class EmployeeViewModel
    {
        [Key]
        public int EmployeeId { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string? FullName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(FirstName) && string.IsNullOrWhiteSpace(LastName))
                {
                    return null;
                }
                return string.Join(" ", new[] { FirstName, LastName }.Where(n => !string.IsNullOrWhiteSpace(n)));
            }
        }

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

        [Display(Name = "Is a Manager!")]
        public bool IsManager { get; set; }
        public bool IsActive { get; set; } = true;

        [Display(Name = "Hierarchy Level")]
        public int HierarchyLevel { get; set; } = 0;

        [ForeignKey("DepartmentId")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Display(Name = "Manager")]
        public int? ManagerId { get; set; }

        public string? DepartmentName { get; set; }

        public string? ManagerFirstName { get; set; }
        public string? ManagerLastName { get; set; }
        public string? ManagerName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ManagerFirstName) && string.IsNullOrWhiteSpace(ManagerLastName))
                {
                    return null;
                }
                return string.Join(" ", new[] { ManagerFirstName, ManagerLastName }.Where(n => !string.IsNullOrWhiteSpace(n)));
            }
        }

        public IEnumerable<SelectListItem> DepartmentList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> ManagerList { get; set; } = new List<SelectListItem>();
        public IEnumerable<Employee> DirectReports { get; set; } = new List<Employee>();
        public IEnumerable<Employee> AllReports { get; set; } = new List<Employee>();
        public IEnumerable<Project> Projects { get; set; } = new List<Project>();

    }
}
