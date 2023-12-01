using ExpensesReport.Expenses.Application.InputModels.SignatureInputModel;
using ExpensesReport.Expenses.Application.Services.Signature;
using ExpensesReport.Expenses.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesReport.Expenses.API.Controllers
{
    [ApiController]
    [Route("api/signature")]
    public class SignatureController : ControllerBase
    {
        private readonly ISignatureServices _signatureServices;

        public SignatureController(ISignatureServices signatureServices)
        {
            _signatureServices = signatureServices;
        }

        /// <summary>
        /// Get all signature
        /// </summary>
        /// <returns>Signature collection</returns>
        /// <response code="200">Success</response>
        //[Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SignatureViewModel>), 200)]
        public async Task<IActionResult> GetAllSignature()
        {
            var signatures = await _signatureServices.GetAllSignatures();

            return Ok(signatures);
        }

        /// <summary>
        /// Get a signature by id
        /// </summary>
        /// <param name="id">Signature identifier</param>
        /// <returns>Signature data</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        //[Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SignatureViewModel), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> GetSignatureById(string id)
        {
            var signature = await _signatureServices.GetSignatureById(id);

            return Ok(signature);
        }

        /// <summary>
        /// Get signatures by expense report id
        /// </summary>
        /// <param name="expenseReportId">Expense Report identifier</param>
        /// <returns>Signature data</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        //[Authorize]
        [HttpGet("expenseReport/{expenseReportId}")]
        [ProducesResponseType(typeof(IEnumerable<SignatureViewModel>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> GetSignatureByExpenseReportId(string expenseReportId)
        {
            var signature = await _signatureServices.GetSignaturesByExpenseReportId(expenseReportId);

            return Ok(signature);
        }

        /// <summary>
        /// Register a new signature
        /// </summary>
        /// <param name="expenseReportId">Expense Report identifier</param>
        /// <param name="inputModel">Signature data</param>
        /// <returns>Newly created signature</returns>
        /// <response code="201">Created</response>
        /// <response code="400">Bad request</response>
        //[Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(SignatureViewModel), 201)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        public async Task<IActionResult> CreateSignature(string expenseReportId, AddSignatureInputModel inputModel)
        {
            var signature = await _signatureServices.AddSignature(expenseReportId, inputModel);

            return CreatedAtAction(nameof(GetSignatureById), new { id = signature.Id }, signature);
        }
    }
}
