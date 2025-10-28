using Employee_Management_System.Models;
using Employee_Management_System.Models.ViewModels;
using Employee_Management_System.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Employee_Management_System.Controllers
{
    public class ProjectController: Controller
    {
        private readonly IDataRepository _repository;

        public ProjectController(IDataRepository repository)
        {
            _repository = repository;
        }

        // GET: /Project/
        public async Task<IActionResult> Index()
        {
            var projects = await _repository.GetAllProjectsAsync();
            return View(projects);
        }

        // GET: /Project/Create
        public IActionResult Create()
        {
            return View(new Project { StartDate = DateTime.Today, Status = "Active" });
        }

        // POST: /Project/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectName,StartDate,EndDate,HoursWorked")] Project project)
        {
            project.Status = "Active";

            if (ModelState.IsValid)
            {
                int newId = await _repository.AddProjectAsync(project);
                return RedirectToAction(nameof(Details), new { id = newId });
            }
            return View(project);
        }

        // GET: /Project/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var project = await _repository.GetProjectByIdAsync(id);
            if (project == null) return NotFound();

            var assignedEmployees = await _repository.GetEmployeesByProjectIdAsync(id);

            var viewModel = new ProjectDetailsViewModel
            {
                Project = project,
                AssignedEmployees = assignedEmployees
            };

            return View(viewModel);
        }

        // GET: /Project/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var project = await _repository.GetProjectByIdAsync(id);
            if (project == null) return NotFound();
            return View(project);
        }

        // POST: /Project/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Project project)
        {
            if (id != project.ProjectId || !ModelState.IsValid)
            {
                return View(project);
            }

            bool success = await _repository.UpdateProjectAsync(project);
            if (success)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Update failed or project not found.");
            return View(project);
        }
        public async Task<IActionResult> Overlap()
        {
            var overlaps = await _repository.FindOverlappingProjectsAsync();
            return View(overlaps);
        }

        // GET: Project/AssignEmployees/5
        public async Task<IActionResult> AssignEmployees(int id)
        {
            var project = await _repository.GetProjectByIdAsync(id);
            if (project == null) return NotFound();

            var unassignedEmployees = await _repository.GetUnassignedEmployeesForProjectAsync(id);

            var viewModel = new EmployeeAssignmentViewModel
            {
                ProjectId = id,
                ProjectName = project.ProjectName,
                AvailableEmployees = unassignedEmployees.Select(e => new SelectListItem
                {
                    Value = e.EmployeeId.ToString(),
                    Text = $"{e.FirstName} {e.LastName}"
                })
            };

            return View(viewModel);
        }

        // POST: Project/AssignEmployees/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignEmployees(EmployeeAssignmentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                int assignmentsCount = 0;
                foreach (var employeeId in viewModel.SelectedEmployeeIds)
                {
                    if (await _repository.AssignEmployeeToProjectAsync(employeeId, viewModel.ProjectId))
                    {
                        assignmentsCount++;
                    }
                }

                TempData["SuccessMessage"] = $"Successfully assigned {assignmentsCount} employees to project '{viewModel.ProjectName}'.";
                return RedirectToAction(nameof(Details), new { id = viewModel.ProjectId });
            }

            var unassignedEmployees = await _repository.GetUnassignedEmployeesForProjectAsync(viewModel.ProjectId);
            viewModel.AvailableEmployees = unassignedEmployees.Select(e => new SelectListItem
            {
                Value = e.EmployeeId.ToString(),
                Text = $"{e.FirstName} {e.LastName}"
            });

            return View(viewModel);
        }
        public async Task<IActionResult> UnassignEmployee(int employeeId, int projectId)
        {
            if (employeeId <= 0 || projectId <= 0)
            {
                return BadRequest("Invalid employee or project ID.");
            }

            bool success = await _repository.UnassignEmployeeFromProjectAsync(employeeId, projectId);

            if (success)
            {
                TempData["SuccessMessage"] = "Employee successfully unassigned from project.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to unassign employee (assignment record not found).";
            }

            return RedirectToAction(nameof(Details), new { id = projectId });
        }

        // POST: Project/QuickStatusUpdate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuickStatusUpdate(int projectId, string newStatus)
        {
            var project = await _repository.GetProjectByIdAsync(projectId);
            if (project == null)
            {
                TempData["ErrorMessage"] = "Project not found.";
                return RedirectToAction(nameof(Index));
            }

            project.Status = newStatus;
            bool success = await _repository.UpdateProjectAsync(project);

            if (success)
            {
                TempData["SuccessMessage"] = $"Project '{project.ProjectName}' status updated to '{newStatus}'.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update project status.";
            }

            return RedirectToAction(nameof(Details), new { id = projectId });
        }
    }
}
