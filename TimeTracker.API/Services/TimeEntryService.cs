using Mapster;
using TimeTracker.Shared.Models.Project;

namespace TimeTracker.API.Services;

public class TimeEntryService : ITimeEntryService
{
    private readonly ITimeEntryRepository _timeEntryRepo;

    public TimeEntryService(ITimeEntryRepository timeEntryRepo)
    {
        _timeEntryRepo = timeEntryRepo;
    }

    public async Task<List<TimeEntryResponse>> GetAllTimeEntries()
    {
        var result = await _timeEntryRepo.GetAllTimeEntries();
        return result.Adapt<List<TimeEntryResponse>>();
    } 

    /* public List<TimeEntryResponse> GetAllTimeEntries()
    {
        var result = _timeEntryRepo.GetAllTimeEntries();
        return result.Adapt<List<TimeEntryResponse>>();
    } */

    /* public async Task<List<TimeEntryResponse>> GetAllTimeEntries()
    {
        var result = await _timeEntryRepo.GetAllTimeEntries();
        return result.Adapt<List<TimeEntryResponse>>();

        // manual mapping (replaced with mapster, no config needed unlike auto mapper as it provides extensions):
        // return result.Select(t => new TimeEntryResponse
        // {
        //     Id = t.Id,
        //     Project = t.Project,
        //     Start = t.Start, 
        //     End = t.End
        // }).ToList();
    } */
    
    public async Task<TimeEntryResponse?> GetTimeEntryById(int id)
    {
        var result = await _timeEntryRepo.GetTimeEntryById(id);
        if (result is null)
        {
            return null;
        }
        return result.Adapt<TimeEntryResponse>();
    }

    public async Task<List<TimeEntryResponse>> GetTimeEntriesByProject(int projectId)
    {
        var result = await _timeEntryRepo.GetTimeEntriesByProject(projectId);
        return result.Adapt<List<TimeEntryResponse>>();
    }

    /*
        public TimeEntryResponse? GetTimeEntryById(int id)
    {
        var result = await _timeEntryRepo.GetTimeEntryById(id);
        if (result is null)
        {
            return null;
        }
        return result.Adapt<TimeEntryResponse>();
    }
     */

    /* public List<TimeEntryResponse> CreateTimeEntry(TimeEntryCreateRequest request)
    {
        var newEntry = request.Adapt<TimeEntry>();
        var result = _timeEntryRepo.CreateTimeEntry(newEntry);
        return result.Adapt<List<TimeEntryResponse>>();
    } */

    
    public async Task<List<TimeEntryResponse>> CreateTimeEntry(TimeEntryCreateRequest request)
    {
        // auto mapper
        var newEntry = request.Adapt<TimeEntry>(); // type to map the incoming data to
        var result = await _timeEntryRepo.CreateTimeEntry(newEntry);
        return result.Adapt<List<TimeEntryResponse>>();

        // manual mapping
        /* var newEntry = new TimeEntry
        {
            Project = timeEntry.Project,
            Start = timeEntry.Start,
            End = timeEntry.End
        };

        var result = _timeEntryRepo.CreateTimeEntry(newEntry);

        return result.Select(t => new TimeEntryResponse
        {
            Id = t.Id,
            Project = t.Project,
            Start = t.Start,
            End = t.End
        }).ToList(); */
    }

    /* public List<TimeEntryResponse>? UpdateTimeEntry(int id, TimeEntryUpdateRequest request)
    {
        var updatedEntry = request.Adapt<TimeEntry>();
        var result = _timeEntryRepo.UpdateTimeEntry(id, updatedEntry);
    } */

    public async Task<List<TimeEntryResponse>?> UpdateTimeEntry(int id, TimeEntryUpdateRequest request)
    {
        try
        {
            var updateEntry = request.Adapt<TimeEntry>();
            var result = await _timeEntryRepo.UpdateTimeEntry(id, updateEntry);
            return result.Adapt<List<TimeEntryResponse>>();
        }
        catch (EntityNotFoundException)
        {
            return null;
        }

        // var updateEntry = request.Adapt<TimeEntry>();
        // var result = await _timeEntryRepo.UpdateTimeEntry(id, updateEntry);
        // if (result is null)
        //  {
        //  return null;
        // }
    }
    /* public List<TimeEntryResponse>? DeleteTimeEntry(int id)
    {
        var result = _timeEntryRepo.DeleteTimeEntry(id);
        if (result is null)
        {
            return null;
        }

        return result.Adapt<List<TimeEntryResponse>>();
    } */

    
    public async Task<List<TimeEntryResponse>?> DeleteTimeEntry(int id)
    {
        var result = await _timeEntryRepo.DeleteTimeEntry(id);
        if (result is null)
        {
            return null;
        }

        return result.Adapt<List<TimeEntryResponse>>();
    }
} 