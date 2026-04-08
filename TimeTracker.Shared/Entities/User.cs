using Microsoft.AspNetCore.Identity;

namespace TimeTracker.Shared.Entities
{
    // IdentityUser already has properties like Id, UserName, PasswordHash, etc. that are needed for authentication and user management
    public class User : IdentityUser
    {
        // public int Id { get; set; }
        // public required string Name { get; set; }
        public List<Project> Projects { get; set; } = new List<Project>();
        public List<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();
    }
}
