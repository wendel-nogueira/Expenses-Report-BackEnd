﻿using ExpensesReport.Users.Application.InputModels;
using ExpensesReport.Users.Application.Services;
using ExpensesReport.Users.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesReport.Users.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserServices _userServices;

        public UsersController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        /// <summary>
        /// Get a user by id
        /// </summary>
        /// <param name="id">User identifier</param>
        /// <returns>User data</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserViewModel), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userServices.GetUserById(id);
            return Ok(user);
        }

        /// <summary>
        /// Get a user by email
        /// </summary>
        /// <param name="email">User email</param>
        /// <returns>User data</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        [HttpGet("email/{email}")]
        [ProducesResponseType(typeof(UserViewModel), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _userServices.GetUserByEmail(email);
            return Ok(user);
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>User collection</returns>
        /// <response code="200">Success</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserViewModel>), 200)]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userServices.GetUsers();
            return Ok(users);
        }

        /// <summary>
        /// Register a user
        /// </summary>
        /// <param name="inputModel">Model with user data</param>
        /// <returns>Newly created user</returns>
        /// <response code="201">Created</response>
        /// <response code="400">Bad request</response>
        [HttpPost]
        [ProducesResponseType(typeof(UserViewModel), 201)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        public async Task<IActionResult> AddUser(AddUserInputModel inputModel)
        {
            var user = await _userServices.AddUser(inputModel);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="id">User identifier</param>
        /// <param name="inputModel">Model with user data to update</param>
        /// <returns>Updated user</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserViewModel), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> UpdateUser(Guid id, UpdateUserInputModel inputModel)
        {
            var user = await _userServices.UpdateUser(id, inputModel);
            return Ok(user);
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="id">User identifier</param>
        /// <returns></returns>
        /// <response code="204">Success</response>
        /// <response code="404">Not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _userServices.DeleteUser(id);
            return NoContent();
        }

        /// <summary>
        /// Adds a supervisor to the user
        /// </summary>
        /// <param name="id">User identifier</param>
        /// <param name="supervisorId">Supervisor identifier</param>
        /// <returns></returns>
        /// <response code="204">Success</response>
        /// <response code="404">Not found</response>
        [HttpPost("{id}/supervisors/{supervisorId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> AddUserSupervisor(Guid id, Guid supervisorId)
        {
            await _userServices.AddUserSupervisor(id, supervisorId);
            return NoContent();
        }

        /// <summary>
        /// Removes a supervisor from the user
        /// </summary>
        /// <param name="id">User identifier</param>
        /// <param name="supervisorId">Supervisor identifier</param>
        /// <returns></returns>
        /// <response code="204">Success</response>
        /// <response code="404">Not found</response>
        [HttpDelete("{id}/supervisors/{supervisorId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> DeleteUserSupervisor(Guid id, Guid supervisorId)
        {
            await _userServices.DeleteUserSupervisor(id, supervisorId);
            return NoContent();
        }

        /// <summary>
        /// Get all supervisors from the user
        /// </summary>
        /// <param name="id">User identifier</param>
        /// <returns></returns>
        /// <response code="204">Success</response>
        /// <response code="404">Not found</response>
        [HttpGet("{id}/supervisors")]
        [ProducesResponseType(typeof(IEnumerable<UserViewModel>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> GetUserSupervisorsById(Guid id)
        {
            var user = await _userServices.GetUserSupervisorsById(id);
            return Ok(user);
        }
    }
}
