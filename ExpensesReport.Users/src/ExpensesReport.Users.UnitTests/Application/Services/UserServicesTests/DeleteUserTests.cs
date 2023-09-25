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
    public class DeleteUserTests
    {
        [Fact]
        public void ValidUserIsDeleted()
        {
            var userMock = new User(
                new UserName("FirstName", "LastName"),
                (UserRole)1,
                "test@gmail.com",
                new UserAddress("address", "city", "state", "country", "zip")
                );
            var userRepositoryMock = new Mock<IUserRepository>();
            var userServices = new UserServices(userRepositoryMock.Object);


            var userIdToDelete = userMock.Id;
            userRepositoryMock.Setup(userRepository => userRepository.GetByIdAsync(userIdToDelete)).ReturnsAsync(userMock);
            var result = userServices.DeleteUser(userIdToDelete);


            userRepositoryMock.Verify(userRepository => userRepository.DeleteAsync(userIdToDelete), Times.Once);
        }

        [Fact]
        public async void InvalidUserIsNotDeleted_WhenUserDoesNotExist()
        {
            var userMock = new User(
                new UserName("FirstName", "LastName"),
                (UserRole)1,
                "test@gmail.com",
                new UserAddress("address", "city", "state", "country", "zip")
                );
            var userRepositoryMock = new Mock<IUserRepository>();
            var userServices = new UserServices(userRepositoryMock.Object);


            var userIdToDelete = userMock.Id;
            userRepositoryMock.Setup(userRepository => userRepository.GetByIdAsync(userIdToDelete)).ReturnsAsync(userMock);
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => userServices.DeleteUser(Guid.NewGuid()));


            exception.Message.ShouldBe("User not found!");
            userRepositoryMock.Verify(userRepository => userRepository.DeleteAsync(userIdToDelete), Times.Never);
        }
    }
}
