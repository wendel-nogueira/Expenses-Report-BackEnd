using ExpensesReport.Projects.Application.InputModels;
using ExpensesReport.Projects.Application.ViewModels;
using ExpensesReport.Projects.Core.Repositories;
using ExpensesReport.Projects.Application.Exceptions;
using ExpensesReport.Projects.Application.Validators;

namespace ExpensesReport.Projects.Application.Services
{
    public class ProjectServices(IProjectRepository projectRepository) : IProjectServices
    {
        private readonly IProjectRepository _projectRepository = projectRepository;

        public async Task<IEnumerable<ProjectViewModel>> GetAllProjects()
        {
            var projects = await _projectRepository.GetAllAsync();
            return projects.Select(ProjectViewModel.FromEntity);
        }

        public async Task<ProjectViewModel?> GetProjectById(Guid id)
        {
            var project = await _projectRepository.GetByIdAsync(id) ?? throw new NotFoundException("Project not found!");

            return ProjectViewModel.FromEntity(project);
        }

        public async Task<ProjectViewModel?> GetProjectByCode(string code)
        {
            var project = await _projectRepository.GetByCodeAsync(code) ?? throw new NotFoundException("Project not found!");

            return ProjectViewModel.FromEntity(project);
        }

        public async Task<IEnumerable<ProjectViewModel>> GetProjectsByDepartamentId(Guid departamentId)
        {
            var projects = await _projectRepository.GetProjectsByDepartamentIdAsync(departamentId);
            return projects.Select(ProjectViewModel.FromEntity);
        }

        public async Task<ProjectViewModel> AddProject(AddProjectInputModel inputModel)
        {
            var errorsInput = InputModelValidator.Validate(inputModel);

            if (errorsInput?.Length > 0)
            {
                throw new BadRequestException("Error on create project!", errorsInput);
            }

            var projectExists = await _projectRepository.GetByCodeAsync(inputModel.Code!);

            if (projectExists != null)
            {
                throw new BadRequestException("Project already exists!", []);
            }

            var project = inputModel.ToEntity();

            await _projectRepository.AddAsync(project);

            return ProjectViewModel.FromEntity(project);
        }

        public async Task UpdateProject(Guid id, ChangeProjectInputModel inputModel)
        {
            var errorsInput = InputModelValidator.Validate(inputModel);

            if (errorsInput?.Length > 0)
            {
                throw new BadRequestException("Error on update project!", errorsInput);
            }

            var project = await _projectRepository.GetByIdAsync(id) ?? throw new NotFoundException("Project not found!");

            project.Update(inputModel.Name!, inputModel.Code!, inputModel.Description!, inputModel.DepartamentId);

            await _projectRepository.UpdateAsync(project);
        }

        public async Task DeactivateProject(Guid id)
        {
            var project = await _projectRepository.GetByIdAsync(id) ?? throw new NotFoundException("Project not found!");

            await _projectRepository.DeactivateAsync(project);
        }

        public async Task ActivateProject(Guid id)
        {
            var project = await _projectRepository.GetByIdAsync(id) ?? throw new NotFoundException("Project not found!");

            project.Activate();

            await _projectRepository.UpdateAsync(project);
        }
    }
}
