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

        public async Task<User> RegisterUser(string username, string password)
        {
            var existingUser = await _userRepository.GetUserByUsernameAsync(username);
            if (existingUser != null)
            {
                throw new Exception("Username already exists");
            }

            var hashedPassword = _passwordHasher.HashPassword(password);
            var newUser = new User { Username = username, Password = hashedPassword };
            await _userRepository.AddUserAsync(newUser);
            return newUser;
        }

        public async Task<User> LoginUser(string username, string password)
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
