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
        Task<UserTask> AddTaskAsync(UserTask task);
        Task<UserTask> UpdateTaskAsync(UserTask task);
        Task DeleteTaskAsync(string id);
        Task<IEnumerable<UserTask>> GetTasksByUserIdAsync(string userId);
        Task<IEnumerable<UserTask>> SearchTasksAsync(string userId, string searchTerm);
        Task<IEnumerable<UserTask>> SortTasksAsync(string userId, string sortBy);
        Task<UserTask> SetTaskPriorityAsync(string taskId, TaskPriority priority);
        Task<UserTask> SetTaskDueDateAsync(string taskId, DateTime dueDate);
        Task<IEnumerable<UserTask>> GetTasksByDueDateAsync(string userId, DateTime dueDate);
        Task SendRemindersAsync();
    }
}
