using ExpensesReport.Projects.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpensesReport.Projects.Infrastructure.Persistence.Context
{
    public interface IProjectsDbContext
    {
        DbSet<Project> Projects { get; set; }
    }
}
