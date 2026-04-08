using TimeTracker.API.Services;

namespace TimeTracker.API.Repositories;

public class TimeEntryRepository : ITimeEntryRepository
{
    private readonly DataContext _context;
    private readonly IUserContextService _userContextService;

    public TimeEntryRepository(DataContext context, IUserContextService userContextService)
    {
        _context = context;
        _userContextService = userContextService;
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
        var userId = _userContextService.GetUserId();
        if(userId == null)
            return new List<TimeEntry>();
        // return await _context.TimeEntries.ToListAsync();
        // Includes ensures the Project property is loaded to prevent null values in TimeEntries
        return await _context.TimeEntries.Where(t => t.User.Id == userId).ToListAsync();
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
        var userId = _userContextService.GetUserId();
        if (userId == null)
        {
            return null;
        }

        var timeEntry = await _context.TimeEntries.FirstOrDefaultAsync(t => t.Id == id && t.User.Id == userId);
        // var timeEntry = await _context.TimeEntries.FindAsync(id);
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
        var userId = _userContextService.GetUserId();
        if (userId == null)
        {
             throw new EntityNotFoundException("User was not found.");
        }
        
        return await _context.TimeEntries
            .Where(te => te.ProjectId == projectId && te.User.Id == userId)
            .ToListAsync();
    }

    public async Task<List<TimeEntry>> CreateTimeEntry(TimeEntry timeEntry)
    {
        var user = await _userContextService.GetUserAsync();
        if(user == null)
        {
            throw new EntityNotFoundException("User was not found.");
        }
        timeEntry.User = user;
            
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
        var userId = _userContextService.GetUserId();
        if (userId == null)
        {
             throw new EntityNotFoundException("User was not found.");
        }

        // var dbTimeEntry = await _context.TimeEntries.FindAsync(id);
        var dbTimeEntry = await _context.TimeEntries.FirstOrDefaultAsync(t => t.Id == id && t.User.Id == userId);
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
        var userId = _userContextService.GetUserId();
        if (userId == null)
        {
            return null;
        }
        var dbTimeEntry = await _context.TimeEntries.FirstOrDefaultAsync(t => t.Id == id && t.User.Id == userId);
        // var dbTimeEntry = await _context.TimeEntries.FindAsync(id);
        if (dbTimeEntry is null)
        {
            return null;
        }

        _context.TimeEntries.Remove(dbTimeEntry);
        await _context.SaveChangesAsync();

        return await GetAllTimeEntries();
    }

    public async Task<List<TimeEntry>> GetTimeEntries(int skip, int limit)
    {
        // example - items 1-10 skip 0, limit 10
        return await _context.TimeEntries.Skip(skip).Take(limit).ToListAsync();
    }

    public async Task<int> GetTimeEntriesCount()
    {
        return await _context.TimeEntries.CountAsync();
    }
}