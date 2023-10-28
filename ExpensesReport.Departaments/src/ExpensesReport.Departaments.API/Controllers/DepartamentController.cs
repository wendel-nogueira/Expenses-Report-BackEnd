using ExpensesReport.Departaments.Application.InputModels;
using ExpensesReport.Departaments.Application.Services;
using ExpensesReport.Departaments.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesReport.Departaments.API.Controllers
{
    [ApiController]
    [Route("api/departaments")]
    public class DepartamentController : ControllerBase
    {
        private readonly IDepartamentServices _departamentServices;

        public DepartamentController(IDepartamentServices departamentServices)
        {
            _departamentServices = departamentServices;
        }

        /// <summary>
        /// Get a departament by id
        /// </summary>
        /// <param name="id">Departament identifier</param>
        /// <returns>Departament data</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DepartamentViewModel), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> GetDepartamentById(Guid id)
        {
            var departament = await _departamentServices.GetDepartamentById(id);
            return Ok(departament);
        }

        /// <summary>
        /// Get a departament by name
        /// </summary>
        /// <param name="name">Departament name</param>
        /// <returns>Departament data</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpGet("name/{name}")]
        [ProducesResponseType(typeof(DepartamentViewModel), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> GetDepartamentByName(string name)
        {
            var departament = await _departamentServices.GetDepartamentByName(name);
            return Ok(departament);
        }

        /// <summary>
        /// Get a departament by acronym
        /// </summary>
        /// <param name="acronym">Departament acronym</param>
        /// <returns>Departament data</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpGet("acronym/{acronym}")]
        [ProducesResponseType(typeof(DepartamentViewModel), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> GetDepartamentByAcronym(string acronym)
        {
            var departament = await _departamentServices.GetDepartamentByAcronym(acronym);
            return Ok(departament);
        }

        /// <summary>
        /// Get all departaments
        /// </summary>
        /// <returns>Departament collection</returns>
        /// <response code="200">Success</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DepartamentViewModel>), 200)]
        public async Task<IActionResult> GetAllDepartaments()
        {
            var departaments = await _departamentServices.GetAllDepartaments();
            return Ok(departaments);
        }

        /// <summary>
        /// Register a departament
        /// </summary>
        /// <param name="inputModel">Model with departament data</param>
        /// <returns>Newly created departament</returns>
        /// <response code="201">Created</response>
        /// <response code="400">Bad request</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(DepartamentViewModel), 201)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        public async Task<IActionResult> RegisterDepartament(AddDepartamentInputModel inputModel)
        {
            var departament = await _departamentServices.AddDepartament(inputModel);
            return CreatedAtAction(nameof(GetDepartamentById), new { id = departament.Id }, departament);
        }

        /// <summary>
        /// Update a departament
        /// </summary>
        /// <param name="id">Departament identifier</param>
        /// <param name="inputModel">Model with departament data to update</param>
        /// <returns></returns>
        /// <response code="204">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> UpdateDepartament(Guid id, ChangeDepartamentInputModel inputModel)
        {
            await _departamentServices.UpdateDepartament(id, inputModel);
            return NoContent();
        }

        /// <summary>
        /// Delete a departament
        /// </summary>
        /// <param name="id">Departament identifier</param>
        /// <returns></returns>
        /// <response code="204">Success</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> DeleteDepartament(Guid id)
        {
            await _departamentServices.DeleteDepartament(id);
            return NoContent();
        }

        /// <summary>
        /// Activate a departament
        /// </summary>
        /// <param name="id">Departament identifier</param>
        /// <returns></returns>
        /// <response code="204">Success</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpPatch("{id}/activate")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> ActivateDepartament(Guid id)
        {
            await _departamentServices.ActivateDepartament(id);
            return NoContent();
        }

        /// <summary>
        /// Get all managers from a departament
        /// </summary>
        /// <param name="departamentId">Departament identifier</param>
        /// <returns>Manager collection</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpGet("{departamentId}/managers")]
        [ProducesResponseType(typeof(IEnumerable<ManagerViewModel>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> GetDepartamentManagers(Guid departamentId)
        {
            var managers = await _departamentServices.GetDepartamentManagers(departamentId);
            return Ok(managers);
        }

        /// <summary>
        /// Add a manager to a departament
        /// </summary>
        /// <param name="departamentId">Departament identifier</param>
        /// <param name="managerId">Manager identifier</param>
        /// <returns></returns>
        /// <response code="204">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpPost("{departamentId}/managers/{managerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> AddDepartamentManager(Guid departamentId, Guid managerId)
        {
            await _departamentServices.AddDepartamentManager(departamentId, managerId);
            return NoContent();
        }

        /// <summary>
        /// Remove a manager from a departament
        /// </summary>
        /// <param name="departamentId">Departament identifier</param>
        /// <param name="managerId">Manager identifier</param>
        /// <returns></returns>
        /// <response code="204">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpDelete("{departamentId}/managers/{managerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> RemoveDepartamentManager(Guid departamentId, Guid managerId)
        {
            await _departamentServices.RemoveDepartamentManager(departamentId, managerId);
            return NoContent();
        }

        /// <summary>
        /// Get all users from a departament
        /// </summary>
        /// <param name="departamentId">Departament identifier</param>
        /// <returns>User collection</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpGet("{departamentId}/users")]
        [ProducesResponseType(typeof(IEnumerable<ManagerViewModel>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> GetDepartamentUsers(Guid departamentId)
        {
            var users = await _departamentServices.GetDepartamentUsers(departamentId);
            return Ok(users);
        }

        /// <summary>
        /// Add a user to a departament
        /// </summary>
        /// <param name="departamentId">Departament identifier</param>
        /// <param name="userId">User identifier</param>
        /// <returns></returns>
        /// <response code="204">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpPost("{departamentId}/users/{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> AddDepartamentUser(Guid departamentId, Guid userId)
        {
            await _departamentServices.AddDepartamentUser(departamentId, userId);
            return NoContent();
        }

        /// <summary>
        /// Remove a user from a departament
        /// </summary>
        /// <param name="departamentId">Departament identifier</param>
        /// <param name="userId">User identifier</param>
        /// <returns></returns>
        /// <response code="204">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpDelete("{departamentId}/users/{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> RemoveDepartamentUser(Guid departamentId, Guid userId)
        {
            await _departamentServices.RemoveDepartamentUser(departamentId, userId);
            return NoContent();
        }

        /// <summary>
        /// Get all departaments from a manager and user
        /// </summary>
        /// <param name="id">Manager and user identifier</param>
        /// <returns>Departament collection</returns>
        /// <response code="200">Success</response>
        [Authorize]
        [HttpGet("allRelated/{id}")]
        [ProducesResponseType(typeof(IEnumerable<DepartamentViewModel>), 200)]
        public async Task<IActionResult> GetDepartamentsByManagerAndUser(Guid id)
        {
            var departaments = await _departamentServices.GetDepartamentsByManagerAndUser(id);
            return Ok(departaments);
        }
    }
}
