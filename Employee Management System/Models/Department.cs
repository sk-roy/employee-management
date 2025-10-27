using Employee_Management_System.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required]
        [StringLength(50)]
        public required string Name { get; set; }

        public decimal Budget { get; set; }
        public decimal Spent { get; set; }
    }
}
