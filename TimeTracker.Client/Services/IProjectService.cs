using TimeTracker.Shared.Models.Project;

namespace TimeTracker.Client.Services;

public interface IProjectService
{
    event Action? OnChange;
    List<ProjectResponse> Projects { get; set; }
    Task LoadAllProjects();
}
