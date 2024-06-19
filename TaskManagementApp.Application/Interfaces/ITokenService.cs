using TaskManagementApp.Application.Models;
using TaskManagementApp.Domain.Entities;

namespace TaskManagementApp.Application.Interfaces
{
    public interface ITokenService
    {
        AuthResponse GenerateToken(User user);
    }
}
