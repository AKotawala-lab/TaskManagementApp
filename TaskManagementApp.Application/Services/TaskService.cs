using TaskManagementApp.Application.Interfaces;
using TaskManagementApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManagementApp.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<UserTask> GetTaskByIdAsync(string id)
        {
            return await _taskRepository.GetTaskByIdAsync(id);
        }

        public async Task<IEnumerable<UserTask>> GetAllTasksAsync()
        {
            return await _taskRepository.GetAllTasksAsync();
        }

        public async Task AddTaskAsync(UserTask task)
        {
            await _taskRepository.AddTaskAsync(task);
        }

        public async Task UpdateTaskAsync(UserTask task)
        {
            await _taskRepository.UpdateTaskAsync(task);
        }

        public async Task DeleteTaskAsync(string id)
        {
            await _taskRepository.DeleteTaskAsync(id);
        }

        public async Task<IEnumerable<UserTask>> GetTasksByUserIdAsync(string userId)
        {
            return await _taskRepository.GetTasksByUserIdAsync(userId);
        }

        public async Task<IEnumerable<UserTask>> SearchTasksAsync(string userId, string searchTerm)
        {
            return await _taskRepository.SearchTasksAsync(userId, searchTerm);
        }

        public async Task<IEnumerable<UserTask>> SortTasksAsync(string userId, string sortBy)
        {
            return await _taskRepository.SortTasksAsync(userId, sortBy);
        }
    }
}
