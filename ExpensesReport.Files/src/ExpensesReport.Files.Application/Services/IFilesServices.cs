using ExpensesReport.Files.Application.ViewModels;
using Microsoft.AspNetCore.Http;

namespace ExpensesReport.Files.Application.Services
{
    public interface IFilesServices
    {
        Task<IEnumerable<FileViewModel>> GetFiles();
        Task<FileViewModel> GetFileByName(string name);
        Task<FileViewModel> UploadFile(IFormFile file);
        Task DeleteFile(string name);
    }
}
