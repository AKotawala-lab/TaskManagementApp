using TaskManagementApp.Domain.Entities;

namespace TaskManagementApp.Application.Interfaces
{
    public interface IUserManagementService
    {
        Task<User> GetUserByIdAsync(string id);
        Task UpdateUserProfileAsync(string id, string userName, string email, string avatarUrl);
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByEmailAsync(string email);
    }
}
