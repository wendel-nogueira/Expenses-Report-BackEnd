using AutoFixture;
using ExpensesReport.Users.API.Exceptions;
using ExpensesReport.Users.Application.InputModels;
using ExpensesReport.Users.Application.Services;
using ExpensesReport.Users.Core.Entities;
using ExpensesReport.Users.Core.Enums;
using ExpensesReport.Users.Core.Repositories;
using ExpensesReport.Users.Core.ValueObjects;
using Moq;
using Shouldly;
using Xunit;

namespace ExpensesReport.Users.UnitTests.Application.Services.UserServicesTests
{
    public class UpdateUserTests
    {
        [Fact]
        public void ValidUserIsUpdated()
        {
            var updateUserInputModel = new Fixture().Create<UpdateUserInputModel>();
            var userMock = new User(
                new UserName("FirstName", "LastName"),
                (UserRole)1,
                "test@gmail.com",
                new UserAddress("address", "city", "state", "country", "zip")
                );
            var userRepositoryMock = new Mock<IUserRepository>();
            var userServices = new UserServices(userRepositoryMock.Object);
            updateUserInputModel.Email = "updatedEmail@gmail.com";


            var userIdToUpdate = userMock.Id;
            var updatedUser = updateUserInputModel.ToEntity();
            userRepositoryMock.Setup(userRepository => userRepository.GetByIdAsync(userIdToUpdate)).ReturnsAsync(userMock);
            var result = userServices.UpdateUser(userIdToUpdate, updateUserInputModel);


            result.Result.Name.FirstName.ShouldBe(updateUserInputModel.Name.FirstName);
            result.Result.Name.LastName.ShouldBe(updateUserInputModel.Name.LastName);

            result.Result.Email.ShouldBe(updateUserInputModel.Email);
            result.Result.Role.ShouldBe(updateUserInputModel.Role);

            result.Result.Address.Address.ShouldBe(updateUserInputModel.Address.Address);
            result.Result.Address.City.ShouldBe(updateUserInputModel.Address.City);
            result.Result.Address.State.ShouldBe(updateUserInputModel.Address.State);
            result.Result.Address.Country.ShouldBe(updateUserInputModel.Address.Country);
            result.Result.Address.Zip.ShouldBe(updateUserInputModel.Address.Zip);

            userRepositoryMock.Verify(userRepository => userRepository.UpdateAsync(userIdToUpdate, It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async void InvalidUserIsNotUpdated_WhenUserDoesNotExist()
        {
            var updateUserInputModel = new Fixture().Create<UpdateUserInputModel>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var userServices = new UserServices(userRepositoryMock.Object);
            updateUserInputModel.Email = "updatedEmail@gmail.com";


            var exception = await Assert.ThrowsAsync<NotFoundException>(() => userServices.UpdateUser(Guid.NewGuid(), updateUserInputModel));


            exception.Message.ShouldBe("User not found!");
            userRepositoryMock.Verify(userRepository => userRepository.UpdateAsync(Guid.NewGuid(), It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async void InvalidUserIsNotUpdated_WhenEmailAlreadyExists()
        {
            var updateUserInputModel = new Fixture().Create<UpdateUserInputModel>();
            var usersMock = Enumerable.Range(0, 1).Select(i => new User(
                new UserName("FirstName", "LastName"),
                (UserRole)1,
                $"test{i}@gmail.com",
                new UserAddress("address", "city", "state", "country", "zip")
                ));
            var userRepositoryMock = new Mock<IUserRepository>();
            var userServices = new UserServices(userRepositoryMock.Object);
            updateUserInputModel.Email = usersMock.Last().Email;


            var userToUpdate = usersMock.First();
            var userComplement = usersMock.Last();
            userRepositoryMock.Setup(userRepository => userRepository.GetByIdAsync(userToUpdate.Id)).ReturnsAsync(userToUpdate);
            userRepositoryMock.Setup(userRepository => userRepository.GetByEmailAsync(userToUpdate.Email)).ReturnsAsync(userToUpdate);
            userRepositoryMock.Setup(userRepository => userRepository.GetByEmailAsync(userComplement.Email)).ReturnsAsync(userComplement);

            var exception = await Assert.ThrowsAsync<BadRequestException>(() => userServices.UpdateUser(userToUpdate.Id, updateUserInputModel));


            exception.Message.ShouldBe("Email already exists!");
            userRepositoryMock.Verify(userRepository => userRepository.UpdateAsync(userToUpdate.Id, It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task InvalidUserIsNotUpdated_WhenEmailIsInvalid()
        {
            var updateUserInputModel = new Fixture().Create<UpdateUserInputModel>();
            var userMock = new User(
                new UserName("FirstName", "LastName"),
                (UserRole)1,
                $"test@gmail.com",
                new UserAddress("address", "city", "state", "country", "zip")
                );
            var userRepositoryMock = new Mock<IUserRepository>();
            var userServices = new UserServices(userRepositoryMock.Object);
            updateUserInputModel.Email = "testgmail.com";


            userRepositoryMock.Setup(userRepository => userRepository.GetByIdAsync(userMock.Id)).ReturnsAsync(userMock);
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => userServices.UpdateUser(userMock.Id, updateUserInputModel));


            exception.Message.ShouldBe("User data is required! Check that all fields have been filled in correctly.");
            exception?.Errors?.First().ShouldBe("Email must be a valid email!");
            userRepositoryMock.Verify(userRepository => userRepository.UpdateAsync(Guid.NewGuid(), It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task InvalidUserIsNotUpdated_WhenRoleIsInvalid()
        {
            var updateUserInputModel = new Fixture().Create<UpdateUserInputModel>();
            var userMock = new User(
                new UserName("FirstName", "LastName"),
                (UserRole)1,
                $"test@gmail.com",
                new UserAddress("address", "city", "state", "country", "zip")
                );
            var userRepositoryMock = new Mock<IUserRepository>();
            var userServices = new UserServices(userRepositoryMock.Object);
            updateUserInputModel.Role = (UserRole)3;
            updateUserInputModel.Email = "test@gmail.com";


            userRepositoryMock.Setup(userRepository => userRepository.GetByIdAsync(userMock.Id)).ReturnsAsync(userMock);
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => userServices.UpdateUser(userMock.Id, updateUserInputModel));


            exception.Message.ShouldBe("User data is required! Check that all fields have been filled in correctly.");
            exception?.Errors?.First().ShouldBe("Role must be a valid value!");
            userRepositoryMock.Verify(userRepository => userRepository.UpdateAsync(Guid.NewGuid(), It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task InvalidUserIsNotUpdated_WhenUserPropertiesAreInvalid()
        {
            var updateUserInputModel = new Fixture().Create<UpdateUserInputModel>();
            var userMock = new User(
                new UserName("FirstName", "LastName"),
                (UserRole)1,
                $"test@gmail.com",
                new UserAddress("address", "city", "state", "country", "zip")
                );
            var userRepositoryMock = new Mock<IUserRepository>();
            var userServices = new UserServices(userRepositoryMock.Object);
            updateUserInputModel.Name.FirstName = "";
            updateUserInputModel.Name.LastName = "";
            updateUserInputModel.Email = "";
            updateUserInputModel.Role = (UserRole)3;
            updateUserInputModel.Address.Address = "";
            updateUserInputModel.Address.City = "";
            updateUserInputModel.Address.State = "";
            updateUserInputModel.Address.Country = "";
            updateUserInputModel.Address.Zip = "";


            userRepositoryMock.Setup(userRepository => userRepository.GetByIdAsync(userMock.Id)).ReturnsAsync(userMock);
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => userServices.UpdateUser(userMock.Id, updateUserInputModel));


            exception.Message.ShouldBe("User data is required! Check that all fields have been filled in correctly.");
            userRepositoryMock.Verify(userRepository => userRepository.UpdateAsync(Guid.NewGuid(), It.IsAny<User>()), Times.Never);
        }
    }
}
