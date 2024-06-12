using TaskManagementApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManagementApp.Application.Interfaces
{
    public interface ITaskService
    {
        Task<UserTask> GetTaskByIdAsync(string id);
        Task<IEnumerable<UserTask>> GetAllTasksAsync();
        Task AddTaskAsync(UserTask task);
        Task UpdateTaskAsync(UserTask task);
        Task DeleteTaskAsync(string id);
        Task<IEnumerable<UserTask>> GetTasksByUserIdAsync(string userId);
        Task<IEnumerable<UserTask>> SearchTasksAsync(string userId, string searchTerm);
        Task<IEnumerable<UserTask>> SortTasksAsync(string userId, string sortBy);
    }
}
