using ExpensesReport.Files.Application.Services;
using ExpensesReport.Files.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesReport.Files.API.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FilesController : ControllerBase
    {
        private readonly IFilesServices _filesServices;

        public FilesController(IFilesServices filesServices)
        {
            _filesServices = filesServices;
        }

        /// <summary>
        /// Get a file by name
        /// </summary>
        /// <param name="name">Name of the file</param>
        /// <returns>File data</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpGet("{name}")]
        [ProducesResponseType(typeof(FileViewModel), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> GetFileName(string name)
        {
            var file = await _filesServices.GetFileByName(name);
            return Ok(file);
        }

        /// <summary>
        /// Get all files
        /// </summary>
        /// <returns>File collection</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FileViewModel>), 200)]
        public async Task<IActionResult> GetFiles()
        {
            var files = await _filesServices.GetFiles();
            return Ok(files);
        }

        /// <summary>
        /// Upload a file
        /// </summary>
        /// <returns></returns>
        /// <response code="201">Created</response>
        /// <response code="400">Bad request</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(FileViewModel), 201)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        public async Task<IActionResult> UploadFile(IFormFile inputFile)
        {
            var file = await _filesServices.UploadFile(inputFile);
            return CreatedAtAction(nameof(GetFileName), new { name = file.Name }, file);
        }

        /// <summary>
        /// Delete a file by name
        /// </summary>
        /// <returns></returns>
        /// <response code="204">Success</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpDelete("{name}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> DeleteFile(string name)
        {
            await _filesServices.DeleteFile(name);
            return NoContent();
        }
    }
}
