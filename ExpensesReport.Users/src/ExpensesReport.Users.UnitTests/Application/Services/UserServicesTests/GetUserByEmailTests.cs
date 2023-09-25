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
    public class GetUserByEmailTests
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


            var userEmailToGet = userMock.Email;
            userRepositoryMock.Setup(userRepository => userRepository.GetByEmailAsync(userMock.Email)).ReturnsAsync(userMock);
            var result = userServices.GetUserByEmail(userEmailToGet);


            result.Result.Name.FirstName.ShouldBe(userMock.Name.FirstName);
            result.Result.Name.LastName.ShouldBe(userMock.Name.LastName);

            result.Result.Email.ShouldBe(userMock.Email);
            result.Result.Role.ShouldBe(userMock.Role);

            result.Result.Address.Address.ShouldBe(userMock.Address.Address);
            result.Result.Address.City.ShouldBe(userMock.Address.City);
            result.Result.Address.State.ShouldBe(userMock.Address.State);
            result.Result.Address.Country.ShouldBe(userMock.Address.Country);
            result.Result.Address.Zip.ShouldBe(userMock.Address.Zip);
            userRepositoryMock.Verify(userRepository => userRepository.GetByEmailAsync(userEmailToGet), Times.Once);
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


            var userEmailToGet = "test1@gmail.com";
            userRepositoryMock.Setup(userRepository => userRepository.GetByEmailAsync(userMock.Email)).ReturnsAsync(userMock);
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => userServices.GetUserByEmail(userEmailToGet));


            exception.Message.ShouldBe("User not found!");
            userRepositoryMock.Verify(userRepository => userRepository.GetByEmailAsync(userEmailToGet), Times.Once);
        }
    }
}
