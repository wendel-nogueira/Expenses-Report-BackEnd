using ExpensesReport.Users.Core.ValueObjects;

namespace ExpensesReport.Users.Core.Entities
{
    public class User : EntityBase
    {
        [Obsolete("Parameterless constructor should not be used directly.")]
        private User() { }

        public User(Guid identityId, UserName name, UserAddress userAddress)
        {
            IdentityId = identityId;
            Name = name;
            Address = userAddress;

            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;

            Supervisors = new List<UserSupervisor>();
        }

        public Guid IdentityId { get; set; }

        public UserName Name { get; set; }

        public UserAddress Address { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<UserSupervisor> Supervisors { get; set; }

        public void Update(UserName name, UserAddress address)
        {
            Name = name;
            Address = address;
            UpdatedAt = DateTime.Now;
        }

        public void AddSupervisorToUser(User supervisor)
        {
            var userSupervisor = new UserSupervisor(Id, supervisor.Id, this, supervisor);

            Supervisors.Add(userSupervisor);
        }

        public void RemoveSupervisorFromUser(User supervisor)
        {
            var userSupervisor = Supervisors.SingleOrDefault(x => x.SupervisorId == supervisor.Id);

            if (userSupervisor != null)
                Supervisors.Remove(userSupervisor);
        }
    }
}
