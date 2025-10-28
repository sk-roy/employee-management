using Employee_Management_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Employee_Management_System.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly IDataRepository _repository;

        public OrganizationController(IDataRepository repository)
        {
            _repository = repository;
        }
        public async Task<IActionResult> Index()
        {
            var hierarchyData = await _repository.GetOrganizationHierarchyAsync();

            return View(hierarchyData);
        }
    }
}
