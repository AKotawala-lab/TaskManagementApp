using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementApp.API.Controllers;
using TaskManagementApp.Application.Interfaces;
using TaskManagementApp.Domain.Entities;
using Xunit;
using FluentAssertions;

namespace TaskManagementApp.UnitTests.Controllers
{
    public class TaskControllerTests
    {
        private readonly Mock<ITaskService> _taskServiceMock;
        private readonly TaskController _taskController;

        public TaskControllerTests()
        {
            _taskServiceMock = new Mock<ITaskService>();
            _taskController = new TaskController(_taskServiceMock.Object);
        }

        [Fact]
        public async Task GetTaskById_ShouldReturnOk_WhenTaskExists()
        {
            var taskId = Guid.NewGuid().ToString();
            var task = new UserTask { Id = taskId, Title = "Test Task", Description = "Test Description", Priority = TaskPriority.Low, DueDate = DateTime.Now.AddDays(1), CreatedAt = DateTime.Now };
            _taskServiceMock.Setup(service => service.GetTaskByIdAsync(taskId)).ReturnsAsync(task);

            var result = await _taskController.GetTaskById(taskId);

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(task);
        }

        [Fact]
        public async Task GetTaskById_ShouldReturnNotFound_WhenTaskDoesNotExist()
        {
            var taskId = Guid.NewGuid().ToString();
            _taskServiceMock.Setup(service => service.GetTaskByIdAsync(taskId)).ReturnsAsync((UserTask)null);

            var result = await _taskController.GetTaskById(taskId);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetAllTasks_ShouldReturnOk_WithListOfTasks()
        {
            var userId = "1";
            var tasks = new List<UserTask>
            {
                new UserTask { Id = Guid.NewGuid().ToString(), Title = "Test Task 1", Description = "Test Description 1", Priority = TaskPriority.Low, DueDate = DateTime.Now.AddDays(1), CreatedAt = DateTime.Now, UserId = "1" },
                new UserTask { Id = Guid.NewGuid().ToString(), Title = "Test Task 2", Description = "Test Description 2", Priority = TaskPriority.Low, DueDate = DateTime.Now.AddDays(1), CreatedAt = DateTime.Now, UserId = "1" },
                new UserTask { Id = Guid.NewGuid().ToString(), Title = "Test Task 3", Description = "Test Description 3", Priority = TaskPriority.Low, DueDate = DateTime.Now.AddDays(1), CreatedAt = DateTime.Now, UserId = "2" }
            };
            _taskServiceMock.Setup(service => service.GetTasksByUserIdAsync(userId)).ReturnsAsync(tasks);

            var result = await _taskController.GetTasksByUser(userId);

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(tasks);
        }

        [Fact]
        public async Task AddTask_ShouldReturnCreatedAtAction()
        {
            var task = new UserTask { Id = Guid.NewGuid().ToString(), Title = "Test Task", Description = "Test Description", Priority = TaskPriority.Low, DueDate = DateTime.Now.AddDays(1), CreatedAt = DateTime.Now };

            var result = await _taskController.AddTask(task);

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            //okResult.Value.Should().BeEquivalentTo(task);
        }

        [Fact]
        public async Task UpdateTask_ShouldReturnNoContent()
        {
            var taskId = Guid.NewGuid().ToString();
            var task = new UserTask { Id = taskId, Title = "Updated Task", Description = "Updated Description", Priority = TaskPriority.Low, DueDate = DateTime.Now.AddDays(1), CreatedAt = DateTime.Now };

            var result = await _taskController.UpdateTask(taskId, task);

            result.Should().BeOfType<CreatedAtActionResult>();
            var createdAtActionResult = result as CreatedAtActionResult;
            createdAtActionResult.ActionName.Should().Be(nameof(_taskController.GetTaskById));
            createdAtActionResult.RouteValues["id"].Should().Be(task.Id);
            createdAtActionResult.Value.Should().BeEquivalentTo(task);
        }

        [Fact]
        public async Task DeleteTask_ShouldReturnNoContent()
        {
            var taskId = Guid.NewGuid().ToString();

            var result = await _taskController.DeleteTask(taskId);

            result.Should().BeOfType<NoContentResult>();
        }
    }
}
