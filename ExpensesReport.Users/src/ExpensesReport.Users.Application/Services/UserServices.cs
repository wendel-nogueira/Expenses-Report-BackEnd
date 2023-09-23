using ExpensesReport.Users.API.Exceptions;
using ExpensesReport.Users.Application.InputModels;
using ExpensesReport.Users.Application.ViewModels;
using ExpensesReport.Users.Core.Repositories;

namespace ExpensesReport.Users.Application.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userRepository;

        public UserServices(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserViewModel> GetUserById(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user != null)
                return UserViewModel.FromEntity(user);

            throw new NotFoundException("User not found!");
        }

        public async Task<UserViewModel> GetUserByEmail(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);

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
            if (inputModel == null)
                throw new BadRequestException("User data is required! Check that all fields have been filled in correctly.");

            var user = inputModel.ToEntity();
            var emailAlreadyExists = await _userRepository.GetByEmailAsync(user.Email);

            if (emailAlreadyExists != null)
                throw new BadRequestException("Email already exists!");

            await _userRepository.AddAsync(user);

            return UserViewModel.FromEntity(user);
        }

        public async Task<UserViewModel> UpdateUser(Guid id, UpdateUserInputModel inputModelToUpdate)
        {
            if (inputModelToUpdate == null)
                throw new BadRequestException("User data is required! Check that all fields have been filled in correctly.");

            var user = await _userRepository.GetByIdAsync(id);

            if (user != null)
            {
                var userToUpdate = inputModelToUpdate.ToEntity();
                var emailAlreadyExists = await _userRepository.GetByEmailAsync(userToUpdate.Email);

                if (emailAlreadyExists != null && emailAlreadyExists.Id != id)
                    throw new BadRequestException("Email already exists!");

                await _userRepository.UpdateAsync(id, userToUpdate);

                return UserViewModel.FromEntity(userToUpdate);
            }

            throw new NotFoundException("User not found!");
        }

        public async Task DeleteUser(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user != null)
            {
                await _userRepository.DeleteAsync(id);
                return;
            }

            throw new NotFoundException("User not found!");
        }

        public async Task<UserViewModel> AddUserSupervisor(Guid id, Guid supervisorId)
        {
            if (id == supervisorId)
                throw new BadRequestException("User cannot be his own supervisor!");

            var user = await _userRepository.GetByIdAsync(id);

            if (user != null)
            {
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
                throw new BadRequestException("User cannot be its own supervisor!");

            var user = await _userRepository.GetByIdAsync(id);

            if (user != null)
            {
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
