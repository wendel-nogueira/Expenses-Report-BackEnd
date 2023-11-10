namespace ExpensesReport.Departaments.Core.Entities
{
    public class Departament : EntityBase
    {
        [Obsolete("Parameterless constructor should not be used directly.")]

        public Departament() { }

        public Departament(string name, string acronym, string description)
        {
            Name = name;
            Acronym = acronym;
            Description = description;
            IsDeleted = false;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;

            Managers = new List<Manager>();
            Users = new List<User>();
        }

        public string Name { get; set; }
        public string Acronym { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; private set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual ICollection<Manager> Managers { get; set; }
        public virtual ICollection<User> Users { get; }

        public void Update(string name, string acronym, string description, byte[] rowVersion)
        {
            Name = name ?? Name;
            Acronym = acronym ?? Acronym;
            Description = description ?? Description;
            UpdatedAt = DateTime.Now;
        }

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
    }
}
