using Moq;
using TaskManagementApp.Application.Interfaces;
using TaskManagementApp.Application.Services;
using TaskManagementApp.Domain.Entities;
using Xunit;
using FluentAssertions;

namespace TaskManagementApp.Application.Tests.Services
{
    public class UserManagementServiceTests
    {
        private readonly UserManagementService _userManagementService;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IAuthenticationService> _authenticationServiceMock;

        public UserManagementServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _authenticationServiceMock = new Mock<IAuthenticationService>();
            _userManagementService = new UserManagementService(_userRepositoryMock.Object);
        }

        /*[Fact]
        public async Task RegisterUserAsync_ShouldReturnUser_WhenRegistrationIsSuccessful()
        {
            // Arrange
            var username = "testuser";
            var password = "password123";
            var email = "testemail@test.com"
            var user = new User { Id = Guid.NewGuid().ToString(), Username = username, Password = password, Email = email };

            _userRepositoryMock.Setup(repo => repo.AddUserAsync(It.IsAny<User>())).ReturnsAsync(user);

            // Act
            var result = await _authenticationServiceMock.RegisterUserAsync(username, password);

            // Assert
            result.Should().BeEquivalentTo(user);
        }

        [Fact]
        public async Task AuthenticateUserAsync_ShouldReturnUser_WhenCredentialsAreValid()
        {
            // Arrange
            var username = "testuser";
            var password = "password123";
            var user = new User { Username = username, Password = password };

            _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(It.IsAny<string>())).ReturnsAsync(user);

            // Act
            var result = await _authenticationServiceMock.AuthenticateUserAsync(username, password);

            // Assert
            result.Should().BeEquivalentTo(user);
        }

        [Fact]
        public async Task AuthenticateUserAsync_ShouldReturnNull_WhenCredentialsAreInvalid()
        {
            // Arrange
            var username = "testuser";
            var password = "wrongpassword";
            var user = new User { Username = "testuser", Password = "password123" };

            _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(It.IsAny<string>())).ReturnsAsync(user);

            // Act
            var result = await _authenticationServiceMock.AuthenticateUserAsync(username, password);

            // Assert
            result.Should().BeNull();
        }*/
    }
}
