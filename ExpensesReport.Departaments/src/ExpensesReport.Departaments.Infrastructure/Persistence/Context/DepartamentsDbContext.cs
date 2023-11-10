using ExpensesReport.Departaments.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpensesReport.Departaments.Infrastructure.Persistence.Context
{
    public class DepartamentsDbContext : DbContext, IDepartamentsDbContext
    {
        public DbSet<Departament> Departaments { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<User> Users { get; set; }

        public DepartamentsDbContext(DbContextOptions<DepartamentsDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Departament>(entity =>
            {
                entity.HasKey(departament => departament.Id);

                entity.Property(departament => departament.Name)
                    .HasColumnName("Name")
                    .IsRequired()
                    .HasAnnotation("MaxLength", 50)
                    .HasColumnType("varchar(50)");

                entity.Property(departament => departament.Acronym)
                    .HasColumnName("Acronym")
                    .IsRequired()
                    .HasAnnotation("MaxLength", 10)
                    .HasColumnType("varchar(10)");

                entity.Property(departament => departament.Description)
                    .HasColumnName("Description")
                    .HasAnnotation("MaxLength", 200)
                    .HasColumnType("varchar(200)");

                entity.Property(user => user.IsDeleted)
                    .HasColumnName("IsDeleted")
                    .IsRequired();

                entity.Property(user => user.CreatedAt)
                    .HasColumnName("CreatedAt")
                    .IsRequired();

                entity.Property(user => user.UpdatedAt)
                    .HasColumnName("UpdatedAt")
                    .IsRequired();

                entity.HasIndex(departament => departament.Acronym)
                    .IsUnique();
            });

            builder.Entity<Manager>(entity =>
            {
                entity.HasKey(manager => manager.Id);

                entity.Property(manager => manager.ManagerId)
                    .HasColumnName("ManagerId")
                    .IsRequired();

                entity.HasOne(manager => manager.Departament)
                            .WithMany(departament => departament.Managers)
                            .HasForeignKey(manager => manager.DepartamentId)
                            .HasConstraintName("FK_Manager_Departament")
                            .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<User>(entity =>
            {
                entity.HasKey(user => user.Id);

                entity.Property(user => user.UserId)
                    .HasColumnName("UserId")
                    .IsRequired();

                entity.HasOne(user => user.Departament)
                    .WithMany(departament => departament.Users)
                    .HasForeignKey(manager => manager.DepartamentId)
                    .HasConstraintName("FK_User_Departament")
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
