using Employee_Management_System.Models;
using Employee_Management_System.Services;
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
            return View(department);
        }

        // GET: /Department/Create
        public IActionResult Create()
        {
            return View(); // Returns the empty Create.cshtml view
        }

        // POST: /Department/Create (Add Department)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Budget")] Department department)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Calls the SP_AddDepartment stored procedure
                    int newId = await _repository.AddDepartmentAsync(department);

                    // Redirect to the list view or details view of the new department
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    // Log the error (e.g., database connection failure, SP error)
                    ModelState.AddModelError("", "Unable to save changes. Please try again. Error: " + ex.Message);
                }
            }
            // If model state is invalid or exception occurred, return the view with the current model
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
        public async Task<IActionResult> Edit(int id, [Bind("DepartmentId,Name,Budget")] Department department)
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
