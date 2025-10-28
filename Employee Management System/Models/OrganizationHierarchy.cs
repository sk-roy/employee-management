using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Models
{
    public class OrganizationHierarchy
    {
        [Key]
        public required string EmployeeId { get; set; }
        public required string ManagerId { get; set; }
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Display(Name = "Level")]
        public int HierarchyLevel { get; set; }

        [Display(Name = "Manager Chain")]
        public string FullManagerChain { get; set; }
    }
}
