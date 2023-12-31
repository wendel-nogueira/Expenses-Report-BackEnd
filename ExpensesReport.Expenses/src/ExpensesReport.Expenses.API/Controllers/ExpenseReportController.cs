﻿using ExpensesReport.Expenses.Application.InputModels.ExpenseReportInputModel;
using ExpensesReport.Expenses.Application.Services.ExpenseReport;
using ExpensesReport.Expenses.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ExpensesReport.Expenses.API.Controllers
{
    [ApiController]
    [Route("api/expensereport")]
    public class ExpenseReportController : ControllerBase
    {
        private readonly IExpenseReportServices _expenseReportServices;

        public ExpenseReportController(IExpenseReportServices expenseReportServices)
        {
            _expenseReportServices = expenseReportServices;
        }

        /// <summary>
        /// Get all expense reports
        /// </summary>
        /// <returns>Expense reports collection</returns>
        /// <response code="200">Success</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ExpenseReportViewModel>), 200)]
        public async Task<IActionResult> GetAllExpenseReports()
        {
            var result = await _expenseReportServices.GetAllExpenseReports();
            return Ok(result);
        }

        /// <summary>
        /// Get a expense report by id
        /// </summary>
        /// <param name="id">Expense report identifier</param>
        /// <returns>Expense report data</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ExpenseReportViewModel), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> GetExpenseReportById(string id)
        {
            var result = await _expenseReportServices.GetExpenseReportById(id);
            return Ok(result);
        }

        /// <summary>
        /// Get a expense report by user
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Expense report data</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<ExpenseReportViewModel>), 200)]
        public async Task<IActionResult> GetExpenseReportsByUser(Guid userId)
        {
            var result = await _expenseReportServices.GetExpenseReportsByUser(userId);
            return Ok(result);
        }

        /// <summary>
        /// Get a expense report by departament
        /// </summary>
        /// <param name="departamentId">Departament identifier</param>
        /// <returns>Expense report data</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpGet("departament/{departamentId}")]
        [ProducesResponseType(typeof(IEnumerable<ExpenseReportViewModel>), 200)]
        public async Task<IActionResult> GetExpenseReportsByDepartament(Guid departamentId)
        {
            var result = await _expenseReportServices.GetExpenseReportsByDepartament(departamentId);
            return Ok(result);
        }

        /// <summary>
        /// Get a expense report by project
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        /// <returns>Expense report data</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpGet("project/{projectId}")]
        [ProducesResponseType(typeof(IEnumerable<ExpenseReportViewModel>), 200)]
        public async Task<IActionResult> GetExpenseReportsByProject(Guid projectId)
        {
            var result = await _expenseReportServices.GetExpenseReportsByProject(projectId);
            return Ok(result);
        }

        /// <summary>
        /// Register a expense report
        /// </summary>
        /// <param name="inputModel">Model with expense report data</param>
        /// <returns>Newly created expense report</returns>
        /// <response code="201">Created</response>
        /// <response code="400">Bad request</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(ExpenseReportViewModel), 201)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        public async Task<IActionResult> AddExpenseReport(AddExpenseReportInputModel inputModel)
        {
            if (!string.IsNullOrEmpty(Request.Headers["Authorization"])
                && Request.Headers["Authorization"].ToString().StartsWith("Bearer "))
            {
                _expenseReportServices.Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            }

            var result = await _expenseReportServices.AddExpenseReport(inputModel);
            return Ok(result);
        }

        /// <summary>
        /// Update a expense report
        /// </summary>
        /// <param name="id">Expense report identifier</param>
        /// <param name="inputModel">Model with expense report data</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ExpenseReportViewModel), 204)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> UpdateExpenseReport(string id, ChangeExpenseReportInputModel inputModel)
        {
            if (!string.IsNullOrEmpty(Request.Headers["Authorization"])
                && Request.Headers["Authorization"].ToString().StartsWith("Bearer "))
            {
                _expenseReportServices.Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            }

            var result = await _expenseReportServices.UpdateExpenseReport(id, inputModel);
            return Ok(result);
        }

        /// <summary>
        /// Activate a expense report
        /// </summary>
        /// <param name="id">Expense report identifier</param>
        /// <returns></returns>
        /// <response code="204">Success</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpPatch("{id}/activate")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> ActivateExpenseReport(string id)
        {
            await _expenseReportServices.ActivateExpenseReport(id);
            return Ok();
        }

        /// <summary>
        /// Deactivate a expense report
        /// </summary>
        /// <param name="id">Expense report identifier</param>
        /// <returns></returns>
        /// <response code="204">Success</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpDelete("{id}/deactivate")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> DeactivateExpenseReport(string id)
        {
            await _expenseReportServices.DeactivateExpenseReport(id);
            return Ok();
        }
    }
}
