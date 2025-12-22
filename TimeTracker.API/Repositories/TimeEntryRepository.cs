namespace TimeTracker.API.Repositories;

public class TimeEntryRepository : ITimeEntryRepository
{
    private readonly DataContext _context;

    public TimeEntryRepository(DataContext context)
    {
        _context = context;
    }

    /* private static List<TimeEntry> _timeEntries = new List<TimeEntry>
    {
        new TimeEntry
        {
            Id = 1,
            Project = "Time Tracker App",
            End = DateTime.Now.AddHours(1)
        }
    }; */

    /* public List<TimeEntry> GetAllTimeEntries()
    {
        return _timeEntries;
    } */

    public async Task<List<TimeEntry>> GetAllTimeEntries()
    {
        // return await _context.TimeEntries.ToListAsync();
        // Includes ensures the Project property is loaded to prevent null values in TimeEntries
        return await _context.TimeEntries.ToListAsync();
        // .Include(te => te.Project).ToListAsync(); - replaced with AutoInclude override inside DataContext to remove explicit statement. Bear in mind we probably wont always want to auto include related entities.
        // .ThenInclude(p => p.ProjectDetails).ToListAsync(); // ensures ProjectDetails inside the Project isnt null
    }
    
    /* public async Task<TimeEntry?> GetTimeEntryById(int id)
    {
        var timeEntry = await _context.TimeEntries.FindAsync(id);
        return timeEntry;
    } */

    /* public async Task<TimeEntry?> GetTimeEntryById(int id)
    {
        return _timeEntries.FirstOrDefault(t => t.Id == id);
    } */

    public async Task<TimeEntry?> GetTimeEntryById(int id)
    {
        var timeEntry = await _context.TimeEntries.FindAsync(id);
        return timeEntry;
        // .Include(te => te.Project)
        // .FirstOrDefaultAsync(te => te.Id == id);
    }

    /* public List<TimeEntry> CreateTimeEntry(TimeEntry timeEntry)
    {
        _timeEntries.Add(timeEntry);
        return _timeEntries;
    } */

    public async Task<List<TimeEntry>> GetTimeEntriesByProject(int projectId)
    {
        return await _context.TimeEntries.Where(te => te.ProjectId == projectId).ToListAsync();
    }

    public async Task<List<TimeEntry>> CreateTimeEntry(TimeEntry timeEntry)
    {
        _context.TimeEntries.Add(timeEntry);
        await _context.SaveChangesAsync();

        return await GetAllTimeEntries();
        // return await _context.TimeEntries.ToListAsync();
    }

    /* public async Task<List<TimeEntry>> UpdateTimeEntry(int id, TimeEntry timeEntry)
    {
        var entryToUpdateIndex = _timeEntries.FindIndex(t => t.Id == id);
        if (entryToUpdateIndex == -1)
        {
            return null;
        }
        _timeEntries[entryToUpdateIndex] = timeEntry;
        return _timeEntries;
    } */

    public async Task<List<TimeEntry>> UpdateTimeEntry(int id, TimeEntry timeEntry)
    {
        var dbTimeEntry = await _context.TimeEntries.FindAsync(id);
        if (dbTimeEntry is null)
        {
            throw new EntityNotFoundException($"Entity with ID {id} was not found.");
        }

        dbTimeEntry.ProjectId = timeEntry.ProjectId;
        dbTimeEntry.Start = timeEntry.Start;
        dbTimeEntry.End = timeEntry.End;
        dbTimeEntry.DateUpdated = DateTime.Now;

        await _context.SaveChangesAsync();

        return await GetAllTimeEntries();

        /* var entryToUpdateIndex = _timeEntries.FindIndex(t => t.Id == id);
        if (entryToUpdateIndex == -1) // _timeEntries[entryToUpdateIndex] == null
        {
            return null;
        }
        _timeEntries[entryToUpdateIndex] = timeEntry; 
    } */
    }


    /* public List<TimeEntry>? DeleteTimeEntry(int id)
    {
        var entryToDelete = _timeEntries.FirstOrDefault(t => t.Id == id);
        if (entryToDelete == null)
        {
            return null;
        }
        _timeEntries.Remove(entryToDelete);
        return _timeEntries;
    } */

    public async Task<List<TimeEntry>?> DeleteTimeEntry(int id)
    {
        var dbTimeEntry = await _context.TimeEntries.FindAsync(id);
        if (dbTimeEntry is null)
        {
            return null;
        }

        _context.TimeEntries.Remove(dbTimeEntry);
        await _context.SaveChangesAsync();

        return await GetAllTimeEntries();
    }
}