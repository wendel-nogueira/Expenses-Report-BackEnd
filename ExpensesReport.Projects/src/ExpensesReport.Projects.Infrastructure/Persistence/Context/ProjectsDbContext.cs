using ExpensesReport.Projects.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpensesReport.Projects.Infrastructure.Persistence.Context
{
    public class ProjectsDbContext : DbContext, IProjectsDbContext
    {
        public DbSet<Project> Projects { get; set; }

        public ProjectsDbContext(DbContextOptions<ProjectsDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Project>(entity =>
            {
                entity.HasKey(project => project.Id);

                entity.Property(project => project.Name)
                    .HasColumnName("Name")
                    .IsRequired()
                    .HasAnnotation("MaxLength", 50)
                    .HasColumnType("varchar(50)");

                entity.Property(project => project.Code)
                    .HasColumnName("Code")
                    .IsRequired()
                    .HasAnnotation("MaxLength", 10)
                    .HasColumnType("varchar(10)");

                entity.Property(project => project.Description)
                    .HasColumnName("Description")
                    .HasAnnotation("MaxLength", 200)
                    .HasColumnType("varchar(200)");

                entity.Property(project => project.DepartamentId)
                    .HasColumnName("DepartamentId")
                    .IsRequired();

                entity.Property(project => project.IsDeleted)
                    .HasColumnName("IsDeleted")
                    .IsRequired();

                entity.Property(project => project.CreatedAt)
                    .HasColumnName("CreatedAt")
                    .IsRequired();

                entity.Property(project => project.UpdatedAt)
                    .HasColumnName("UpdatedAt")
                    .IsRequired();
            });
        }
    }
}
