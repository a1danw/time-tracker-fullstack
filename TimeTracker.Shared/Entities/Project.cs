namespace TimeTracker.Shared.Entities
{
    // EF Core automatically maps primitive/scalar properties (int, string, bool, DateTime) as columns in the Projects table.
    // Navigation properties (List<> or related classes) are mapped to their own tables with foreign keys — no manual SQL needed.
    public class Project : SoftDeletableEntity
    {
        public required string Name { get; set; }
        public List<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();
        public ProjectDetails? ProjectDetails { get; set; }
        public List<User> Users { get; set; } = new List<User>();
    }
}
