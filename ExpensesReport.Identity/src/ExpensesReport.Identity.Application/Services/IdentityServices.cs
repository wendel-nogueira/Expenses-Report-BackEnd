using ExpensesReport.Identity.Application.Exceptions;
using ExpensesReport.Identity.Application.InputModels;
using ExpensesReport.Identity.Application.Publishers;
using ExpensesReport.Identity.Application.Validators;
using ExpensesReport.Identity.Application.ViewModels;
using ExpensesReport.Identity.Core.Constants;
using ExpensesReport.Identity.Core.Enums;
using ExpensesReport.Identity.Core.repositories;
using ExpensesReport.Identity.Infrastructure.Authentication;
using ExpensesReport.Identity.Infrastructure.Queue;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace ExpensesReport.Identity.Application.Services
{
    public class IdentityServices(IUserIdentityRepository userIdentityRepository, IConfiguration config, MailQueue mailQueue) : IIdentityServices
    {
        private readonly IUserIdentityRepository _userIdentityRepository = userIdentityRepository;
        private readonly IConfiguration _config = config;
        private readonly MailQueue _mailQueue = mailQueue;

        public async Task<IdentityViewModel> GetIdentityById(Guid id)
        {
            var identity = await _userIdentityRepository.GetByIdAsync(id.ToString()) ?? throw new NotFoundException("Identity not found!");
            var role = await _userIdentityRepository.GetRoleByIdentityIdAsync(identity.Id) ?? throw new NotFoundException("Role not found!");
            var identityRole = UserIdentityRoleExtensions.ToEnum(role.Name!);

            return IdentityViewModel.FromEntity(identity, identityRole);
        }

        public async Task<IdentityViewModel> GetIdentityByEmail(string email)
        {
            var identity = await _userIdentityRepository.GetByEmailAsync(email) ?? throw new NotFoundException("Identity not found!");
            var role = await _userIdentityRepository.GetRoleByIdentityIdAsync(identity.Id) ?? throw new NotFoundException("Role not found!");
            var identityRole = UserIdentityRoleExtensions.ToEnum(role.Name!);

            return IdentityViewModel.FromEntity(identity, identityRole);
        }

        public async Task<IdentityCheckViewModel> GetMe(string token)
        {
            var tokenDecoded = AuthServices.DecodeToken(token);
            var identityId = tokenDecoded[0].Value;

            var identity = await _userIdentityRepository.GetByIdAsync(identityId) ?? throw new NotFoundException("Identity not found!");
            var role = await _userIdentityRepository.GetRoleByIdentityIdAsync(identity.Id) ?? throw new NotFoundException("Role not found!");
            var identityRole = UserIdentityRoleExtensions.ToEnum(role.Name!);

            var identityPermissions = UserIdentityPermissions.GetPermissions(role.Name!);

            return IdentityCheckViewModel.FromEntity(identity, identityRole, identityPermissions);
        }

        public async Task<IEnumerable<IdentityViewModel>> GetAll()
        {
            var identities = await _userIdentityRepository.GetAllAsync();

            var identitiesViewModel = identities.Select(identity =>
            {
                var role = _userIdentityRepository.GetRoleByIdentityIdAsync(identity.Id).Result;
                var identityRole = UserIdentityRoleExtensions.ToEnum(role!.Name!);

                return IdentityViewModel.FromEntity(identity, identityRole);
            });

            return identitiesViewModel;
        }

        public async Task<IEnumerable<IdentityViewModel>> GetAllByRole(string role)
        {
            var identities = await _userIdentityRepository.GetAllByRoleAsync(role);

            var identitiesViewModel = identities.Select(identity =>
            {
                var role = _userIdentityRepository.GetRoleByIdentityIdAsync(identity.Id).Result;
                var identityRole = UserIdentityRoleExtensions.ToEnum(role!.Name!);

                return IdentityViewModel.FromEntity(identity, identityRole);
            });

            return identitiesViewModel;
        }

        public async Task<IEnumerable<RoleViewModel>> GetAllRoles()
        {
            var roles = await _userIdentityRepository.GetAllRolesAsync();

            var rolesViewModel = roles.Select(role => RoleViewModel.FromEntity(role));

            return rolesViewModel;
        }
        public async Task<AuthenticationViewModel> Login(LoginInputModel inputModel)
        {
            var errorsInput = InputModelValidator.Validate(inputModel);

            if (errorsInput?.Length > 0)
                throw new BadRequestException("Error on login!", errorsInput);

            var identity = await _userIdentityRepository.GetByEmailAsync(inputModel.Email!) ?? throw new BadRequestException("Email or password invalid!", []);
            var role = await _userIdentityRepository.GetRoleByIdentityIdAsync(identity.Id);

            if (identity.IsDeleted)
                throw new BadRequestException("Email or password invalid!", []);

            if (identity.PasswordHash == null || identity.PasswordHash == string.Empty)
            {
                var subscriber = new MailPublisher(_mailQueue);
                var tokenCreatePassword = AuthServices.GenerateToken(identity, role!.Name!, _config);

                subscriber.SendAddPasswordMail(identity.Id, identity.Email!, tokenCreatePassword);

                throw new BadRequestException("Error on login!", new[] { "Password not created, check your email to create a password!" });
            }

            var passwordValid = BCrypt.Net.BCrypt.Verify(inputModel.Password!, identity.PasswordHash!);

            if (!passwordValid)
                throw new BadRequestException("Email or password invalid!", []);

            var token = AuthServices.GenerateToken(identity, role!.Name!, _config);

            return AuthenticationViewModel.FromEntity(token);
        }

        public async Task<IdentityViewModel> AddIdentity(AddIdentityInputModel inputModel)
        {
            var errorsInput = InputModelValidator.Validate(inputModel);

            if (errorsInput?.Length > 0)
            {
                throw new BadRequestException("Error on create identity!", errorsInput);
            }

            var (identity, role) = inputModel.ToEntity();

            identity.UserName = inputModel.Email!.Split("@")[0];
            identity.Email = inputModel.Email!;

            var emailExists = await _userIdentityRepository.GetByEmailAsync(identity.Email);

            if (emailExists != null)
            {
                throw new BadRequestException("Email already exists!", []);
            }

            var identityResult = await _userIdentityRepository.AddAsync(identity, role);

            if (!identityResult.Succeeded)
            {
                var errorsRepository = identityResult.Errors.Select(error => error.Description);

                throw new BadRequestException("Error on create identity!", errorsRepository);
            }

            var roleResult = UserIdentityRoleExtensions.ToEnum(role.Name!);

            var subscriber = new MailPublisher(_mailQueue);
            var token = AuthServices.GenerateToken(identity, role!.Name!, _config);

            subscriber.SendAddPasswordMail(identity.Id, identity.Email!, token);

            return IdentityViewModel.FromEntity(identity, roleResult);
        }

        public async Task AddIdentityPassword(string token, ChangePasswordInputModel inputModel)
        {
            var errorsInput = InputModelValidator.Validate(inputModel);

            if (errorsInput?.Length > 0)
                throw new BadRequestException("Error on update identity password!", errorsInput);

            if (token == null || token == string.Empty)
                throw new BadRequestException("Token invalid!", []);

            var identityId = AuthServices.DecodeToken(token)[0].Value;
            var identity = await _userIdentityRepository.GetByIdAsync(identityId) ?? throw new NotFoundException("Token invalid!");

            if (identity.PasswordHash != null && identity.PasswordHash != string.Empty)
                throw new BadRequestException("Token invalid!", []);

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(inputModel.NewPassword!);

            identity.EmailConfirmed = true;
            identity.PasswordHash = passwordHash;

            await _userIdentityRepository.UpdateIdentityAsync(identity);
        }

        public async Task UpdateIdentityPassword(Guid id, ChangePasswordInputModel inputModel)
        {
            var errorsInput = InputModelValidator.Validate(inputModel);

            if (errorsInput?.Length > 0)
                throw new BadRequestException("Error on update identity password!", errorsInput);

            var identity = await _userIdentityRepository.GetByIdAsync(id.ToString()) ?? throw new NotFoundException("Identity not found!");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(inputModel.NewPassword!);
            identity.PasswordHash = passwordHash;

            await _userIdentityRepository.UpdateIdentityAsync(identity);
        }

        public async Task UpdateIdentityEmail(Guid id, ChangeEmailInputModel inputModel)
        {
            var errorsInput = InputModelValidator.Validate(inputModel);

            if (errorsInput?.Length > 0)
                throw new BadRequestException("Error on update identity email!", errorsInput);

            var identity = await _userIdentityRepository.GetByIdAsync(id.ToString()) ?? throw new NotFoundException("Identity not found!");
            var emailAlreadyExists = await _userIdentityRepository.GetByEmailAsync(inputModel.NewEmail!);

            if (emailAlreadyExists != null && emailAlreadyExists.Id != identity.Id)
                throw new BadRequestException("Email already exists!", []);

            identity.Email = inputModel.NewEmail!;

            await _userIdentityRepository.UpdateIdentityAsync(identity);
        }

        public async Task UpdateIdentityRole(Guid id, ChangeIdentityRoleInputModel inputModel)
        {
            var errorsInput = InputModelValidator.Validate(inputModel);

            if (errorsInput?.Length > 0)
                throw new BadRequestException("Error on update identity role!", errorsInput);

            var identity = await _userIdentityRepository.GetByIdAsync(id.ToString()) ?? throw new NotFoundException("Identity not found!");
            var newRole = inputModel.Role!;

            await _userIdentityRepository.UpdateIdentityRoleAsync(identity, newRole);
        }

        public async Task DeleteIdentity(Guid id)
        {
            var identity = await _userIdentityRepository.GetByIdAsync(id.ToString()) ?? throw new NotFoundException("Identity not found!");

            await _userIdentityRepository.DeleteAsync(identity.Id);
        }
    }
}
