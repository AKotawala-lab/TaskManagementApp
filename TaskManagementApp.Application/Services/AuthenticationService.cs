using System;
using System.Threading.Tasks;
using TaskManagementApp.Application.Interfaces;
using TaskManagementApp.Domain.Entities;

namespace TaskManagementApp.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;

        public AuthenticationService(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<User> RegisterUserAsync(string username, string email, string password, string avatarUrl)
        {
            var existingUser = await _userRepository.GetUserByUsernameAsync(username);
            if (existingUser != null)
            {
                throw new Exception("Username already exists");
            }

            var hashedPassword = _passwordHasher.HashPassword(password);
             var newUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = username,
                Email = email,
                Password = hashedPassword,
                AvatarUrl = avatarUrl
            };

            await _userRepository.AddUserAsync(newUser);
            return newUser;
        }

        public async Task<User> LoginUserAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null || !_passwordHasher.VerifyPassword(user.Password, password))
            {
                throw new Exception("Invalid username or password");
            }
            return user;
        }

        public string GenerateToken(User user)
        {
            return _tokenService.GenerateToken(user);
        }
    }
}
