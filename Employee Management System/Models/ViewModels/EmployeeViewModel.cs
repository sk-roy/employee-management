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
        public string FullName => $"{FirstName} {LastName}";

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

        [Display(Name = "Hiererkey Level")]
        public int HiererKeyLevel { get; set; }

        [ForeignKey("DepartmentId")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Display(Name = "Manager")]
        public int? ManagerId { get; set; }

        public string? DepartmentName { get; set; }

        public string? ManagerFirstName { get; set; }
        public string? ManagerLastName { get; set; }
        public string ManagerName => $"{ManagerFirstName} {ManagerLastName}";

        public IEnumerable<SelectListItem> DepartmentList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> ManagerList { get; set; } = new List<SelectListItem>();

    }
}
