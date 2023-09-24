using AutoFixture;
using ExpensesReport.Users.API.Exceptions;
using ExpensesReport.Users.Application.InputModels;
using ExpensesReport.Users.Application.Services;
using ExpensesReport.Users.Core.Entities;
using ExpensesReport.Users.Core.Enums;
using ExpensesReport.Users.Core.Repositories;
using Moq;
using Shouldly;
using Xunit;

namespace ExpensesReport.Users.UnitTests.Application.Services.UserServicesTests
{
    public class AddUserTests
    {
        [Fact]
        public void ValidUserIsAdded()
        {
            var addUserInputModel = new Fixture().Create<AddUserInputModel>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var userServices = new UserServices(userRepositoryMock.Object);
            addUserInputModel.Email = "test@gmail.com";


            var result = userServices.AddUser(addUserInputModel);


            result.Result.Name.FirstName.ShouldBe(addUserInputModel.Name.FirstName);
            result.Result.Name.LastName.ShouldBe(addUserInputModel.Name.LastName);

            result.Result.Email.ShouldBe(addUserInputModel.Email);
            result.Result.Role.ShouldBe(addUserInputModel.Role);

            result.Result.Address.Address.ShouldBe(addUserInputModel.Address.Address);
            result.Result.Address.City.ShouldBe(addUserInputModel.Address.City);
            result.Result.Address.State.ShouldBe(addUserInputModel.Address.State);
            result.Result.Address.Country.ShouldBe(addUserInputModel.Address.Country);
            result.Result.Address.Zip.ShouldBe(addUserInputModel.Address.Zip);

            userRepositoryMock.Verify(userRepository => userRepository.AddAsync(It.IsAny<User>()), Times.Once);

            var teste = userServices.GetUsers();

            Console.WriteLine(teste);
        }

        [Fact]
        public async Task InvalidUserIsNotAdded_WhenEmailAlreadyExists()
        {
            var addUserInputModel = new Fixture().Create<AddUserInputModel>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var userServices = new UserServices(userRepositoryMock.Object);
            addUserInputModel.Email = "test@gmail.com";


            var user = addUserInputModel.ToEntity();
            userRepositoryMock.Setup(userRepository => userRepository.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);


            var exception = await Assert.ThrowsAsync<BadRequestException>(() => userServices.AddUser(addUserInputModel));

            exception.Message.ShouldBe("Email already exists!");
            userRepositoryMock.Verify(userRepository => userRepository.AddAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task InvalidUserIsNotAdded_WhenEmailIsInvalid()
        {
            var addUserInputModel = new Fixture().Create<AddUserInputModel>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var userServices = new UserServices(userRepositoryMock.Object);
            addUserInputModel.Email = "testgmail.com";


            var exception = await Assert.ThrowsAsync<BadRequestException>(() => userServices.AddUser(addUserInputModel));

            exception.Message.ShouldBe("User data is required! Check that all fields have been filled in correctly.");
            exception?.Errors?.First().ShouldBe("Email must be a valid email!");
            userRepositoryMock.Verify(userRepository => userRepository.AddAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task InvalidUserIsNotAdded_WhenRoleIsInvalid()
        {
            var addUserInputModel = new Fixture().Create<AddUserInputModel>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var userServices = new UserServices(userRepositoryMock.Object);
            addUserInputModel.Role = (UserRole)3;
            addUserInputModel.Email = "test@gmail.com";


            var exception = await Assert.ThrowsAsync<BadRequestException>(() => userServices.AddUser(addUserInputModel));


            exception.Message.ShouldBe("User data is required! Check that all fields have been filled in correctly.");
            exception?.Errors?.First().ShouldBe("Role must be a valid value!");
            userRepositoryMock.Verify(userRepository => userRepository.AddAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task InvalidUserIsNotAdded_WhenUserPropertiesAreInvalid()
        {
            var addUserInputModel = new Fixture().Create<AddUserInputModel>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var userServices = new UserServices(userRepositoryMock.Object);
            addUserInputModel.Name.FirstName = "";
            addUserInputModel.Name.LastName = "";
            addUserInputModel.Email = "";
            addUserInputModel.Role = (UserRole)3;
            addUserInputModel.Address.Address = "";
            addUserInputModel.Address.City = "";
            addUserInputModel.Address.State = "";
            addUserInputModel.Address.Country = "";
            addUserInputModel.Address.Zip = "";


            var exception = await Assert.ThrowsAsync<BadRequestException>(() => userServices.AddUser(addUserInputModel));


            exception.Message.ShouldBe("User data is required! Check that all fields have been filled in correctly.");
        }
    }
}
