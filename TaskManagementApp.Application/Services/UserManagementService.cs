using TaskManagementApp.Application.Interfaces;
using TaskManagementApp.Domain.Entities;
using System.Threading.Tasks;

namespace TaskManagementApp.Application.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserRepository _userRepository;

        public UserManagementService(IAuthenticationService authenticationService, IUserRepository userRepository)
        {
            _authenticationService = authenticationService;
            _userRepository = userRepository;
        }

        public async Task<User> RegisterUserAsync(string username, string password)
        {
            var user = new User
            {
                Username = username,
                Password = password
            };

            var registeredUser = await _userRepository.AddUserAsync(user);
            return registeredUser;
        }

        public async Task<User> AuthenticateUserAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);

            if (user != null && user.Password == password)
            {
                return user;
            }

            return null;
        }
    }
}
