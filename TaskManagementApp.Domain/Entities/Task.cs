namespace TaskManagementApp.Domain.Entities
{
    public class UserTask
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? Reminder { get; set; }
        public TaskPriority Priority { get; set; }
        public string UserId { get; set; }

        public User? User { get; set; } // Navigation property
    }

    public enum TaskPriority
    {
        Low = 1,
        Medium = 2,
        High = 3
    }
}
