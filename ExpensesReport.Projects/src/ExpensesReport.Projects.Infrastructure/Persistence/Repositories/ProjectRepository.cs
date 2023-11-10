using ExpensesReport.Projects.Core.Entities;
using ExpensesReport.Projects.Core.Repositories;
using ExpensesReport.Projects.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace ExpensesReport.Projects.Infrastructure.Persistence.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ProjectsDbContext _context;

        public ProjectRepository(ProjectsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _context.Projects
                .ToListAsync();
        }

        public async Task<Project?> GetByIdAsync(Guid id)
        {
            return await _context.Projects
                .FirstOrDefaultAsync(project => project.Id == id);
        }

        public async Task<Project?> GetByCodeAsync(string code)
        {
            return await _context.Projects
                .FirstOrDefaultAsync(project => project.Code == code);
        }

        public async Task<IEnumerable<Project>> GetProjectsByDepartamentIdAsync(Guid departmentId)
        {
            return await _context.Projects
                .Where(project => project.DepartamentId == departmentId)
                .ToListAsync();
        }

        public async Task<Project> AddAsync(Project project)
        {
            var entry = await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<Project> UpdateAsync(Project project)
        {
            var entry = _context.Projects.Update(project);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task ActivateAsync(Project project)
        {
            project.Activate();
            _context.Projects.Update(project);
            await UpdateAsync(project);
        }

        public async Task DeactivateAsync(Project project)
        {
            project.Deactivate();
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }
    }
}
