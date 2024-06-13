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
            _users[user.Id] = user;
            return Task.FromResult(user);
        }

        public Task<User> GetUserByUsernameAsync(string username)
        {
            var user = _users.Values.FirstOrDefault(u => u.Username == username);
            return Task.FromResult(user);
        }

        public Task<User> GetUserByEmailAsync(string email)
        {
            var user = _users.Values.FirstOrDefault(u => u.Email == email);
            return Task.FromResult(user);
        }

        public Task<User> GetUserByIdAsync(string id)
        {
            _users.TryGetValue(id, out var user);
            return Task.FromResult(user);
        }
    }

}
