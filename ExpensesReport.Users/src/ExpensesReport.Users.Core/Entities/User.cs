using ExpensesReport.Users.Core.Enums;
using ExpensesReport.Users.Core.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace ExpensesReport.Users.Core.Entities
{
    public class User : EntityBase
    {
        public User(UserName name, UserRole role, string email, UserAddress userAddress)
        {
            Name = name;
            Role = role;
            Email = email;
            Password = GeneratePassword();
            Address = userAddress;

            IsDeleted = false;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Supervisors = new List<Guid>();
        }

        public UserName Name { get; set; }

        [Required(ErrorMessage = "Role is required")]
        [EnumDataType(typeof(UserRole), ErrorMessage = "Role must be a valid value")]
        public UserRole Role { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email must be a valid email")]
        public string Email { get; set; }

        public string Password { get; set; }

        public UserAddress Address { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public List<Guid> Supervisors { get; set; }

        public void Delete()
        {
            IsDeleted = true;
            UpdatedAt = DateTime.Now;
        }

        public void Update(User user)
        {
            Name = user.Name;
            Role = user.Role;
            Email = user.Email;
            Address = user.Address;
            UpdatedAt = DateTime.Now;
        }

        public void AddSupervisorToUser(Guid supervisorId)
        {
            if (Supervisors.Contains(supervisorId))
                throw new Exception("Supervisor already exists");

            Supervisors.Add(supervisorId);
        }

        public void RemoveSupervisorFromUser(Guid supervisorId)
        {
            if (!Supervisors.Contains(supervisorId))
                throw new Exception("Supervisor does not exists");

            Supervisors.Remove(supervisorId);
        }

        public static string GeneratePassword()
        {
            string _allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
            Random randNum = new();
            char[] chars = new char[8];

            for (int i = 0; i < 8; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }

            return new string(chars);
        }
    }
}
