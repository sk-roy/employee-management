namespace Employee_Management_System.Models.ViewModels
{
    public class ProjectDetailsViewModel
    {
        public Project Project { get; set; } = new Project();
        public IEnumerable<Employee> AssignedEmployees { get; set; } = new List<Employee>();
    }
}
