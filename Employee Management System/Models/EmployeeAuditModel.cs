using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Models
{
    public class EmployeeAuditModel
    {
        public long AuditId { get; set; }
        public int EmployeeId { get; set; }
        public string ChangeType { get; set; } = string.Empty;

        [Display(Name = "Field Changed")]
        public string FieldName { get; set; } = string.Empty;

        [Display(Name = "Old Value")]
        public string? OldValue { get; set; }

        [Display(Name = "New Value")]
        public string? NewValue { get; set; }

        [Display(Name = "Date/Time")]
        public DateTime ChangeDate { get; set; }

        [Display(Name = "Changed By")]
        public string ChangedBy { get; set; } = string.Empty;
    }
}
