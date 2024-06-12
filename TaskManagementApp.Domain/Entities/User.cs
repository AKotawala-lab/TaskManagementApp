namespace TaskManagementApp.Domain.Entities
{
    public class User
    {
        public string Id { get; set; } // Using string for flexibility and uniqueness
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string AvatarUrl { get; set; }

        // Navigation property for related tasks
        public ICollection<UserTask> Tasks { get; set; } = new List<UserTask>();
    }

}
