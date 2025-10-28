using Employee_Management_System.Models;
using Employee_Management_System.Services;
using Employee_Management_System.Utilities;
using Employee_Management_System.Utilities.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Employee_Management_System.Controllers
{

    public class DepartmentController : Controller
    {
        private readonly IDataRepository _repository;

        public DepartmentController(IDataRepository repository)
        {
            _repository = repository;
        }

        //GET: /Department/Index (Get All)
        public async Task<IActionResult> Index()
        {
            var departments = await _repository.GetAllDepartmentsAsync();
            return View(departments);
        }

        // GET: /Department/Details/5 (Get By ID)
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var department = await _repository.GetDepartmentByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            var viewModel = department.ToDepartmentViewModel();
            viewModel.Employees = await _repository.GetEmployeesByDepartmentAsync(id);

            return View(viewModel);
        }

        // GET: /Department/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Department/Create (Add Department)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Budget,Spent")] Department department)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int newId = await _repository.AddDepartmentAsync(department);
                    return RedirectToAction(nameof(Index));
                }
                catch (BudgetExceededException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(department);
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError("", "Unable to save changes. Please try again. Error: " + ex.Message);
                }
            }

            return View(department);
        }

        // GET: /Department/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var department = await _repository.GetDepartmentByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        // POST: /Department/Edit/5 (Update Department)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DepartmentId,Name,Budget,Spent")] Department department)
        {
            if (id != department.DepartmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bool success = await _repository.UpdateDepartmentAsync(department);

                    if (success)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Department not found or update failed.");
                    }
                }
                catch (BudgetExceededException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(department);
                }
                catch (System.Exception ex)
                {
                    // Log the error
                    ModelState.AddModelError("", "Unable to save changes. Please try again. Error: " + ex.Message);
                }
            }
            return View(department);
        }
    }
}
