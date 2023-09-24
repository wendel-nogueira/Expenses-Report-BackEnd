namespace ExpensesReport.Users.Core.Entities
{
    public class UserSupervisor : EntityBase
    {
        public Guid UserId { get; set; }
        public Guid SupervisorId { get; set; }

        public UserSupervisor(Guid userId, Guid supervisorId)
        {
            UserId = userId;
            SupervisorId = supervisorId;
        }
    }
}
