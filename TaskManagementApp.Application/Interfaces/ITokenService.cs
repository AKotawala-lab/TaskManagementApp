using TaskManagementApp.Domain.Entities;

namespace TaskManagementApp.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
