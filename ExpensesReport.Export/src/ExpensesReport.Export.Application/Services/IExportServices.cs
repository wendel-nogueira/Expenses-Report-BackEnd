using ExpensesReport.Export.Application.ViewModels;

namespace ExpensesReport.Export.Application.Services
{
    public interface IExportServices
    {
        public string? Token { get; set; }

        Task<FileViewModel> exportUsersAndIdentites();
        Task<FileViewModel> exportDepartments();
        Task<FileViewModel> exportProjects();
        Task<FileViewModel> exportExpenseAccounts();
        Task<FileViewModel> exportExpenseReports();
        Task<FileViewModel> exportExpenseReport(string id);
    }
}
