using ExpensesReport.Expenses.Application.InputModels.SignatureInputModel;
using ExpensesReport.Expenses.Application.ViewModels;

namespace ExpensesReport.Expenses.Application.Services.Signature
{
    public interface ISignatureServices
    {
        public Task<IEnumerable<SignatureViewModel>> GetAllSignatures();
        public Task<SignatureViewModel> GetSignatureById(string id);
        public Task<IEnumerable<SignatureViewModel>> GetSignaturesByExpenseReportId(string expenseReportId);
        public Task<SignatureViewModel> AddSignature(string expenseReportId, AddSignatureInputModel inputModel);
    }
}
