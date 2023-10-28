using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.Data;

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

            RowVersion = BitConverter.GetBytes(DateTime.UtcNow.Ticks);
        }

        public string Name { get; set; }
        public string Acronym { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; private set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual ICollection<Manager> Managers { get; set; }
        public virtual ICollection<User> Users { get; }

        [Timestamp]
        public byte[] RowVersion { get; set; }


        public void Update(string name, string acronym, string description, byte[] rowVersion)
        {
            if (!rowVersion.SequenceEqual(RowVersion))
            {
                throw new DBConcurrencyException("Concurrency violation");
            }

            Name = name ?? Name;
            Acronym = acronym ?? Acronym;
            Description = description ?? Description;
            UpdatedAt = DateTime.Now;
            RowVersion = BitConverter.GetBytes(DateTime.UtcNow.Ticks);
        }

        public void Activate()
        {
            IsDeleted = false;
            UpdatedAt = DateTime.Now;
        }

        public void Delete()
        {
            IsDeleted = true;
            UpdatedAt = DateTime.Now;
        }
    }
}
