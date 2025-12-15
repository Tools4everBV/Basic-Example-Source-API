using EXAMPLE.SOURCE.API.Data.Models;
using EXAMPLE.SOURCE.API.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace EXAMPLE.SOURCE.API.Controllers
{
    [Route("api/jobtitles")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "5-JobTitles", IgnoreApi = true)]
    public class JobTitleController : ControllerBase
    {
        private readonly MockDataService _mockDataService;

        public JobTitleController(MockDataService mockDataService)
        {
            _mockDataService = mockDataService;
        }

        // GET: api/jobtitles
        /// <summary>
        /// Get all jobtitles
        /// </summary>
        /// <remarks>
        /// <h2>Implementation notes</h2>
        /// This API call is merely added for development purposes.
        /// </remarks>
        /// <returns>List of all jobtitles</returns>
        /// <response code="200">Returns the list of all jobtitles</response>
        [HttpGet]
        public ActionResult<List<JobTitle>> Get()
        {
            var obj = _mockDataService.JobTitles;
            return Ok(obj);
        }

        // GET: api/jobtitles/{id}
        /// <summary>
        /// Get jobtitle by Id
        /// </summary>
        /// <remarks>
        /// <h2>Implementation notes</h2>
        /// This API call is merely added for development purposes.
        /// </remarks>
        /// <returns>A single jobTitle</returns>
        /// <response code="200">Returns a single jobTitle</response>
        [HttpGet("{id}")]
        public ActionResult<JobTitle> GetById(int id)
        {
            var obj = _mockDataService.JobTitles.FirstOrDefault(c => c.Id == id);
            if (obj == null)
                return NotFound();

            return Ok(obj);
        }
    }
}
