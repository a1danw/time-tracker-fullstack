using Microsoft.AspNetCore.Mvc;
using TimeTracker.Shared.Models.Project;

namespace TimeTracker.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TimeEntryController : ControllerBase
{
    private readonly ITimeEntryService _timeEntryService;
    
    public TimeEntryController(ITimeEntryService timeEntryService)
    {
        _timeEntryService = timeEntryService;
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

    [HttpGet]
    public async Task<ActionResult<List<TimeEntryResponse>>> GetAllTimeEntries()
    {
        // mapping done in service layer
        return Ok(await _timeEntryService.GetAllTimeEntries());
    }

    [HttpGet("project/{projectId}")]
    public async Task<ActionResult<List<TimeEntryResponse>>> GetTimeEntriesByProject(int projectId)
    {
        return Ok(await _timeEntryService.GetTimeEntriesByProject(projectId));
    }

    /* public ActionResult<List<TimeEntry>> GetAllTimeEntries()
    {
        return Ok(_timeEntries.GetAllTimeEntries());
    } */

    /* public ActionResult<List<TimeEntry>> CreateTimeEntry(TimeEntry timeEntry)
    {
        _timeEntries.Add(timeEntry);
        return Ok(_timeEntries);
    } */

    [HttpPost]
    public async Task<ActionResult<List<TimeEntryResponse>>> CreateTimeEntry(TimeEntryCreateRequest timeEntry)
    {
        return Ok(await _timeEntryService.CreateTimeEntry(timeEntry));
    }

    /* public ActionResult<List<TimeEntryResponse>> GetAllTimeEntries()
    {
        return Ok(await _timeEntryService.GetAllTimeEntries());
    } */

    [HttpGet("{id}")]
    public async Task<ActionResult<TimeEntryResponse>> GetTimeEntryById(int id)
    {
        var result = await _timeEntryService.GetTimeEntryById(id);
        if (result is null)
        {
            return NotFound("TimeEntry with the given ID was not found.");
        }
        return Ok(result);
    } 
    
    /* public ActionResult<TimeEntryResponse> GetTimeEntryById(int id)
    {
        var result = _timeEntryService.GetTimeEntryById(id);
        if (result is null)
        {
            return NotFound("TimeEntry with the given ID was not found.");
        }
        return Ok(result);
    } */
  
    /* public async ActionResult<List<TimeEntryResponse>> UpdateTimeEntry(int id, TimeEntryUpdateRequest timeEntry)
    {
        var result = await _timeEntryService.UpdateTimeEntry(id, timeEntry);
        if (result is null)
        {
            return NotFound("TimeEntry with the given ID was not found.");
        }
        return Ok(result);
    } */

    [HttpPut("{id}")]
    public async Task<ActionResult<List<TimeEntryResponse>>> UpdateTimeEntry(int id, TimeEntryUpdateRequest timeEntry)
    {
        var result = await _timeEntryService.UpdateTimeEntry(id, timeEntry);
        if (result is null)
        {
            return NotFound("TimeEntry with the given ID was not found.");
        }
        return Ok(result);
    }
     
    [HttpDelete("{id}")]
    public async Task<ActionResult<List<TimeEntryResponse>>> DeleteTimeEntry(int id)

    {
        var result = await _timeEntryService.DeleteTimeEntry(id);
        if (result is null)
        {
            return NotFound("TimeEntry with the given ID was not found.");
        }
        return Ok(result);
    }
}