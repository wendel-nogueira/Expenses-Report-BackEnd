using Microsoft.AspNetCore.Identity;

namespace ExpensesReport.Identity.Core.Entities
{
    public class UserIdentity : IdentityUser
    {
        public UserIdentity()
        {
            Id = Guid.NewGuid().ToString();
            PasswordHash = null;
            ResetPasswordToken = null;

            IsDeleted = false;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public string? ResetPasswordToken { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public void Activate()
        {
            IsDeleted = false;
            UpdatedAt = DateTime.Now;
        }

        public void Deactivate()
        {
            IsDeleted = true;
            UpdatedAt = DateTime.Now;
        }

        public void Update()
        {
            UpdatedAt = DateTime.Now;
        }
    }
}
