using System.Threading.Tasks;
using TaskManagementApp.Domain.Entities;

namespace TaskManagementApp.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<User> RegisterUser(string username, string password);
        Task<User> LoginUser(string username, string password);
        string GenerateToken(User user);
    }
}
