using TaskManagementApp.Application.Interfaces;
using TaskManagementApp.Domain.Entities;
using System.Threading.Tasks;

namespace TaskManagementApp.Application.Services
{
    public class UserManagementService : IUserManagementService
    {

        private readonly IUserRepository _userRepository;

        public UserManagementService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task UpdateUserProfileAsync(string id, string username, string email, string avatarUrl)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            user.Username = username;
            user.Email = email;
            user.AvatarUrl = avatarUrl;

            await _userRepository.AddUserAsync(user);  // This should update the user in the repository
        }
    }
}
