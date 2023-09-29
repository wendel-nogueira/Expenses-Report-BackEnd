using Microsoft.AspNetCore.Identity;

namespace ExpensesReport.Identity.Core.Entities
{
    public class UserIdentity : IdentityUser
    {
        public UserIdentity()
        {
            Id = Guid.NewGuid().ToString();
            PasswordHash = null;

            IsDeleted = false;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public void Delete()
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
