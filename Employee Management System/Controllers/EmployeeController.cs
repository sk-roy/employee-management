using Employee_Management_System.Models;
using Employee_Management_System.Models.ViewModels;
using Employee_Management_System.Services;
using Employee_Management_System.Utilities;
using Employee_Management_System.Utilities.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection.Metadata.Ecma335;

namespace Employee_Management_System.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IDataRepository _repository;

        public EmployeeController(IDataRepository repository)
        {
            _repository = repository;
        }

        // GET: EmployeeController
        public async Task<ActionResult> Index()
        {
            var employees = await _repository.GetAllEmployeesAsync();
            return View(employees);
        }

        // GET: EmployeeController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var employee = await _repository.GetEmployeeByIdAsync(id);
            return View(employee);
        }

        // GET: EmployeeController/Create
        public async Task<ActionResult> Create()
        {
            var viewModel = await PrepareEmployeeViewModel();

            return View(viewModel);
        }

        private async Task<EmployeeViewModel> PrepareEmployeeViewModel()
        {
            var departments = await _repository.GetAllDepartmentsAsync();
            var managers = await _repository.GetManagersAsync();

            return new EmployeeViewModel
            {
                DepartmentList = departments.Select(d => new SelectListItem
                {
                    Value = d.DepartmentId.ToString(),
                    Text = d.Name
                }),

                ManagerList = managers.Select(e => new SelectListItem
                {
                    Value = e.EmployeeId.ToString(),
                    Text = $"{e.FirstName} {e.LastName}"
                })
            };
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Employee model = viewModel.ToEmployeeModel();

                    int newId = await _repository.AddEmployeeAsync(model);
                    if (newId == -1)
                    {
                        var newViewModel = await PrepareEmployeeViewModel();
                        ModelState.AddModelError("Email", "The email address is already used.");

                        return View(newViewModel);
                    }
                    else if (newId > 0)
                    {
                        return RedirectToAction(nameof(Details), new { id = newId });
                    }
                }
                catch (BudgetExceededException ex)
                {
                    var newViewModel = await PrepareEmployeeViewModel();
                    ModelState.AddModelError("", ex.Message);
                    return View(newViewModel);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Failed to create employee: " + ex.Message);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // --- Helper to Load Dropdowns ---
        private async Task<(IEnumerable<SelectListItem> Depts, IEnumerable<SelectListItem> Mgrs)> LoadLookupsAsync()
        {
            var departments = await _repository.GetAllDepartmentsAsync();
            var managers = await _repository.GetManagersAsync();

            var deptList = departments.Select(d => new SelectListItem(d.Name, d.DepartmentId.ToString()));
            var mgrList = managers.Select(m => new SelectListItem(m.FullName, m.ManagerId.ToString()));

            return (deptList, mgrList);
        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _repository.GetEmployeeByIdAsync(id);
            if (employee == null) return NotFound();

            var lookups = await LoadLookupsAsync();

            employee.DepartmentList = lookups.Depts;
            employee.ManagerList = lookups.Mgrs;

            return View(employee);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeViewModel model)
        {
            if (id != model.EmployeeId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    bool success = await _repository.UpdateEmployeeAsync(model);
                    if (success) return RedirectToAction(nameof(Index));
                    ModelState.AddModelError("", "Update failed. Employee ID not found.");
                }
                catch (BudgetExceededException ex)
                {
                    var newViewModel = await PrepareEmployeeViewModel();
                    ModelState.AddModelError("", ex.Message);
                    return View(newViewModel);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An unexpected error occurred during save: " + ex.Message);
                }
            }

            return View(model);
        }

        // POST: Employee/Deactivate/5 (Action to set IsActive=false)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deactivate(int id)
        {
            bool success = await _repository.DeactivateEmployeeAsync(id);
            if (success)
            {
                TempData["Message"] = $"Employee ID {id} has been deactivated.";
            }
            else
            {
                TempData["Error"] = $"Deactivation failed for Employee ID {id}.";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Employee/ListByDepartment/5
        public async Task<IActionResult> ListByDepartment(int departmentId)
        {
            var employees = await _repository.GetEmployeesByDepartmentAsync(departmentId);
            return View("Index", employees);
        }
    }
}
