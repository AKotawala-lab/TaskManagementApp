using TaskManagementApp.Application.Interfaces;
using TaskManagementApp.Application.Services;
using TaskManagementApp.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TaskManagementApp.Application.Tests.Services
{
    public class TaskServiceTests
    {
        private readonly TaskService _taskService;
        private readonly Mock<ITaskRepository> _taskRepositoryMock;
        private readonly Mock<DbSet<UserTask>> _mockTaskSet;
        private readonly Mock<IApplicationDbContext> _mockContext;

        public TaskServiceTests()
        {
            _mockTaskSet = new Mock<DbSet<UserTask>>();
            _mockContext = new Mock<IApplicationDbContext>();
            _mockContext.Setup(m => m.Tasks).Returns(_mockTaskSet.Object);
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _taskService = new TaskService(_taskRepositoryMock.Object);
        }

        private List<UserTask> GetTestTasksList()
        {
            return new List<UserTask>
            {
                new UserTask { Id = Guid.NewGuid().ToString(), UserId = "user1", Title = "Task 1", Description = "Description 1", Priority = TaskPriority.Low, DueDate = DateTime.Now.AddDays(1), CreatedAt = DateTime.Now },
                new UserTask { Id = Guid.NewGuid().ToString(), UserId = "user1", Title = "Task 2", Description = "Description 2", Priority = TaskPriority.Medium, DueDate = DateTime.Now.AddDays(2), CreatedAt = DateTime.Now },
                new UserTask { Id = Guid.NewGuid().ToString(), UserId = "user2", Title = "Task 3", Description = "Description 3", Priority = TaskPriority.High, DueDate = DateTime.Now.AddDays(3), CreatedAt = DateTime.Now }
            };
        } 

        private IQueryable<UserTask> GetTestTasks()
        {
            return new List<UserTask>
            {
                new UserTask { Id = Guid.NewGuid().ToString(), UserId = "user1", Title = "Task 1", Description = "Description 1", Priority = TaskPriority.Low, DueDate = DateTime.Now.AddDays(1), CreatedAt = DateTime.Now },
                new UserTask { Id = Guid.NewGuid().ToString(), UserId = "user1", Title = "Task 2", Description = "Description 2", Priority = TaskPriority.Medium, DueDate = DateTime.Now.AddDays(2), CreatedAt = DateTime.Now },
                new UserTask { Id = Guid.NewGuid().ToString(), UserId = "user2", Title = "Task 3", Description = "Description 3", Priority = TaskPriority.High, DueDate = DateTime.Now.AddDays(3), CreatedAt = DateTime.Now }
            }.AsQueryable();
        }      

        private void SetUpMockTaskSet(IQueryable<UserTask> testTasks)
        {
            _mockTaskSet.As<IQueryable<UserTask>>().Setup(m => m.Provider).Returns(testTasks.Provider);
            _mockTaskSet.As<IQueryable<UserTask>>().Setup(m => m.Expression).Returns(testTasks.Expression);
            _mockTaskSet.As<IQueryable<UserTask>>().Setup(m => m.ElementType).Returns(testTasks.ElementType);
            _mockTaskSet.As<IQueryable<UserTask>>().Setup(m => m.GetEnumerator()).Returns(testTasks.GetEnumerator());
        }          

        [Fact]
        public async Task GetTaskByIdAsync_ShouldReturnTask_WhenTaskExists()
        {
            var taskId = Guid.NewGuid().ToString();
            var task = new UserTask { Id = taskId, Title = "Test Task", Description = "Test Description", Priority = TaskPriority.Low, DueDate = DateTime.Now.AddDays(1), CreatedAt = DateTime.Now };

            _taskRepositoryMock.Setup(repo => repo.GetTaskByIdAsync(taskId)).ReturnsAsync(task);

            var result = await _taskService.GetTaskByIdAsync(taskId);

            result.Should().BeEquivalentTo(task);
        }

        [Fact]
        public async Task GetAllTasksAsync_ShouldReturnAllTasks()
        {
            var tasks = new List<UserTask>
            {
                new UserTask { Id = Guid.NewGuid().ToString(), Title = "Test Task 1", Description = "Test Description 1", Priority = TaskPriority.Low, DueDate = DateTime.Now.AddDays(1), CreatedAt = DateTime.Now },
                new UserTask { Id = Guid.NewGuid().ToString(), Title = "Test Task 2", Description = "Test Description 2", Priority = TaskPriority.Low, DueDate = DateTime.Now.AddDays(1), CreatedAt = DateTime.Now }
            };

            _taskRepositoryMock.Setup(repo => repo.GetAllTasksAsync()).ReturnsAsync(tasks);

            var result = await _taskService.GetAllTasksAsync();

            result.Should().BeEquivalentTo(tasks);
        }

        [Fact]
        public async Task AddTaskAsync_ShouldAddTask()
        {
            var task = new UserTask { Id = Guid.NewGuid().ToString(), Title = "Test Task", Description = "Test Description", Priority = TaskPriority.Low, DueDate = DateTime.Now.AddDays(1), CreatedAt = DateTime.Now };

            await _taskService.AddTaskAsync(task);

            _taskRepositoryMock.Verify(repo => repo.AddTaskAsync(task), Times.Once);
        }

        [Fact]
        public async Task UpdateTaskAsync_ShouldUpdateTask()
        {
            var task = new UserTask { Id = Guid.NewGuid().ToString(), Title = "Test Task", Description = "Test Description", Priority = TaskPriority.Low, DueDate = DateTime.Now.AddDays(1), CreatedAt = DateTime.Now };

            await _taskService.UpdateTaskAsync(task);

            _taskRepositoryMock.Verify(repo => repo.UpdateTaskAsync(task), Times.Once);
        }

        [Fact]
        public async Task DeleteTaskAsync_ShouldDeleteTask()
        {
            var taskId = Guid.NewGuid().ToString();

            await _taskService.DeleteTaskAsync(taskId);

            _taskRepositoryMock.Verify(repo => repo.DeleteTaskAsync(taskId), Times.Once);
        }

        [Fact]
        public async Task GetTasksByUserIdAsync_ShouldReturnTasksForGivenUserId()
        {
            // Arrange
            var userId = "user1";
            var testTasks = GetTestTasksList();
            //SetUpMockTaskSet(testTasks);
            _taskRepositoryMock.Setup(repo => repo.GetTasksByUserIdAsync(userId)).ReturnsAsync(testTasks.Where(t => t.UserId == userId));

            // Act
            var result = await _taskService.GetTasksByUserIdAsync(userId);
            

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, t => Assert.Equal(userId, t.UserId));
        }

        [Fact]
        public async Task SearchTasksAsync_ShouldReturnTasksThatMatchSearchCriteria()
        {
            // Arrange
            var userId = "user1";
            var searchTerm = "Task 1";
            var testTasks = GetTestTasksList();
            //SetUpMockTaskSet(testTasks);
            _taskRepositoryMock.Setup(repo => repo.SearchTasksAsync(userId, searchTerm))
                               .ReturnsAsync(testTasks.Where(t => t.UserId == userId && (t.Title.Contains(searchTerm) || t.Description.Contains(searchTerm))));

            // Act
            var result = await _taskService.SearchTasksAsync(userId, searchTerm);

            // Assert
            Assert.Equal(1, result.Count());
            Assert.Contains(result, t => (t.Title.Contains(searchTerm) || t.Description.Contains(searchTerm)));
        }

        [Fact]
        public async Task SortTasksAsync_ShouldReturnTasksSortedByPriority()
        {
            // Arrange
            var userId = "user1";
            var sortBy = "priority";
            var testTasks = GetTestTasksList();
            //SetUpMockTaskSet(testTasks);
            var tasksQuery = testTasks.Where(t => t.UserId == userId);

            tasksQuery = sortBy switch
            {
                "priority" => tasksQuery.OrderBy(t => t.Priority),
                "dueDate" => tasksQuery.OrderBy(t => t.DueDate),
                _ => tasksQuery.OrderBy(t => t.CreatedAt)
            };

            _taskRepositoryMock.Setup(repo => repo.SortTasksAsync(userId, sortBy)).ReturnsAsync(tasksQuery);

            // Act
            var result = await _taskService.SortTasksAsync(userId, sortBy);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("Task 1", result.First().Title);
            Assert.Equal("Task 2", result.Last().Title);
        }

        [Fact]
        public async Task SortTasksAsync_ShouldReturnTasksSortedByDueDate()
        {
            // Arrange
            var userId = "user1";
            var sortBy = "dueDate";
            var testTasks = GetTestTasksList();
            //SetUpMockTaskSet(testTasks);
            var tasksQuery = testTasks.Where(t => t.UserId == userId);

            tasksQuery = sortBy switch
            {
                "priority" => tasksQuery.OrderBy(t => t.Priority),
                "dueDate" => tasksQuery.OrderBy(t => t.DueDate),
                _ => tasksQuery.OrderBy(t => t.CreatedAt)
            };

            _taskRepositoryMock.Setup(repo => repo.SortTasksAsync(userId, sortBy)).ReturnsAsync(tasksQuery);

            // Act
            var result = await _taskService.SortTasksAsync(userId, sortBy);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("Task 1", result.First().Title);
            Assert.Equal("Task 2", result.Last().Title);
        }

        [Fact]
        public async Task SortTasksAsync_ShouldReturnTasksSortedByCreatedAt()
        {
            // Arrange
            var userId = "user1";
            var sortBy = "createdAt";
            var testTasks = GetTestTasks();
            //SetUpMockTaskSet(testTasks);
            var tasksQuery = testTasks.Where(t => t.UserId == userId);

            tasksQuery = sortBy switch
            {
                "priority" => tasksQuery.OrderBy(t => t.Priority),
                "dueDate" => tasksQuery.OrderBy(t => t.DueDate),
                _ => tasksQuery.OrderBy(t => t.CreatedAt)
            };

            _taskRepositoryMock.Setup(repo => repo.SortTasksAsync(userId, sortBy)).ReturnsAsync(tasksQuery);

            // Act
            var result = await _taskService.SortTasksAsync(userId, sortBy);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("Task 1", result.First().Title);
            Assert.Equal("Task 2", result.Last().Title);
        }
    }
}
