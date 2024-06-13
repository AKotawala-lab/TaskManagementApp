using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using TaskManagementApp.Application.Interfaces;
using TaskManagementApp.Application.Services;
using TaskManagementApp.Domain.Entities;
using Xunit;

namespace TaskManagementApp.Application.Tests.Services
{
    public class AuthenticationServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly AuthenticationService _authenticationService;

        public AuthenticationServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _tokenServiceMock = new Mock<ITokenService>();
            _authenticationService = new AuthenticationService(
                _userRepositoryMock.Object,
                _passwordHasherMock.Object,
                _tokenServiceMock.Object
            );
        }

        /*[Fact]
        public async Task RegisterUser_ShouldReturnUser_WhenRegistrationIsSuccessful()
        {
            // Arrange
            var newUser = new User { Username = "testuser", Password = "password123" };
            _userRepositoryMock.Setup(repo => repo.AddUserAsync(It.IsAny<User>())).ReturnsAsync(newUser);

            // Act
            var result = await _authenticationService.RegisterUser(newUser.Username, newUser.Password);

            // Assert
            result.Should().BeEquivalentTo(newUser);
            _userRepositoryMock.Verify(repo => repo.AddUserAsync(It.IsAny<User>()), Times.Once);
        }*/

        [Fact]
        public async Task RegisterUser_ShouldThrowException_WhenUsernameAlreadyExists()
        {
            // Arrange
            var newUser = new User { Username = "existinguser", Password = "password123", Email = "existinguser@test.com", AvatarUrl = "" };
            _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(It.IsAny<string>())).ReturnsAsync(newUser);

            // Act
            Func<Task> act = async () => { await _authenticationService.RegisterUserAsync(newUser.Username, newUser.Password, newUser.Email, newUser.AvatarUrl); };

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Username already exists");
            _userRepositoryMock.Verify(repo => repo.AddUserAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task LoginUser_ShouldReturnUser_WhenLoginIsSuccessful()
        {
            // Arrange
            var existingUser = new User { Username = "testuser", Password = "hashedpassword" };
            _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(It.IsAny<string>())).ReturnsAsync(existingUser);
            _passwordHasherMock.Setup(hasher => hasher.VerifyPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            // Act
            var result = await _authenticationService.LoginUserAsync(existingUser.Username, "password123");

            // Assert
            result.Should().BeEquivalentTo(existingUser);
            _userRepositoryMock.Verify(repo => repo.GetUserByUsernameAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task LoginUser_ShouldThrowException_WhenLoginFails()
        {
            // Arrange
            var existingUser = new User { Username = "testuser", Password = "hashedpassword" };
            _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(It.IsAny<string>())).ReturnsAsync(existingUser);
            _passwordHasherMock.Setup(hasher => hasher.VerifyPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            // Act
            Func<Task> act = async () => { await _authenticationService.LoginUserAsync(existingUser.Username, "wrongpassword"); };

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Invalid username or password");
            _userRepositoryMock.Verify(repo => repo.GetUserByUsernameAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GenerateToken_ShouldReturnToken_WhenUserIsValid()
        {
            // Arrange
            var user = new User { Username = "testuser", Id = Guid.NewGuid().ToString() };
            _tokenServiceMock.Setup(ts => ts.GenerateToken(user)).Returns("valid_token");

            // Act
            var token = _authenticationService.GenerateToken(user);

            // Assert
            token.Should().Be("valid_token");
        }

    }
}
