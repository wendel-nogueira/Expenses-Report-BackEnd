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
    public class GetUserByIdTests
    {
        [Fact]
        public void ShouldReturnUser_WhenUserExists()
        {
            var userMock = new User(
                new UserName("FirstName", "LastName"),
                (UserRole)1,
                "test@gmail.com",
                new UserAddress("address", "city", "state", "country", "zip")
                );
            var userRepositoryMock = new Mock<IUserRepository>();
            var userServices = new UserServices(userRepositoryMock.Object);


            var userIdToGet = userMock.Id;
            userRepositoryMock.Setup(userRepository => userRepository.GetByIdAsync(userIdToGet)).ReturnsAsync(userMock);
            var result = userServices.GetUserById(userIdToGet);


            result.Result.Name.FirstName.ShouldBe(userMock.Name.FirstName);
            result.Result.Name.LastName.ShouldBe(userMock.Name.LastName);

            result.Result.Email.ShouldBe(userMock.Email);
            result.Result.Role.ShouldBe(userMock.Role);

            result.Result.Address.Address.ShouldBe(userMock.Address.Address);
            result.Result.Address.City.ShouldBe(userMock.Address.City);
            result.Result.Address.State.ShouldBe(userMock.Address.State);
            result.Result.Address.Country.ShouldBe(userMock.Address.Country);
            result.Result.Address.Zip.ShouldBe(userMock.Address.Zip);
            userRepositoryMock.Verify(userRepository => userRepository.GetByIdAsync(userIdToGet), Times.Once);
        }

        [Fact]
        public async void NotShouldReturn_WhenUserDoesNotExist()
        {
            var userMock = new User(
                new UserName("FirstName", "LastName"),
                (UserRole)1,
                "test@gmail.com",
                new UserAddress("address", "city", "state", "country", "zip")
            );
            var userRepositoryMock = new Mock<IUserRepository>();
            var userServices = new UserServices(userRepositoryMock.Object);


            var userIdComplement = userMock.Id;
            userRepositoryMock.Setup(userRepository => userRepository.GetByIdAsync(userIdComplement)).ReturnsAsync(userMock);
            var userIdToGet = Guid.NewGuid();
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => userServices.GetUserById(userIdToGet));


            exception.Message.ShouldBe("User not found!");
            userRepositoryMock.Verify(userRepository => userRepository.GetByIdAsync(userIdToGet), Times.Once);
        }
    }
}
