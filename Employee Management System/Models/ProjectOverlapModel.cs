using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Models
{
    public class ProjectOverlapModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;

        public string ProjectName1 { get; set; } = string.Empty;
        public string ProjectName2 { get; set; } = string.Empty;
        public int ProjectId1 { get; set; }
        public int ProjectId2 { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDateProject1 { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDateProject1 { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDateProject2 { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDateProject2 { get; set; }
    }
}