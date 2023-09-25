using ExpensesReport.Users.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpensesReport.Users.Infrastructure.Persistence
{
    public class UsersDbContext : DbContext, IUsersDbContext
    {
        public DbSet<User> Users { get; set; }

        public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(entity =>
            {
                entity.HasKey(user => user.Id);

                entity.OwnsOne(user => user.Name, name =>
                {
                    name.Property(name => name.FirstName)
                        .HasColumnName("FirstName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50)
                        .HasColumnType("varchar(50)");

                    name.Property(name => name.LastName)
                        .HasColumnName("LastName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50)
                        .HasColumnType("varchar(50)");
                });

                entity.Property(user => user.Role)
                    .HasColumnName("Role")
                    .IsRequired()
                    .HasConversion<string>();

                entity.Property(user => user.Email)
                    .HasColumnName("Email")
                    .IsRequired()
                    .HasAnnotation("MaxLength", 100)
                    .HasColumnType("varchar(100)");

                entity.HasIndex(user => user.Email).IsUnique();

                entity.Property(user => user.Password)
                    .HasColumnName("Password")
                    .IsRequired()
                    .HasAnnotation("MaxLength", 100)
                    .HasColumnType("varchar(100)");

                entity.OwnsOne(user => user.Address, addressUser =>
                {
                    addressUser.Property(address => address.Address)
                        .HasColumnName("Address")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100)
                        .HasColumnType("varchar(100)");

                    addressUser.Property(address => address.City)
                        .HasColumnName("City")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50)
                        .HasColumnType("varchar(50)");

                    addressUser.Property(address => address.State)
                        .HasColumnName("State")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50)
                        .HasColumnType("varchar(50)");

                    addressUser.Property(address => address.Zip)
                        .HasColumnName("Zip")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 10)
                        .HasColumnType("varchar(10)");

                    addressUser.Property(address => address.Country)
                        .HasColumnName("Country")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50)
                        .HasColumnType("varchar(50)");
                });

                entity.Property(user => user.IsDeleted)
                    .HasColumnName("IsDeleted")
                    .IsRequired();

                entity.Property(user => user.CreatedAt)
                    .HasColumnName("CreatedAt")
                    .IsRequired();

                entity.Property(user => user.UpdatedAt)
                    .HasColumnName("UpdatedAt")
                    .IsRequired();

            });

            builder.Entity<UserSupervisor>(entity =>
                    {
                        entity.HasKey(userSupervisor => new { userSupervisor.UserId, userSupervisor.SupervisorId });

                        entity.Property(userSupervisor => userSupervisor.UserId)
                            .HasColumnName("UserId")
                            .IsRequired();

                        entity.Property(userSupervisor => userSupervisor.SupervisorId)
                            .HasColumnName("SupervisorId")
                            .IsRequired();

                        entity.HasOne(userSupervisor => userSupervisor.User)
                            .WithMany(user => user.Supervisors)
                            .HasForeignKey(userSupervisor => userSupervisor.UserId)
                            .HasConstraintName("FK_UserSupervisor_User")
                            .OnDelete(DeleteBehavior.Cascade);

                        entity.HasOne(userSupervisor => userSupervisor.Supervisor)
                            .WithMany()
                            .HasForeignKey(userSupervisor => userSupervisor.SupervisorId)
                            .HasConstraintName("FK_UserSupervisor_Supervisor")
                            .OnDelete(DeleteBehavior.Cascade);
                    });
        }
    }
}
