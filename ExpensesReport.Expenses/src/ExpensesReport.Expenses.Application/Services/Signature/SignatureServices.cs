using ExpensesReport.Expenses.Application.Exceptions;
using ExpensesReport.Expenses.Application.InputModels.SignatureInputModel;
using ExpensesReport.Expenses.Application.Validators;
using ExpensesReport.Expenses.Application.ViewModels;
using ExpensesReport.Expenses.Core.Repositories;

namespace ExpensesReport.Expenses.Application.Services.Signature
{
    public class SignatureServices(ISignatureRepository signatureRepository, IExpenseReportRepository expenseReportRepository) : ISignatureServices
    {
        public async Task<IEnumerable<SignatureViewModel>> GetAllSignatures()
        {
            var signatures = await signatureRepository.GetAllAsync();
            return signatures.Select(SignatureViewModel.FromEntity);
        }

        public async Task<SignatureViewModel> GetSignatureById(string id)
        {
            var signature = await signatureRepository.GetByIdAsync(id) ?? throw new NotFoundException("Signature not found!");
            return SignatureViewModel.FromEntity(signature);
        }

        public async Task<IEnumerable<SignatureViewModel>> GetSignaturesByExpenseReportId(string expenseReportId)
        {
            var signatures = await signatureRepository.GetAllInExpenseReportAsync(expenseReportId);

            return signatures.Select(SignatureViewModel.FromEntity);
        }

        public async Task<SignatureViewModel> AddSignature(string expenseReportId, AddSignatureInputModel inputModel)
        {
            var errorsInput = InputModelValidator.Validate(inputModel);

            if (errorsInput?.Length > 0)
            {
                throw new BadRequestException("Error on create signature!", errorsInput);
            }

            var expenseReport = await expenseReportRepository.GetByIdAsync(expenseReportId) ?? throw new NotFoundException("Expense report not found!");
            var signature = inputModel.ToEntity();

            expenseReport.Signatures.Add(signature);

            await expenseReportRepository.UpdateAsync(expenseReport);

            return SignatureViewModel.FromEntity(signature);
        }
    }
}
