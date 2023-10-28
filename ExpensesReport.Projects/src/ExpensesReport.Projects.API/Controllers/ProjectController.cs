using ExpensesReport.Projects.Application.InputModels;
using ExpensesReport.Projects.Application.Services;
using ExpensesReport.Projects.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesReport.Projects.API.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectServices _projectServices;

        public ProjectController(IProjectServices projectServices)
        {
            _projectServices = projectServices;
        }

        /// <summary>
        /// Get a project by id
        /// </summary>
        /// <param name="id">Project identifier</param>
        /// <returns>Project data</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProjectViewModel), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> GetProjectById(Guid id)
        {
            var project = await _projectServices.GetProjectById(id);
            return Ok(project);
        }

        /// <summary>
        /// Get a project by code
        /// </summary>
        /// <param name="code">Project code</param>
        /// <returns>Project data</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        [HttpGet("code/{code}")]
        [ProducesResponseType(typeof(ProjectViewModel), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> GetProjectByCode(string code)
        {
            var project = await _projectServices.GetProjectByCode(code);
            return Ok(project);
        }

        /// <summary>
        /// Get all projects
        /// </summary>
        /// <returns>Project collection</returns>
        /// <response code="200">Success</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProjectViewModel>), 200)]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _projectServices.GetAllProjects();
            return Ok(projects);
        }

        /// <summary>
        /// Register a project
        /// </summary>
        /// <param name="inputModel">Model with project data</param>
        /// <returns>Newly created project</returns>
        /// <response code="201">Created</response>
        /// <response code="400">Bad request</response>
        [HttpPost]
        [ProducesResponseType(typeof(ProjectViewModel), 201)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        public async Task<IActionResult> RegisterProject(AddProjectInputModel inputModel)
        {
            var project = await _projectServices.AddProject(inputModel);
            return CreatedAtAction(nameof(GetProjectById), new { id = project.Id }, project);
        }

        /// <summary>
        /// Update a project
        /// </summary>
        /// <param name="id">Project identifier</param>
        /// <param name="inputModel">Model with project data to update</param>
        /// <returns></returns>
        /// <response code="204">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> UpdateProject(Guid id, ChangeProjectInputModel inputModel)
        {
            await _projectServices.UpdateProject(id, inputModel);
            return NoContent();
        }

        /// <summary>
        /// Delete a project
        /// </summary>
        /// <param name="id">Project identifier</param>
        /// <returns></returns>
        /// <response code="204">Success</response>
        /// <response code="404">Not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            await _projectServices.DeleteProject(id);
            return NoContent();
        }

        /// <summary>
        /// Activate a project
        /// </summary>
        /// <param name="id">Project identifier</param>
        /// <returns></returns>
        /// <response code="204">Success</response>
        /// <response code="404">Not found</response>
        [HttpPatch("{id}/activate")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> ActivateProject(Guid id)
        {
            await _projectServices.ActivateProject(id);
            return NoContent();
        }

        /// <summary>
        /// Get all projects from a departament
        /// </summary>
        /// <param name="departamentId">Departament identifier</param>
        /// <returns>Project collection</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        [HttpGet("departament/{departamentId}")]
        [ProducesResponseType(typeof(IEnumerable<ProjectViewModel>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> GetProjectsByDepartamentId(Guid departamentId)
        {
            var projects = await _projectServices.GetProjectsByDepartamentId(departamentId);
            return Ok(projects);
        }
    }
}
