namespace Employee_Management_System.Models
{
    public class ManagerInfo
    {
        public int ManagerId { get; set; }
        public string ManagerFirstName { get; set; }
        public string ManagerLastName { get; set; }

        public string FullName => $"{ManagerFirstName} {ManagerLastName}";
    }
}
