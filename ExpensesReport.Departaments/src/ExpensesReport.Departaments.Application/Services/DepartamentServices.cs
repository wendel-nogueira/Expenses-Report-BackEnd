using ExpensesReport.Departaments.Application.InputModels;
using ExpensesReport.Departaments.Application.ViewModels;
using ExpensesReport.Departaments.Core.Repositories;
using ExpensesReport.Departaments.Application.Exceptions;
using ExpensesReport.Departaments.Application.Validators;

namespace ExpensesReport.Departaments.Application.Services
{
    public class DepartamentServices(IDepartamentRepository departamentRepository) : IDepartamentServices
    {
        private readonly IDepartamentRepository _departamentRepository = departamentRepository;

        public async Task<DepartamentViewModel> GetDepartamentById(Guid id)
        {
            var departament = await _departamentRepository.GetByIdAsync(id) ?? throw new NotFoundException("Departament not found!");
            return DepartamentViewModel.FromEntity(departament);
        }

        public async Task<DepartamentViewModel> GetDepartamentByName(string name)
        {
            var departament = await _departamentRepository.GetByNameAsync(name) ?? throw new NotFoundException("Departament not found!");
            return DepartamentViewModel.FromEntity(departament);
        }

        public async Task<DepartamentViewModel> GetDepartamentByAcronym(string acronym)
        {
            var departament = await _departamentRepository.GetByAcronymAsync(acronym) ?? throw new NotFoundException("Departament not found!");
            return DepartamentViewModel.FromEntity(departament);
        }

        public async Task<IEnumerable<DepartamentViewModel>> GetAllDepartaments()
        {
            var departaments = await _departamentRepository.GetAllAsync();
            return departaments.Select(DepartamentViewModel.FromEntity);
        }

        public async Task<DepartamentViewModel> AddDepartament(AddDepartamentInputModel inputModel)
        {
            var errorsInput = InputModelValidator.Validate(inputModel);

            if (errorsInput?.Length > 0)
            {
                throw new BadRequestException("Error on create departament!", errorsInput);
            }

            var departamentExists = await _departamentRepository.GetByAcronymAsync(inputModel.Acronym!);

            if (departamentExists != null)
            {
                throw new BadRequestException("Departament already exists!", []);
            }

            var departament = inputModel.ToEntity();

            await _departamentRepository.AddAsync(departament);

            return DepartamentViewModel.FromEntity(departament);
        }

        public async Task UpdateDepartament(Guid id, ChangeDepartamentInputModel inputModel)
        {
            var errorsInput = InputModelValidator.Validate(inputModel);

            if (errorsInput?.Length > 0)
            {
                throw new BadRequestException("Error on update departament!", errorsInput);
            }

            var departament = await _departamentRepository.GetByIdAsync(id) ?? throw new NotFoundException("Departament not found!");

            var rowVersion = BitConverter.GetBytes(DateTime.UtcNow.Ticks);
            departament.Update(inputModel.Name!, inputModel.Acronym!, inputModel.Description!, rowVersion);

            await _departamentRepository.UpdateAsync(departament);
        }

        public async Task DeactivateDepartament(Guid id)
        {
            var departament = await _departamentRepository.GetByIdAsync(id) ?? throw new NotFoundException("Departament not found!");

            await _departamentRepository.DeactivateAsync(departament);
        }

        public async Task ActivateDepartament(Guid id)
        {
            var departament = await _departamentRepository.GetByIdAsync(id) ?? throw new NotFoundException("Departament not found!");

            departament.Activate();

            await _departamentRepository.UpdateAsync(departament);
        }

        public async Task<ManagerViewModel> GetDepartamentManagers(Guid departamentId)
        {
            _ = await _departamentRepository.GetByIdAsync(departamentId) ?? throw new NotFoundException("Departament not found!");
            var managers = await _departamentRepository.GetAllManagersAsync(departamentId);

            return ManagerViewModel.FromEntity(managers);
        }

        public async Task AddDepartamentManager(Guid departamentId, Guid managerId)
        {
            var departament = await _departamentRepository.GetByIdAsync(departamentId) ?? throw new NotFoundException("Departament not found!");
            var departamentManagers = await _departamentRepository.GetAllManagersAsync(departamentId);

            if (departamentManagers.Any(x => x.ManagerId == managerId))
            {
                throw new BadRequestException("Manager already exists in departament!", []);
            }

            await _departamentRepository.AddManagerAsync(departamentId, managerId);
        }

        public async Task RemoveDepartamentManager(Guid departamentId, Guid managerId)
        {
            var departament = await _departamentRepository.GetByIdAsync(departamentId) ?? throw new NotFoundException("Departament not found!");
            var departamentManagers = await _departamentRepository.GetAllManagersAsync(departamentId);

            if (!departamentManagers.Any(x => x.ManagerId == managerId))
            {
                throw new NotFoundException("Manager not found!");
            }

            await _departamentRepository.RemoveManagerAsync(departamentId, managerId);
        }

        public async Task<UserViewModel> GetDepartamentUsers(Guid departamentId)
        {
            _ = await _departamentRepository.GetByIdAsync(departamentId) ?? throw new NotFoundException("Departament not found!");
            var users = await _departamentRepository.GetAllUsersAsync(departamentId);

            return UserViewModel.FromEntity(users);
        }

        public async Task AddDepartamentUser(Guid departamentId, Guid userId)
        {
            var departament = await _departamentRepository.GetByIdAsync(departamentId) ?? throw new NotFoundException("Departament not found!");
            var departamentUsers = await _departamentRepository.GetAllUsersAsync(departamentId);

            if (departamentUsers.Any(x => x.UserId == userId))
            {
                throw new BadRequestException("User already exists in departament!", []);
            }

            await _departamentRepository.AddUserAsync(departamentId, userId);
        }

        public async Task RemoveDepartamentUser(Guid departamentId, Guid userId)
        {
            var departament = await _departamentRepository.GetByIdAsync(departamentId) ?? throw new NotFoundException("Departament not found!");
            var departamentUsers = await _departamentRepository.GetAllUsersAsync(departamentId);

            if (!departamentUsers.Any(x => x.UserId == userId))
            {
                throw new NotFoundException("User not found!");
            }

            await _departamentRepository.RemoveUserAsync(departamentId, userId);
        }

        public async Task<IEnumerable<DepartamentViewModel>> GetDepartamentsByManagerAndUser(Guid id)
        {
            var departaments = await _departamentRepository.GetDepartamentsByManagerAndUser(id);

            return departaments.Select(DepartamentViewModel.FromEntity);
        }
    }
}
