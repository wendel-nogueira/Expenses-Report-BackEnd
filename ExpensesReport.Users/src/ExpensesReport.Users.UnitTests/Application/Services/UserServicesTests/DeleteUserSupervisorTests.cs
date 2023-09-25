using ExpensesReport.Users.Application.Exceptions;
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
    public class DeleteUserSupervisorTests
    {
        [Fact]
        public void ValidUserAndSupervisorAreDeleted()
        {
            var usersMock = Enumerable.Range(0, 1).Select(i => new User(
                new UserName("FirstName", "LastName"),
                (UserRole)1,
                $"test{i}@gmail.com",
                new UserAddress("address", "city", "state", "country", "zip")
                ));
            var userRepositoryMock = new Mock<IUserRepository>();
            var userServices = new UserServices(userRepositoryMock.Object);
            usersMock.First().AddSupervisorToUser(usersMock.Last().Id);


            var user = usersMock.First();
            var supervisorToDelete = usersMock.Last();
            user.AddSupervisorToUser(supervisorToDelete.Id);
            userRepositoryMock.Setup(userRepository => userRepository.GetByIdAsync(user.Id)).ReturnsAsync(user);
            userRepositoryMock.Setup(userRepository => userRepository.GetByIdAsync(supervisorToDelete.Id)).ReturnsAsync(supervisorToDelete);
            var result = userServices.DeleteUserSupervisor(user.Id, supervisorToDelete.Id);


            userRepositoryMock.Verify(userRepository => userRepository.DeleteSupervisorAsync(user.Id, supervisorToDelete.Id), Times.Once);
        }

        [Fact]
        public async void InvalidUserAndSupervisorNotDeleted_WhenUserDoesNotExist()
        {
            var usersMock = Enumerable.Range(0, 1).Select(i => new User(
                new UserName("FirstName", "LastName"),
                (UserRole)1,
                $"test{i}@gmail.com",
                new UserAddress("address", "city", "state", "country", "zip")
                ));
            var userRepositoryMock = new Mock<IUserRepository>();
            var userServices = new UserServices(userRepositoryMock.Object);
            usersMock.First().AddSupervisorToUser(usersMock.Last().Id);


            var user = usersMock.First();
            var supervisorToDelete = usersMock.Last();
            userRepositoryMock.Setup(userRepository => userRepository.GetByIdAsync(user.Id)).ReturnsAsync(user);
            userRepositoryMock.Setup(userRepository => userRepository.GetByIdAsync(supervisorToDelete.Id)).ReturnsAsync(supervisorToDelete);
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => userServices.DeleteUserSupervisor(Guid.NewGuid(), supervisorToDelete.Id));


            exception.Message.ShouldBe("User not found!");
            userRepositoryMock.Verify(userRepository => userRepository.AddSupervisorAsync(Guid.NewGuid(), supervisorToDelete.Id), Times.Never);
        }

        [Fact]
        public async void InvalidUserAndSupervisorNotDeleted_WhenSupervisorDoesNotExist()
        {
            var usersMock = Enumerable.Range(0, 1).Select(i => new User(
                new UserName("FirstName", "LastName"),
                (UserRole)1,
                $"test{i}@gmail.com",
                new UserAddress("address", "city", "state", "country", "zip")
                ));
            var userRepositoryMock = new Mock<IUserRepository>();
            var userServices = new UserServices(userRepositoryMock.Object);
            usersMock.First().AddSupervisorToUser(usersMock.Last().Id);


            var user = usersMock.First();
            var supervisorToDelete = usersMock.Last();
            userRepositoryMock.Setup(userRepository => userRepository.GetByIdAsync(user.Id)).ReturnsAsync(user);
            userRepositoryMock.Setup(userRepository => userRepository.GetByIdAsync(supervisorToDelete.Id)).ReturnsAsync(supervisorToDelete);
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => userServices.DeleteUserSupervisor(supervisorToDelete.Id, Guid.NewGuid()));


            exception.Message.ShouldBe("Supervisor not found!");
            userRepositoryMock.Verify(userRepository => userRepository.AddSupervisorAsync(supervisorToDelete.Id, Guid.NewGuid()), Times.Never);
        }

        [Fact]
        public async void InvalidUserAndSupervisorNotDeleted_WhenSupervisorDoesNotExistsInUser()
        {
            var usersMock = Enumerable.Range(0, 2).Select(i => new User(
                new UserName("FirstName", "LastName"),
                (UserRole)1,
                $"test{i}@gmail.com",
                new UserAddress("address", "city", "state", "country", "zip")
                ));
            var userRepositoryMock = new Mock<IUserRepository>();
            var userServices = new UserServices(userRepositoryMock.Object);


            var user = usersMock.First();
            var supervisorToDelete = usersMock.Last();
            userRepositoryMock.Setup(userRepository => userRepository.GetByIdAsync(user.Id)).ReturnsAsync(user);
            userRepositoryMock.Setup(userRepository => userRepository.GetByIdAsync(supervisorToDelete.Id)).ReturnsAsync(supervisorToDelete);
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => userServices.DeleteUserSupervisor(user.Id, supervisorToDelete.Id));


            exception.Message.ShouldBe("Supervisor does not exists!");
            userRepositoryMock.Verify(userRepository => userRepository.DeleteSupervisorAsync(user.Id, supervisorToDelete.Id), Times.Never);
        }
    }
}
