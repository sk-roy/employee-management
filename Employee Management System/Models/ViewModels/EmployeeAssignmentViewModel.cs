using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Models.ViewModels
{
    public class EmployeeAssignmentViewModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select at least one employee.")]
        [Display(Name = "Employees to Assign")]
        public List<int> SelectedEmployeeIds { get; set; } = new List<int>();

        public IEnumerable<SelectListItem> AvailableEmployees { get; set; } = new List<SelectListItem>();
    }
}
