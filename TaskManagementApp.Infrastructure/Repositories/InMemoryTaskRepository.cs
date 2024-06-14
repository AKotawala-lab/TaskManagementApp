using TaskManagementApp.Application.Interfaces;
using TaskManagementApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementApp.Infrastructure.Repositories
{
    public class InMemoryTaskRepository : ITaskRepository
    {
        private readonly List<UserTask> _tasks = new List<UserTask>();

        public async Task<UserTask> GetTaskByIdAsync(string id)
        {
            return await Task.FromResult(_tasks.FirstOrDefault(t => t.Id == id));
        }

        public async Task<IEnumerable<UserTask>> GetAllTasksAsync()
        {
            return await Task.FromResult(_tasks);
        }

        public async Task<UserTask> AddTaskAsync(UserTask task)
        {
            _tasks.Add(task);
            return await Task.FromResult(task);
        }

        public async Task<UserTask> UpdateTaskAsync(UserTask task)
        {
            var existingTask = _tasks.FirstOrDefault(t => t.Id == task.Id);
            if (existingTask != null)
            {
                existingTask.Title = task.Title;
                existingTask.Description = task.Description;
                existingTask.DueDate = task.DueDate;
                existingTask.Priority = task.Priority;
                existingTask.UserId = task.UserId;
            }
            return await Task.FromResult(existingTask);
        }

        public async Task DeleteTaskAsync(string id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                _tasks.Remove(task);
            }
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<UserTask>> GetTasksByUserIdAsync(string userId)
        {
            return await Task.FromResult(_tasks.Where(t => t.UserId == userId));
        }

        public async Task<IEnumerable<UserTask>> SearchTasksAsync(string userId, string searchTerm)
        {
            return await Task.FromResult(_tasks.Where(t => t.UserId == userId && 
                                     (t.Title.Contains(searchTerm) || t.Description.Contains(searchTerm))));
        }

        public async Task<IEnumerable<UserTask>> SortTasksAsync(string userId, string sortBy)
        {
            var tasksQuery = _tasks.Where(t => t.UserId == userId);

            tasksQuery = sortBy switch
            {
                "priority" => tasksQuery.OrderBy(t => t.Priority),
                "dueDate" => tasksQuery.OrderBy(t => t.DueDate),
                _ => tasksQuery.OrderBy(t => t.CreatedAt)
            };

            return await Task.FromResult(tasksQuery);
        }
    }
}
