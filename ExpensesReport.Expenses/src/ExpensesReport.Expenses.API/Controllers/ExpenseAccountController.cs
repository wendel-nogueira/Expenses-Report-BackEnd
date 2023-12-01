using ExpensesReport.Expenses.Application.ViewModels;
using ExpensesReport.Expenses.Application.Services.ExpenseAccount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ExpensesReport.Expenses.Application.InputModels.ExpenseAccountInputModel;

namespace ExpensesReport.Expenses.API.Controllers
{
    [ApiController]
    [Route("api/expenseaccount")]
    public class ExpenseAccountController : ControllerBase
    {
        private readonly IExpenseAccountServices _expensesAccountServices;

        public ExpenseAccountController(IExpenseAccountServices expensesAccountServices)
        {
            _expensesAccountServices = expensesAccountServices;
        }

        /// <summary>
        /// Get all expense accounts
        /// </summary>
        /// <returns>Expense account collection</returns>
        /// <response code="200">Success</response>
        //[Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ExpenseAccountViewModel>), 200)]
        public async Task<IActionResult> GetAllExpenseAccounts()
        {
            var expenseAccounts = await _expensesAccountServices.GetAllExpenseAccounts();

            return Ok(expenseAccounts);
        }

        /// <summary>
        /// Get a expense account by id
        /// </summary>
        /// <param name="id">Expense account identifier</param>
        /// <returns>Expense account data</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        //[Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ExpenseAccountViewModel), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> GetExpenseAccountById(string id)
        {
            var expenseAccount = await _expensesAccountServices.GetExpenseAccountById(id);

            return Ok(expenseAccount);
        }

        /// <summary>
        /// Get a expense account by code
        /// </summary>
        /// <param name="code">Expense account code</param>
        /// <returns>Expense account data</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        //[Authorize]
        [HttpGet("code/{code}")]
        [ProducesResponseType(typeof(ExpenseAccountViewModel), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> GetExpenseAccountByCode(string code)
        {
            var expenseAccount = await _expensesAccountServices.GetExpenseAccountByCode(code);

            return Ok(expenseAccount);
        }

        /// <summary>
        /// Register a expense account
        /// </summary>
        /// <param name="inputModel">Model with expense account data</param>
        /// <returns>Newly created expense account</returns>
        /// <response code="201">Created</response>
        /// <response code="400">Bad request</response>
        //[Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(ExpenseAccountViewModel), 201)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        public async Task<IActionResult> AddExpenseAccount(AddExpenseAccountInputModel inputModel)
        {
            var expenseAccount = await _expensesAccountServices.AddExpenseAccount(inputModel);

            return CreatedAtAction(nameof(GetExpenseAccountById), new { id = expenseAccount.Id }, expenseAccount);
        }

        /// <summary>
        /// Update a expense account
        /// </summary>
        /// <param name="id">Expense account identifier</param>
        /// <param name="inputModel">Model with expense account data</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not found</response>
        //[Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ExpenseAccountViewModel), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> UpdateExpenseAccount(string id, ChangeExpenseAccountInputModel inputModel)
        {
            var expenseAccount = await _expensesAccountServices.UpdateExpenseAccount(id, inputModel);

            return Ok(expenseAccount);
        }

        /// <summary>
        /// Activate a expense account
        /// </summary>
        /// <param name="id">Expense account identifier</param>
        /// <returns></returns>
        /// <response code="204">Success</response>
        /// <response code="404">Not found</response>
        //[Authorize]
        [HttpPatch("{id}/activate")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> ActivateExpenseAccount(string id)
        {
            await _expensesAccountServices.ActivateExpenseAccount(id);

            return NoContent();
        }

        /// <summary>
        /// Deactivate a expense account
        /// </summary>
        /// <param name="id">Expense account identifier</param>
        /// <returns></returns>
        /// <response code="204">Success</response>
        /// <response code="404">Not found</response>
        //[Authorize]
        [HttpDelete("{id}/deactivate")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> DeactivateExpenseAccount(string id)
        {
            await _expensesAccountServices.DeactivateExpenseAccount(id);

            return NoContent();
        }
    }
}
