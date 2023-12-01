using ExpensesReport.Identity.Application.InputModels;
using ExpensesReport.Identity.Application.Services;
using ExpensesReport.Identity.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesReport.Identity.API.Controllers
{
    [ApiController]
    [Route("api/identity")]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityServices _identityServices;

        public IdentityController(IIdentityServices identityServices, IConfiguration config)
        {
            _identityServices = identityServices;
        }

        /// <summary>
        /// Get a identity by id
        /// </summary>
        /// <param name="id">Identity identifier</param>
        /// <returns>Identity data</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IdentityViewModel), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> GetIdentityById(Guid id)
        {
            var identity = await _identityServices.GetIdentityById(id);

            return Ok(identity);
        }

        /// <summary>
        /// Get a identity by token
        /// </summary>
        /// <returns>Identity data</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(typeof(IdentityCheckViewModel), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> GetMe()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            var identity = await _identityServices.GetMe(token);

            return Ok(identity);
        }

        /// <summary>
        /// Get a identity by email
        /// </summary>
        /// <param name="email">Identity email</param>
        /// <returns>Identity data</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpGet("email/{email}")]
        [ProducesResponseType(typeof(IdentityViewModel), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> GetIdentityByEmail(string email)
        {
            var identity = await _identityServices.GetIdentityByEmail(email);

            return Ok(identity);
        }

        /// <summary>
        /// Get all identities
        /// </summary>
        /// <returns>Identity collection</returns>
        /// <response code="200">Success</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<IdentityViewModel>), 200)]
        public async Task<IActionResult> GetAllIdentities()
        {
            var identities = await _identityServices.GetAll();

            return Ok(identities);
        }

        /// <summary>
        /// Get all identities by role
        /// </summary>
        /// <returns>Identity collection</returns>
        /// <response code="200">Success</response>
        [Authorize]
        [HttpGet("role/{role}")]
        [ProducesResponseType(typeof(IEnumerable<IdentityViewModel>), 200)]
        public async Task<IActionResult> GetAllIdentitiesByRole(string role)
        {
            var identities = await _identityServices.GetAllByRole(role);

            return Ok(identities);
        }

        /// <summary>
        /// Get all roles
        /// </summary>
        /// <returns>Role collection</returns>
        /// <response code="200">Success</response>
        [Authorize]
        [HttpGet("roles/all")]
        [ProducesResponseType(typeof(IEnumerable<RoleViewModel>), 200)]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _identityServices.GetAllRoles();

            return Ok(roles);
        }

        /// <summary>
        /// Login a identity
        /// </summary>
        /// <param name="inputModel">Model with login data</param>
        /// <returns>Identity authentication data</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        /// <response code="400">Bad request</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthenticationViewModel), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        public async Task<IActionResult> LoginIdentity([FromBody] LoginInputModel inputModel)
        {
            var auth = await _identityServices.Login(inputModel);

            return Ok(auth);
        }

        /// <summary>
        /// Request a reset password email
        /// </summary>
        /// <param name="inputModel">Model with identity email data</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        /// <response code="400">Bad request</response>
        [HttpPost("password/reset")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> RequestResetPassword([FromBody] ResetPasswordInputModel inputModel)
        {
            await _identityServices.SendResetPasswordEmail(inputModel);

            return NoContent();
        }

        /// <summary>
        /// Register a identity
        /// </summary>
        /// <param name="inputModel">Model with identity data</param>
        /// <returns>Newly created identity</returns>
        /// <response code="201">Created</response>
        /// <response code="400">Bad request</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(IdentityViewModel), 201)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        public async Task<IActionResult> AddIdentity([FromBody] AddIdentityInputModel inputModel)
        {
            var identity = await _identityServices.AddIdentity(inputModel);

            return CreatedAtAction(nameof(GetIdentityById), new { id = identity.Id }, identity);
        }

        /// <summary>
        /// Update a identity password
        /// </summary>
        /// <param name="inputModel">Model with identity password data</param>
        /// <param name="token">Reset password token</param>
        /// <returns></returns>
        /// <response code="204">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not found</response>
        [HttpPut("password")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> UpdateIdentityPassword([FromQuery] string token, [FromBody] ChangePasswordInputModel inputModel)
        {
            await _identityServices.UpdateIdentityPassword(token, inputModel);

            return NoContent();
        }

        /// <summary>
        /// Update a identity email
        /// </summary>
        /// <param name="inputModel">Model with identity email data</param>
        /// <returns></returns>
        /// <response code="204">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpPut("email")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> UpdateIdentityEmail([FromBody] ChangeEmailInputModel inputModel)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            var tokenDecoded = AuthServices.DecodeToken(token);
            var identityId = new Guid(tokenDecoded[0].Value);

            await _identityServices.UpdateIdentityEmail(identityId, inputModel);

            return NoContent();
        }

        /// <summary>
        /// Update a identity role
        /// </summary>
        /// <param name="id">Identity identifier</param>
        /// <param name="inputModel">Model with identity role data</param>
        /// <returns></returns>
        /// <response code="204">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpPut("{id}/role")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> UpdateIdentityRole(Guid id, [FromBody] ChangeIdentityRoleInputModel inputModel)
        {
            await _identityServices.UpdateIdentityRole(id, inputModel);

            return NoContent();
        }

        /// <summary>
        /// Activate a identity
        /// </summary>
        /// <param name="id">Identity identifier</param>
        /// <returns></returns>
        /// <response code="204">Success</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpPatch("{id}/activate")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> ActivateIdentity(Guid id)
        {
            await _identityServices.ActivateIdentity(id);

            return NoContent();
        }

        /// <summary>
        /// Deactivate a identity
        /// </summary>
        /// <param name="id">Identity identifier</param>
        /// <returns></returns>
        /// <response code="204">Success</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpDelete("{id}/deactivate")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> DeactivateIdentity(Guid id)
        {
            await _identityServices.DeactivateIdentity(id);

            return NoContent();
        }
    }
}
