using ExpensesReport.Users.Application.InputModels;
using ExpensesReport.Users.Application.ViewModels;
using ExpensesReport.Users.Application.Validators;
using ExpensesReport.Users.Application.Exceptions;
using ExpensesReport.Users.Core.Repositories;
using Xunit;
using ExpensesReport.Users.Application.Services;
using ExpensesReport.Users.Core.Entities;
using ExpensesReport.Users.Core.Enums;
using ExpensesReport.Users.Core.ValueObjects;
using Moq;
using ExpensesReport.Users.Infrastructure.Persistence.Repositories;
using Shouldly;

namespace ExpensesReport.Users.UnitTests.Application.Services.UserServicesTests
{
    public class GetUserSupervisorsByIdTests
    {
        [Fact]
        public void ShouldReturnUserSupervisors()
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
            var supervisorToAdd = usersMock.Last();
            user.AddSupervisorToUser(supervisorToAdd.Id);
            userRepositoryMock.Setup(userRepository => userRepository.GetByIdAsync(user.Id)).ReturnsAsync(user);
            userRepositoryMock.Setup(userRepository => userRepository.GetByIdAsync(supervisorToAdd.Id)).ReturnsAsync(supervisorToAdd);
            var supervisors = Enumerable.Range(0, 1).Select(i => supervisorToAdd);
            userRepositoryMock.Setup(userRepository => userRepository.GetUserSupervisorsByIdAsync(supervisorToAdd.Id)).ReturnsAsync(supervisors);
            var result = userServices.GetUserSupervisorsById(supervisorToAdd.Id);



            result.Result.Count().ShouldBe(supervisors.Count());
            userRepositoryMock.Verify(userRepository => userRepository.GetUserSupervisorsByIdAsync(supervisorToAdd.Id), Times.Once);
        }

        [Fact]
        public void ShouldReturnEmptyList_WhenNoSupervisors()
        {
            var userMock = new User(
                new UserName("FirstName", "LastName"),
                (UserRole)1,
                $"teste@gmail.com",
                new UserAddress("address", "city", "state", "country", "zip")
                );
            var userRepositoryMock = new Mock<IUserRepository>();
            var userServices = new UserServices(userRepositoryMock.Object);


            userRepositoryMock.Setup(userRepository => userRepository.GetByIdAsync(userMock.Id)).ReturnsAsync(userMock);
            var result = userServices.GetUserSupervisorsById(userMock.Id);


            result.Result.Count().ShouldBe(0);
            userRepositoryMock.Verify(userRepository => userRepository.GetUserSupervisorsByIdAsync(userMock.Id), Times.Once);
        }

        [Fact]
        public async void ShouldThrowNotFoundException_WhenUserDoesNotExist()
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
            var supervisorToAdd = usersMock.Last();
            user.AddSupervisorToUser(supervisorToAdd.Id);
            userRepositoryMock.Setup(userRepository => userRepository.GetByIdAsync(user.Id)).ReturnsAsync(user);
            userRepositoryMock.Setup(userRepository => userRepository.GetByIdAsync(supervisorToAdd.Id)).ReturnsAsync(supervisorToAdd);
            var supervisors = Enumerable.Range(0, 1).Select(i => supervisorToAdd);
            userRepositoryMock.Setup(userRepository => userRepository.GetUserSupervisorsByIdAsync(supervisorToAdd.Id)).ReturnsAsync(supervisors);
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => userServices.GetUserSupervisorsById(Guid.NewGuid()));


            exception.Message.ShouldBe("User not found!");
            userRepositoryMock.Verify(userRepository => userRepository.GetUserSupervisorsByIdAsync(supervisorToAdd.Id), Times.Never);
        }
    }
}
