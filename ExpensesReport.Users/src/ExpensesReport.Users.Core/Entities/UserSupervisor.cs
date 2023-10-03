namespace ExpensesReport.Users.Core.Entities
{
    public class UserSupervisor
    {
        [Obsolete("Parameterless constructor should not be used directly.")]
        private UserSupervisor() { }

        public UserSupervisor(Guid userId, Guid supervisorId, User user, User supervisor)
        {
            UserId = userId;
            SupervisorId = supervisorId;

            User = user;
            Supervisor = supervisor;
        }

        public Guid UserId { get; set; }
        public Guid SupervisorId { get; set; }

        public User User { get; set; }
        public User Supervisor { get; set; }
    }
}
