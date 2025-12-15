using EXAMPLE.SOURCE.API.Data.Models;
using EXAMPLE.SOURCE.API.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace EXAMPLE.SOURCE.API.Controllers
{
    [Route("api/departments")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "2-Departments")]
    public class DepartmentController : ControllerBase
    {
        private readonly MockDataService _mockDataService;

        public DepartmentController(MockDataService mockDataService)
        {
            _mockDataService = mockDataService;
        }

        // GET: api/departments
        /// <summary>
        /// Get all departments
        /// </summary>
        /// <remarks>
        /// <h2>Implementation notes</h2>
        /// A department represents an organizational unit. 
        /// Each department has an internal ID, name, code, external identifier, and a reference to the manager assigned to oversee it.
        /// Departmental information is essential because many business rules -and ultimately- the entitlements a person receives, 
        /// are determined by attributes related to the individual, their contract, or the department they belong to.
        /// </remarks>
        /// <returns>List of all departments</returns>
        /// <response code="200">Returns the list of all departments</response>
        [HttpGet]
        public ActionResult<List<Department>> Get()
        {
            var obj = _mockDataService.Departments;
            return Ok(obj);
        }

        // GET: api/departments/{id}
        /// <summary>
        /// Get department by Id
        /// </summary>
        /// <remarks>
        /// <h2>Implementation notes</h2>
        /// This API call is merely added for development purposes.
        /// </remarks>
        /// <returns>A single department</returns>
        /// <response code="200">Returns a single department</response>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("{id}")]
        public ActionResult<Department> GetById(int id)
        {
            var obj = _mockDataService.Departments.FirstOrDefault(c => c.Id == id);
            if (obj == null)
                return NotFound();

            return Ok(obj);
        }
    }
}
