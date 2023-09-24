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
    public class AddUserSupervisorTests
    {
        [Fact]
        public void ValidUserAndSupervisorAreAdded()
        {
            var usersMock = Enumerable.Range(0, 1).Select(i => new User(
                new UserName("FirstName", "LastName"),
                (UserRole)1,
                $"test{i}@gmail.com",
                new UserAddress("address", "city", "state", "country", "zip")
                ));
            var userRepositoryMock = new Mock<IUserRepository>();
            var userServices = new UserServices(userRepositoryMock.Object);


            var userToAdd = usersMock.First();
            var supervisorToAdd = usersMock.Last();
            userRepositoryMock.Setup(userRepository => userRepository.GetByIdAsync(userToAdd.Id)).ReturnsAsync(userToAdd);
            userRepositoryMock.Setup(userRepository => userRepository.GetByIdAsync(supervisorToAdd.Id)).ReturnsAsync(supervisorToAdd);
            var result = userServices.AddUserSupervisor(userToAdd.Id, supervisorToAdd.Id);


            userRepositoryMock.Verify(userRepository => userRepository.AddSupervisorAsync(userToAdd.Id, supervisorToAdd.Id), Times.Once);
        }

        [Fact]
        public async void InvalidUserAndSupervisorNotAdded_WhenUserDoesNotExist()
        {
            var usersMock = Enumerable.Range(0, 1).Select(i => new User(
                new UserName("FirstName", "LastName"),
                (UserRole)1,
                $"test{i}@gmail.com",
                new UserAddress("address", "city", "state", "country", "zip")
                ));
            var userRepositoryMock = new Mock<IUserRepository>();
            var userServices = new UserServices(userRepositoryMock.Object);


            var userToAdd = usersMock.First();
            var supervisorToAdd = usersMock.Last();
            userRepositoryMock.Setup(userRepository => userRepository.GetByIdAsync(userToAdd.Id)).ReturnsAsync(userToAdd);
            userRepositoryMock.Setup(userRepository => userRepository.GetByIdAsync(supervisorToAdd.Id)).ReturnsAsync(supervisorToAdd);
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => userServices.AddUserSupervisor(Guid.NewGuid(), supervisorToAdd.Id));


            exception.Message.ShouldBe("User not found!");
            userRepositoryMock.Verify(userRepository => userRepository.AddSupervisorAsync(userToAdd.Id, supervisorToAdd.Id), Times.Never);
        }

        [Fact]
        public async void InvalidUserAndSupervisorNotAdded_WhenSupervisorDoesNotExist()
        {
            var usersMock = Enumerable.Range(0, 1).Select(i => new User(
                new UserName("FirstName", "LastName"),
                (UserRole)1,
                $"test{i}@gmail.com",
                new UserAddress("address", "city", "state", "country", "zip")
                ));
            var userRepositoryMock = new Mock<IUserRepository>();
            var userServices = new UserServices(userRepositoryMock.Object);


            var userToAdd = usersMock.First();
            var supervisorToAdd = usersMock.Last();
            userRepositoryMock.Setup(userRepository => userRepository.GetByIdAsync(userToAdd.Id)).ReturnsAsync(userToAdd);
            userRepositoryMock.Setup(userRepository => userRepository.GetByIdAsync(supervisorToAdd.Id)).ReturnsAsync(supervisorToAdd);
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => userServices.AddUserSupervisor(userToAdd.Id, Guid.NewGuid()));


            exception.Message.ShouldBe("Supervisor not found!");
            userRepositoryMock.Verify(userRepository => userRepository.AddSupervisorAsync(userToAdd.Id, supervisorToAdd.Id), Times.Never);
        }

        [Fact]
        public async void InvalidUserAndSupervisorNotAdded_WhenUserIsSupervisor()
        {
            var usersMock = Enumerable.Range(0, 1).Select(i => new User(
                new UserName("FirstName", "LastName"),
                (UserRole)1,
                $"test{i}@gmail.com",
                new UserAddress("address", "city", "state", "country", "zip")
                ));
            var userRepositoryMock = new Mock<IUserRepository>();
            var userServices = new UserServices(userRepositoryMock.Object);


            var userToAdd = usersMock.First();
            var supervisorToAdd = usersMock.Last();
            userRepositoryMock.Setup(userRepository => userRepository.GetByIdAsync(userToAdd.Id)).ReturnsAsync(userToAdd);
            userRepositoryMock.Setup(userRepository => userRepository.GetByIdAsync(supervisorToAdd.Id)).ReturnsAsync(supervisorToAdd);
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => userServices.AddUserSupervisor(userToAdd.Id, userToAdd.Id));


            exception.Message.ShouldBe("User cannot be his own supervisor!");
            userRepositoryMock.Verify(userRepository => userRepository.AddSupervisorAsync(userToAdd.Id, supervisorToAdd.Id), Times.Never);
        }
    }
}
