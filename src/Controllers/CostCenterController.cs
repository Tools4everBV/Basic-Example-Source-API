using EXAMPLE.SOURCE.API.Data.Models;
using EXAMPLE.SOURCE.API.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace EXAMPLE.SOURCE.API.Controllers
{
    [Route("api/costcenters")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "4-CostCenters", IgnoreApi = true)]
    public class CostCenterController : ControllerBase
    {
        private readonly MockDataService _mockDataService;

        public CostCenterController(MockDataService mockDataService)
        {
            _mockDataService = mockDataService;
        }

        // GET: api/costcenters
        /// <summary>
        /// Get all costcenters
        /// </summary>
        /// <remarks>
        /// <h2>Implementation notes</h2>
        /// This API call is merely added for development purposes.
        /// </remarks>
        /// <returns>List of all costcenters</returns>
        /// <response code="200">Returns the list of all costcenter</response>
        [HttpGet]
        public ActionResult<List<CostCenter>> Get()
        {
            var obj = _mockDataService.CostCenters;
            return Ok(obj);
        }

        // GET: api/costcenters/{id}
        /// <summary>
        /// Get costcenter by Id
        /// </summary>
        /// <remarks>
        /// <h2>Implementation notes</h2>
        /// This API call is merely added for development purposes.
        /// </remarks>
        /// <returns>A single costcenter</returns>
        /// <response code="200">Returns a single costcenter</response>
        [HttpGet("{id}")]
        public ActionResult<CostCenter> GetById(int id)
        {
            var obj = _mockDataService.CostCenters.FirstOrDefault(c => c.Id == id);
            if (obj == null)
                return NotFound();

            return Ok(obj);
        }
    }
}
