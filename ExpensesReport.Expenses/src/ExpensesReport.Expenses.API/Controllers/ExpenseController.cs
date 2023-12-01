using ExpensesReport.Expenses.Application.InputModels.ExpenseAccountInputModel;
using ExpensesReport.Expenses.Application.InputModels.ExpenseInputModel;
using ExpensesReport.Expenses.Application.Services.Expense;
using ExpensesReport.Expenses.Application.ViewModels;
using ExpensesReport.Expenses.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesReport.Expenses.API.Controllers
{
    [ApiController]
    [Route("api/expense")]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseServices _expenseServices;

        public ExpenseController(IExpenseServices expenseServices)
        {
            _expenseServices = expenseServices;
        }

        /// <summary>
        /// Get all expenses
        /// </summary>
        /// <returns>Expense collection</returns>
        /// <response code="200">Success</response>
        //[Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ExpenseViewModel>), 200)]
        public async Task<IActionResult> GetAllExpenses()
        {
            var expenses = await _expenseServices.GetAllExpenses();

            return Ok(expenses);
        }

        /// <summary>
        /// Get a expense by id
        /// </summary>
        /// <param name="id">Expense identifier</param>
        /// <returns>Expense data</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        //[Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ExpenseViewModel), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> GetExpenseById(string id)
        {
            var expense = await _expenseServices.GetExpenseById(id);

            return Ok(expense);
        }

        /// <summary>
        /// Get a expenses by expense report id
        /// </summary>
        /// <param name="expenseReportId">Expense report identifier</param>
        /// <returns>Expense collection</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        //[Authorize]
        [HttpGet("expensereport/{expenseReportId}")]
        [ProducesResponseType(typeof(IEnumerable<ExpenseViewModel>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> GetExpensesByExpenseReportId(string expenseReportId)
        {
            var expenses = await _expenseServices.GetExpensesByExpenseReportId(expenseReportId);

            return Ok(expenses);
        }

        /// <summary>
        /// Register a expense
        /// </summary>
        /// <param name="expenseReportId">Expense report identifier</param>
        /// <param name="inputModel">Model with expense data</param>
        /// <returns>Newly created expense</returns>
        /// <response code="201">Created</response>
        /// <response code="400">Bad request</response>
        //[Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(ExpenseViewModel), 201)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        public async Task<IActionResult> AddExpense(string expenseReportId, AddExpenseInputModel inputModel)
        {
            var expense = await _expenseServices.AddExpense(expenseReportId, inputModel);

            return CreatedAtAction(nameof(GetExpenseById), new { id = expense.Id }, expense);
        }

        /// <summary>
        /// Update a expense
        /// </summary>
        /// <param name="id">Expense identifier</param>
        /// <param name="inputModel">Model with expense data</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not found</response>
        //[Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ExpenseViewModel), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> UpdateExpense(string id, ChangeExpenseInputModel inputModel)
        {
            var expense = await _expenseServices.UpdateExpense(id, inputModel);

            return Ok(expense);
        }

        /// <summary>
        /// Delete a expense
        /// </summary>
        /// <param name="id">Expense identifier</param>
        /// <returns></returns>
        /// <response code="204">Success</response>
        /// <response code="404">Not found</response>
        //[Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> DeleteExpense(string id)
        {
            await _expenseServices.DeleteExpense(id);

            return NoContent();
        }
    }
}
