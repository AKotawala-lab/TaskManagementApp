using TaskManagementApp.Domain.Entities;

namespace TaskManagementApp.Application.Interfaces
{
    public interface IUserManagementService
    {
        Task<User> RegisterUserAsync(string username, string password);
        Task<User> AuthenticateUserAsync(string username, string password);
    }
}
