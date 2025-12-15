using EXAMPLE.SOURCE.API.Data.Models;
using EXAMPLE.SOURCE.API.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace EXAMPLE.SOURCE.API.Controllers
{
    [Route("api/employees")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "1-Employees")]
    public class EmployeeController : ControllerBase
    {
        private readonly MockDataService _mockDataService;

        public EmployeeController(MockDataService mockDataService)
        {
            _mockDataService = mockDataService;
        }

        // GET: api/employees
        /// <summary>
        /// Get all employees
        /// </summary>
        /// <remarks>
        /// <h2>Implementation notes</h2>
        /// An employee represents a person within the organization and contains both identity information(name, display rules, partner naming), 
        /// business contact information, system identifiers, and a collection of related contracts.
        /// </remarks>
        /// <returns>List of all employees</returns>
        /// <response code="200">Returns the list of all employees</response>
        [HttpGet]
        public ActionResult<List<Employee>> Get()
        {
            var obj = _mockDataService.Employees;
            return Ok(obj);
        }

        // GET: api/employees/{id}
        /// <summary>
        /// Get employee by Id
        /// </summary>
        /// <remarks>
        /// <h2>Implementation notes</h2>
        /// This API call is merely added for development purposes.
        /// </remarks>
        /// <returns>A single employee</returns>
        /// <response code="200">Returns a single employee</response>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("{id}")]
        public ActionResult<Employee> GetById(int id)
        {
            var obj = _mockDataService.Employees.FirstOrDefault(c => c.Id == id);
            if (obj == null)
                return NotFound();
            return Ok(obj);
        }

        // PATCH: api/employees/{id}/businessemail
        /// <summary>
        ///  Update the `businessEmail` of an employee
        /// </summary>
        /// <remarks>
        /// <h2>Implementation notes</h2>
        /// In addition to retrieving all employees, we also need an API call to update the `businessEmail` address of an employee. 
        /// Email addresses are generally provisioned by identity platforms such as Active Directory or Entra ID.Therefore, 
        /// the HR system must be able to receive and store externally generated email addresses through a dedicated update operation.
        /// </remarks>
        /// <returns>The updated employee</returns>
        /// <response code="200">Returns the updated employee</response>
        [HttpPatch("{id}/businessemail")]
        public IActionResult UpdateBusinessEmail(int id, [FromBody] UpdateEmployeeEmailRequest request)
        {
            var employee = _mockDataService.Employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
                return NotFound();
            if (string.IsNullOrWhiteSpace(request.BusinessEmail))
                return BadRequest("BusinessEmail cannot be empty.");
            employee.BusinessEmail = request.BusinessEmail.Trim().ToLower();
            return Ok(employee);
        }
    }

    /// <summary>
    /// Request payload for updating an employee's business email address.
    /// </summary>
    public class UpdateEmployeeEmailRequest
    {
        /// <summary>
        /// The new business email address for the employee.
        /// </summary>
        public string BusinessEmail { get; set; }
    }
}
