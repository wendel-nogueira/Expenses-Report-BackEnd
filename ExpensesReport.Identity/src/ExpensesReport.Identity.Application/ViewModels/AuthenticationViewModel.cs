using Microsoft.AspNetCore.Identity;

namespace ExpensesReport.Identity.Application.ViewModels
{
    public class AuthenticationViewModel
    {
        public AuthenticationViewModel(string token, string userName, string email, string role)
        {
            Token = token;
            UserName = userName;
            Email = email;
            Role = role;
        }

        public string Token { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public static AuthenticationViewModel FromEntity(IdentityUser user, string role, string token)
        {
            return new AuthenticationViewModel(token, user.UserName!, user.Email!, role);
        }
    }
}
