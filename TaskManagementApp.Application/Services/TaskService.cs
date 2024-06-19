using TaskManagementApp.Application.Interfaces;
using TaskManagementApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

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

        public async Task<UserTask> AddTaskAsync(UserTask task)
        {
            task.Id = Guid.NewGuid().ToString();
            task.CreatedAt = DateTime.Now;
            return await _taskRepository.AddTaskAsync(task);
        }

        public async Task<UserTask> UpdateTaskAsync(UserTask task)
        {
            return await _taskRepository.UpdateTaskAsync(task);
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

        public async Task<UserTask> SetTaskPriorityAsync(string taskId, TaskPriority priority)
        {
            var task = await _taskRepository.GetTaskByIdAsync(taskId) ?? throw new Exception("Task not found");
            task.Priority = priority;
            return await _taskRepository.UpdateTaskAsync(task);
        }

        public async Task<UserTask> SetTaskDueDateAsync(string taskId, DateTime dueDate)
        {
            var task = await _taskRepository.GetTaskByIdAsync(taskId) ?? throw new Exception("Task not found");
            task.DueDate = dueDate;
            return await _taskRepository.UpdateTaskAsync(task);
        }

        public async Task<IEnumerable<UserTask>> GetTasksByDueDateAsync(string userId, DateTime dueDate)
        {
            var tasks = await _taskRepository.GetTasksByUserIdAsync(userId);
            return tasks.Where(t => t.DueDate.Date == dueDate.Date);
        }

        // Simulate sending reminders
        public async Task SendRemindersAsync()
        {
            var tasks = await _taskRepository.GetAllTasksAsync();
            var tasksDueTomorrow = tasks.Where(t => t.DueDate.Date == DateTime.Now.AddDays(1).Date);

            foreach (var task in tasksDueTomorrow)
            {
                // Logic to send reminder (e.g., email, notification, etc.)
                Console.WriteLine($"Reminder: Task '{task.Title}' is due tomorrow.");
            }
        }
    }
}
