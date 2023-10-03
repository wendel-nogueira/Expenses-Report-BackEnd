using ExpensesReport.Users.Application.InputModels;
using ExpensesReport.Users.Application.ViewModels;
using ExpensesReport.Users.Application.Validators;
using ExpensesReport.Users.Application.Exceptions;
using ExpensesReport.Users.Core.Repositories;
using ExpensesReport.Users.Core.Entities;

namespace ExpensesReport.Users.Application.Services
{
    public class UserServices(IUserRepository userRepository) : IUserServices
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<UserViewModel> GetUserById(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user != null)
                return UserViewModel.FromEntity(user);

            throw new NotFoundException("User not found!");
        }

        public async Task<UserViewModel> GetUserByIdentityId(Guid id)
        {
            var user = await _userRepository.GetByIdentityIdAsync(id);

            if (user != null)
                return UserViewModel.FromEntity(user);

            throw new NotFoundException("User not found!");
        }

        public async Task<IEnumerable<UserViewModel>> GetUsers()
        {
            var users = await _userRepository.GetAllAsync();

            return users.Select(UserViewModel.FromEntity);
        }

        public async Task<UserViewModel> AddUser(AddUserInputModel inputModel)
        {
            var errors = InputModelValidator.Validate(inputModel);

            if (errors != null && errors.Length > 0)
                throw new BadRequestException("User data is required! Check that all fields have been filled in correctly.", errors);

            var user = inputModel.ToEntity();
            await _userRepository.AddAsync(user);

            return UserViewModel.FromEntity(user);
        }

        public async Task<UserViewModel> UpdateUser(Guid id, UpdateUserInputModel inputModelToUpdate)
        {
            var errors = InputModelValidator.Validate(inputModelToUpdate);

            if (errors != null && errors.Length > 0)
                throw new BadRequestException("User data is required! Check that all fields have been filled in correctly.", errors);

            var user = await _userRepository.GetByIdAsync(id);

            if (user != null)
            {
                var newName = inputModelToUpdate.Name == null ? user.Name : inputModelToUpdate.Name!.ToValueObject();
                var newAddress = inputModelToUpdate.Address == null ? user.Address : inputModelToUpdate.Address!.ToValueObject();

                await _userRepository.UpdateAsync(id, newName, newAddress);

                return UserViewModel.FromEntity(user);
            }

            throw new NotFoundException("User not found!");
        }

        public async Task<IEnumerable<UserViewModel>> GetUserSupervisorsById(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user != null)
            {
                var userSupervisors = await _userRepository.GetUserSupervisorsByIdAsync(id);

                return userSupervisors.Select(UserViewModel.FromEntity);
            }

            throw new NotFoundException("User not found!");
        }

        public async Task<UserViewModel> AddUserSupervisor(Guid id, Guid supervisorId)
        {
            if (id == supervisorId)
                throw new BadRequestException("User cannot be his own supervisor!", []);

            var user = await _userRepository.GetByIdAsync(id);

            if (user != null)
            {
                var userSupervisors = await _userRepository.GetUserSupervisorsByIdAsync(id);

                if (user.Supervisors.Count > 0 && user.Supervisors.Any(x => x.SupervisorId == supervisorId))
                    throw new BadRequestException("Supervisor already exists!", []);

                var supervisor = await _userRepository.GetByIdAsync(supervisorId);

                if (supervisor != null)
                {
                    await _userRepository.AddSupervisorAsync(id, supervisorId);
                    return UserViewModel.FromEntity(user);
                }

                throw new NotFoundException("Supervisor not found!");
            }

            throw new NotFoundException("User not found!");
        }

        public async Task<UserViewModel> DeleteUserSupervisor(Guid id, Guid supervisorId)
        {
            if (id == supervisorId)
                throw new BadRequestException("User cannot be its own supervisor!", []);

            var user = await _userRepository.GetByIdAsync(id);

            if (user != null)
            {
                var userSupervisors = await _userRepository.GetUserSupervisorsByIdAsync(id);

                if (user.Supervisors.Count == 0 || !user.Supervisors.Any(x => x.SupervisorId == supervisorId))
                    throw new BadRequestException("Supervisor does not exists!", []);

                var supervisor = await _userRepository.GetByIdAsync(supervisorId);

                if (supervisor != null)
                {
                    await _userRepository.DeleteSupervisorAsync(id, supervisorId);
                    return UserViewModel.FromEntity(user);
                }

                throw new NotFoundException("Supervisor not found!");
            }

            throw new NotFoundException("User not found!");
        }
    }
}
