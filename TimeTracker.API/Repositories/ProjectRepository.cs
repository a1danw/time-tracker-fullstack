namespace TimeTracker.API.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly DataContext _context;
    private readonly IUserContextService _userContextService;

    public ProjectRepository(DataContext context, IUserContextService userContextService)
    {
        _context = context;
        _userContextService = userContextService;
    }

    public async Task<List<Project>> GetAllProjects()
    {
        var userId = _userContextService.GetUserId();
        if(userId == null)
        {
            return new List<Project>();
        }
        return await _context.Projects.Where(p => !p.IsDeleted && p.Users.Any(u => u.Id == userId)).ToListAsync(); 
    }
    
    public async Task<Project?> GetProjectById(int id)
    {
        var userId = _userContextService.GetUserId();
        if (userId == null)
        {
            return null;
        }
        var project = await _context.Projects.FirstOrDefaultAsync(p => !p.IsDeleted && p.Id == id && p.Users.Any(u => u.Id == userId));
        return project;
    }

    public async Task<List<Project>> CreateProject(Project project)
    {
        var user = await _userContextService.GetUserAsync();
        if(user is null)
        {
            throw new EntityNotFoundException("User was not found.");
        }

        project.Users.Add(user);

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        return await GetAllProjects();
    }

    public async Task<List<Project>> UpdateProject(int id, Project project)
    {
        var userId = _userContextService.GetUserId();
        if(userId is null)
        {
            throw new EntityNotFoundException("User was not found.");
        }

        // var dbProject = await _context.Projects.FindAsync(id);
        var dbProject = await _context.Projects.FirstOrDefaultAsync(p => !p.IsDeleted && p.Id == id && p.Users.Any(u => u.Id == userId));
        if (dbProject is null)
        {
            throw new EntityNotFoundException($"Entity with ID {id} was not found.");
        }

        if(project.ProjectDetails != null && dbProject.ProjectDetails != null)
        {
            dbProject.ProjectDetails.Description = project.ProjectDetails.Description;
            dbProject.ProjectDetails.StartDate = project.ProjectDetails.StartDate;
            dbProject.ProjectDetails.EndDate = project.ProjectDetails.EndDate;
        }
        else if (project.ProjectDetails != null && dbProject.ProjectDetails == null)
        {
            dbProject.ProjectDetails = new ProjectDetails
            {
                Description = project.ProjectDetails.Description,
                StartDate = project.ProjectDetails.StartDate,
                EndDate = project.ProjectDetails.EndDate,
                Project = project
            };
        }

        dbProject.Name = project.Name;
        dbProject.DateUpdated = DateTime.Now;

        await _context.SaveChangesAsync();

        return await GetAllProjects();
    }

    public async Task<List<Project>?> DeleteProject(int id)
    {
        var userId = _userContextService.GetUserId();
        if(userId == null)
        {
            return null;
        }

        // var dbProject = await _context.Projects.FindAsync(id);
        var dbProject = await _context.Projects.FirstOrDefaultAsync(p => !p.IsDeleted && p.Id == id && p.Users.Any(u => u.Id == userId));
        if (dbProject is null)
        {
            return null;
        }

        dbProject.IsDeleted = true;
        dbProject.DateDeleted = DateTime.Now;

        await _context.SaveChangesAsync();

        return await GetAllProjects();
    }
}