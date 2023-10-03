using Microsoft.AspNetCore.Identity;

namespace ExpensesReport.Identity.Application.ViewModels
{
    public class AuthenticationViewModel(string token)
    {
        public string Token { get; set; } = token;

        public static AuthenticationViewModel FromEntity(string token)
        {
            return new AuthenticationViewModel(token);
        }
    }
}
