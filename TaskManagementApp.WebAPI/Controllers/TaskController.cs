using Microsoft.AspNetCore.Mvc;
using TaskManagementApp.Application.Interfaces;
using TaskManagementApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace TaskManagementApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(string id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> AddTask([FromBody] UserTask task)
        {
            var addedTask = await _taskService.AddTaskAsync(task);
            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(string id, [FromBody] UserTask task)
        {
            if (id != task.Id)
                return BadRequest();

            var updatedTask = await _taskService.UpdateTaskAsync(task);
            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(string id)
        {
            await _taskService.DeleteTaskAsync(id);
            return NoContent();
        }

        [HttpGet("{userId}/sort")]
        public async Task<IActionResult> SortTasks(string userId, [FromQuery] string sortBy)
        {
            var tasks = await _taskService.SortTasksAsync(userId, sortBy);
            return Ok(tasks);
        }

        [HttpGet("{userId}/search")]
        public async Task<IActionResult> SearchTasks(string userId, [FromQuery] string searchTerm)
        {
            var tasks = await _taskService.SearchTasksAsync(userId, searchTerm);
            return Ok(tasks);
        }

        [HttpPut("{taskId}/priority")]
        public async Task<IActionResult> SetTaskPriority(string taskId, [FromBody] TaskPriority priority)
        {
            try
            {
                var task = await _taskService.SetTaskPriorityAsync(taskId, priority);
                return Ok(task);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{taskId}/duedate")]
        public async Task<IActionResult> SetTaskDueDate(string taskId, [FromBody] DateTime dueDate)
        {
            try
            {
                var task = await _taskService.SetTaskDueDateAsync(taskId, dueDate);
                return Ok(task);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{userId}/duedate")]
        public async Task<IActionResult> GetTasksByDueDate(string userId, [FromQuery] DateTime dueDate)
        {
            var tasks = await _taskService.GetTasksByDueDateAsync(userId, dueDate);
            return Ok(tasks);
        }
    }
}
