using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Models
{
    public class ProjectOverlapModel
    {
        public string Employee1Name { get; set; } = string.Empty;
        public string Employee2Name { get; set; } = string.Empty;

        public string Employee1Project { get; set; } = string.Empty;
        public string Employee2Project { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime ConflictStart1 { get; set; }
        [DataType(DataType.Date)]
        public DateTime ConflictEnd1 { get; set; }
        [DataType(DataType.Date)]
        public DateTime ConflictStart2 { get; set; }
        [DataType(DataType.Date)]
        public DateTime ConflictEnd2 { get; set; }
    }
}
