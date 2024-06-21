// TaskRepository.cs
using TaskManagementApp.Application.Interfaces;
using TaskManagementApp.Domain.Entities;
using TaskManagementApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementApp.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserTask> GetTaskByIdAsync(string id)
        {
            return await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<UserTask>> GetAllTasksAsync()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task<UserTask> AddTaskAsync(UserTask task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<UserTask> UpdateTaskAsync(UserTask task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task DeleteTaskAsync(string id)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<UserTask>> GetTasksByUserIdAsync(string userId)
        {
            return await _context.Tasks.Where(t => t.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<UserTask>> SearchTasksAsync(string userId, string searchTerm)
        {
            return await _context.Tasks.Where(t => t.UserId == userId && 
                                    (t.Title.Contains(searchTerm) || t.Description.Contains(searchTerm)))
                                    .ToListAsync();
        }

        public async Task<IEnumerable<UserTask>> SortTasksAsync(string userId, string sortBy)
        {
            var tasksQuery = _context.Tasks.Where(t => t.UserId == userId);

            tasksQuery = sortBy switch
            {
                "priority" => tasksQuery.OrderBy(t => t.Priority),
                "dueDate" => tasksQuery.OrderBy(t => t.DueDate),
                _ => tasksQuery.OrderBy(t => t.CreatedAt)
            };

            return await tasksQuery.ToListAsync();
        }
    }
}
