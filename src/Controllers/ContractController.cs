using EXAMPLE.SOURCE.API.Data.Models;
using EXAMPLE.SOURCE.API.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace EXAMPLE.SOURCE.API.Controllers
{
    [Route("api/contracts")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "3-Contracts")]
    public class ContractController : ControllerBase
    {
        private readonly MockDataService _mockDataService;

        public ContractController(MockDataService mockDataService)
        {
            _mockDataService = mockDataService;
        }

        // GET: api/contracts
        /// <summary>
        /// Get all contracts
        /// </summary>
        /// <remarks>
        /// <h2>Implementation notes</h2>
        /// Contracts are retrieved by getting all employees. This API call is merely added for development purposes.
        /// </remarks>
        /// <returns>List of all contracts</returns>
        /// <response code="200">Returns the list of all contracts</response>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult<List<Contract>> Get()
        {
            var obj = _mockDataService.Contracts;
            return Ok(obj);
        }

        // GET: api/contracts/{id}
        /// <summary>
        /// Get contract by Id
        /// </summary>
        /// <remarks>
        /// <h2>Implementation notes</h2>
        /// This API call is merely added for development purposes.
        /// </remarks>
        /// <returns>A single contract</returns>
        /// <response code="200">Returns a single contract</response>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("{id}")]
        public ActionResult<Contract> GetById(int id)
        {
            var obj = _mockDataService.Contracts.FirstOrDefault(c => c.Id == id);
            if (obj == null)
                return NotFound();

            return Ok(obj);
        }
    }
}
