namespace ExpensesReport.Departaments.Core.Entities
{
    public class Manager
    {
        [Obsolete("Parameterless constructor should not be used directly.")]

        public Manager() { }

        public Manager(Guid managerId)
        {
            ManagerId = managerId;
        }

        public Manager(Guid managerId, Departament departament)
        {
            Id = Guid.NewGuid();
            ManagerId = managerId;
            Departament = departament;
        }

        public Guid Id { get; set; }
        public Guid ManagerId { get; set; }
        public Departament Departament { get; set; }
        public Guid DepartamentId { get; set; }
    }
}
