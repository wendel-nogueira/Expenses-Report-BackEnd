namespace ExpensesReport.Departaments.Core.Entities
{
    public class User
    {
        [Obsolete("Parameterless constructor should not be used directly.")]

        public User() { }

        public User(Guid userId, Departament departament)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Departament = departament;
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Departament Departament { get; set; }
        public Guid DepartamentId { get; set; }
    }
}
