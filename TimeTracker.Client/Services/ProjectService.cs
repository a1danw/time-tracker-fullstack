using System.Net.Http.Json;
using TimeTracker.Shared.Models.Project;

namespace TimeTracker.Client.Services;

// centralized state service for fetching instead of using http client instances
public class ProjectService : IProjectService
{
    private readonly HttpClient _http;
    public event Action? OnChange;
    public ProjectService(HttpClient http)
    {
        _http = http;
    }
    public List<ProjectResponse> Projects { get; set; } = new List<ProjectResponse>();
    public async Task LoadAllProjects()
    {
        var result = await _http.GetFromJsonAsync<List<ProjectResponse>>("api/project");
        if(result != null)
        {
            Projects = result;
            OnChange?.Invoke();
        }
    }
}
