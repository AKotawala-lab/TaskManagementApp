using TaskManagementApp.Application.Interfaces;
using TaskManagementApp.Domain.Entities;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace TaskManagementApp.Infrastructure.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly ConcurrentDictionary<string, User> _users = new();

        public Task<User> AddUserAsync(User user)
        {
            _users[user.Username] = user;
            return Task.FromResult(user);
        }

        public Task<User> GetUserByUsernameAsync(string username)
        {
            _users.TryGetValue(username, out var user);
            return Task.FromResult(user);
        }
    }
}
