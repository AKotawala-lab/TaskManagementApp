using System.Threading.Tasks;
using TaskManagementApp.Domain.Entities;

namespace TaskManagementApp.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<User> RegisterUserAsync(string username, string email, string password, string avatarUrl);
        Task<User> LoginUserAsync(string username, string password);
        string GenerateToken(User user);
    }
}
