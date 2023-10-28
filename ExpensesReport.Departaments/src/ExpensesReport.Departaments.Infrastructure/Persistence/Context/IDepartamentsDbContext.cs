using ExpensesReport.Departaments.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpensesReport.Departaments.Infrastructure.Persistence.Context
{
    public interface IDepartamentsDbContext
    {
        DbSet<Departament> Departaments { get; set; }
    }
}
