namespace ExpensesReport.Users.Core.Entities
{
    public class UserSupervisor
    {
        [Obsolete("Parameterless constructor should not be used directly.")]
        private UserSupervisor() { }

        public UserSupervisor(Guid userId, Guid supervisorId)
        {
            UserId = userId;
            SupervisorId = supervisorId;

            User = null!;
            Supervisor = null!;
        }

        public Guid UserId { get; set; }
        public Guid SupervisorId { get; set; }

        public User User { get; set; }
        public User Supervisor { get; set; }
    }
}
