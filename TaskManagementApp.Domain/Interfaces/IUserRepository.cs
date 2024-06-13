using System.Threading.Tasks;
using TaskManagementApp.Domain.Entities;

namespace TaskManagementApp.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AddUserAsync(User user);
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(string id);
    }

}
