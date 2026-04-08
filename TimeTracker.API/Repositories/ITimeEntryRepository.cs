namespace TimeTracker.API.Repositories;

public interface ITimeEntryRepository
{
    
    Task<TimeEntry?> GetTimeEntryById(int id);
    Task<List<TimeEntry>> GetAllTimeEntries();
    Task<List<TimeEntry>> GetTimeEntries(int skip, int limit);
    Task<int> GetTimeEntriesCount();
    Task<List<TimeEntry>> CreateTimeEntry(TimeEntry timeEntry);
    Task<List<TimeEntry>> UpdateTimeEntry(int id, TimeEntry timeEntry);
    Task<List<TimeEntry>?> DeleteTimeEntry(int id);
    Task<List<TimeEntry>> GetTimeEntriesByProject(int projectId);
    
    // TimeEntry? GetTimeEntryById(int id);
    // List<TimeEntry> GetAllTimeEntries();
    // List<TimeEntry> CreateTimeEntry(TimeEntry timeEntry);
    // List<TimeEntry>? UpdateTimeEntry(int id, TimeEntry timeEntry);
    // List<TimeEntry>? DeleteTimeEntry(int id);
}