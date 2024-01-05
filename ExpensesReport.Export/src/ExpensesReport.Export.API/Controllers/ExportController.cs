using ExpensesReport.Export.Application.Services;
using ExpensesReport.Export.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ExpensesReport.Export.API.Controllers
{
    [ApiController]
    [Route("api/export")]
    public class ExportController : ControllerBase
    {
        private readonly IExportServices _exportServices;

        public ExportController(IExportServices exportServices)
        {
            _exportServices = exportServices;
        }

        /// <summary>
        /// Get all users and identities
        /// </summary>
        /// <returns>Exported file</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpGet("users-and-identities")]
        [ProducesResponseType(typeof(FileViewModel), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> ExportUsersAndIdentites()
        {
            if (!string.IsNullOrEmpty(Request.Headers["Authorization"])
                && Request.Headers["Authorization"].ToString().StartsWith("Bearer "))
            {
                _exportServices.Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            }

            var fileViewModel = await _exportServices.exportUsersAndIdentites();

            return Ok(fileViewModel);
        }

        /// <summary>
        /// Get all departments
        /// </summary>
        /// <returns>Exported file</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpGet("departments")]
        [ProducesResponseType(typeof(FileViewModel), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> ExportDepartments()
        {
            if (!string.IsNullOrEmpty(Request.Headers["Authorization"])
                && Request.Headers["Authorization"].ToString().StartsWith("Bearer "))
            {
                _exportServices.Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            }

            var fileViewModel = await _exportServices.exportDepartments();

            return Ok(fileViewModel);
        }

        /// <summary>
        /// Get all projects
        /// </summary>
        /// <returns>Exported file</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpGet("projects")]
        [ProducesResponseType(typeof(FileViewModel), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> ExportProjects()
        {
            if (!string.IsNullOrEmpty(Request.Headers["Authorization"])
                && Request.Headers["Authorization"].ToString().StartsWith("Bearer "))
            {
                _exportServices.Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            }

            var fileViewModel = await _exportServices.exportProjects();

            return Ok(fileViewModel);
        }

        /// <summary>
        /// Get all expense accounts
        /// </summary>
        /// <returns>Exported file</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpGet("expense-accounts")]
        [ProducesResponseType(typeof(FileViewModel), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> ExportExpenseAccounts()
        {
            if (!string.IsNullOrEmpty(Request.Headers["Authorization"])
                && Request.Headers["Authorization"].ToString().StartsWith("Bearer "))
            {
                _exportServices.Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            }

            var fileViewModel = await _exportServices.exportExpenseAccounts();

            return Ok(fileViewModel);
        }

        /// <summary>
        /// Get all expense reports
        /// </summary>
        /// <returns>Exported file</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpGet("expense-reports")]
        [ProducesResponseType(typeof(FileViewModel), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> ExportExpenseReports()
        {
            if (!string.IsNullOrEmpty(Request.Headers["Authorization"])
                && Request.Headers["Authorization"].ToString().StartsWith("Bearer "))
            {
                _exportServices.Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            }

            var fileViewModel = await _exportServices.exportExpenseReports();

            return Ok(fileViewModel);
        }

        /// <summary>
        /// Get a expense report by id
        /// </summary>
        /// <param name="id">Expense report identifier</param>
        /// <returns>Exported file</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpGet("expense-reports/{id}")]
        [ProducesResponseType(typeof(FileViewModel), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> ExportExpenseReport(string id)
        {
            if (!string.IsNullOrEmpty(Request.Headers["Authorization"])
                && Request.Headers["Authorization"].ToString().StartsWith("Bearer "))
            {
                _exportServices.Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            }

            var fileViewModel = await _exportServices.exportExpenseReport(id);

            return Ok(fileViewModel);
        }
    }
}
