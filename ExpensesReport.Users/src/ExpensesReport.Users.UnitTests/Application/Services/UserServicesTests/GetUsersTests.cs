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
    public class GetUsersTests
    {
        [Fact]
        public void ShouldReturnAllUsers()
        {
            var usersMock = Enumerable.Range(0, 10).Select(i => new User(
                new UserName("FirstName", "LastName"),
                (UserRole)1,
                $"test{i}@gmail.com",
                new UserAddress("address", "city", "state", "country", "zip")
            ));
            var userRepositoryMock = new Mock<IUserRepository>();
            var userServices = new UserServices(userRepositoryMock.Object);


            userRepositoryMock.Setup(userRepository => userRepository.GetAllAsync()).ReturnsAsync(usersMock);
            var result = userServices.GetUsers();


            result.Result.Count().ShouldBe(usersMock.Count());
            userRepositoryMock.Verify(userRepository => userRepository.GetAllAsync(), Times.Once);
        }

        [Fact]
        public void ShouldReturnEmptyList_WhenNoUsers()
        {
            var usersMock = Enumerable.Range(0, 0).Select(i => new User(
                new UserName("FirstName", "LastName"),
                (UserRole)1,
                $"test{i}@gmail.com",
                new UserAddress("address", "city", "state", "country", "zip")
            ));
            var userRepositoryMock = new Mock<IUserRepository>();
            var userServices = new UserServices(userRepositoryMock.Object);


            userRepositoryMock.Setup(userRepository => userRepository.GetAllAsync()).ReturnsAsync(usersMock);
            var result = userServices.GetUsers();


            result.Result.Count().ShouldBe(0);
            userRepositoryMock.Verify(userRepository => userRepository.GetAllAsync(), Times.Once);
        }
    }
}
