using ExpensesReport.Expenses.Application.InputModels.ExpenseReportInputModel;
using ExpensesReport.Expenses.Application.ViewModels;

namespace ExpensesReport.Expenses.Application.Services.ExpenseReport
{
    public interface IExpenseReportServices
    {
        public Task<IEnumerable<ExpenseReportViewModel>> GetAllExpenseReports();
        public Task<ExpenseReportViewModel> GetExpenseReportById(string id);
        public Task<IEnumerable<ExpenseReportViewModel>> GetExpenseReportsByUser(Guid userId);
        public Task<IEnumerable<ExpenseReportViewModel>> GetExpenseReportsByDepartment(Guid departmentId);
        public Task<IEnumerable<ExpenseReportViewModel>> GetExpenseReportsByProject(Guid projectId);
        public Task<ExpenseReportViewModel> AddExpenseReport(AddExpenseReportInputModel inputModel);
        public Task<ExpenseReportViewModel> UpdateExpenseReport(string id, ChangeExpenseReportInputModel inputModel);
        public Task ActivateExpenseReport(string id);
        public Task DeactivateExpenseReport(string id);
    }
}