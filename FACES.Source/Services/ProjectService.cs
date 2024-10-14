using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

using FACES.Repositories;
using FACES.RequestModels;
using FACES.ResponseModels;
using FACES.Data;
using FACES.Models;

public class ProjectService : IProjectService
{
    private readonly ApplicationDbContext _db;
    private readonly IGenericRepository<User> _userRepo;
    private readonly IGenericRepository<Project> _projectRepo;
    private readonly IJwtService _jwtService;

    public ProjectService(ApplicationDbContext db, IGenericRepository<User> userRepo, IJwtService jwtService, IGenericRepository<Project> projectRepo)
    {
        _db = db;
        _userRepo = userRepo;
        _jwtService = jwtService;
        _projectRepo = projectRepo;
    }

    [Authorize]
    public async Task<ProjectResponse> GetUserProjects()
    {
        int userId = _jwtService.ExtractUserIdFromToken();
        var user = await _db.Users.SingleOrDefaultAsync(u => u.Id == userId);
        if (user == null) return new ProjectResponse { Success = false, Message = "User not found." };
        
        // Fetch the projects related to the user
        var projects = await _db.UserProjects
                                .Where(up => up.UserId == user.Id)
                                .Select(up => up.Project)
                                .ToListAsync();

        return new ProjectResponse { Success = true, Projects = projects };
    }

    [Authorize]
    public async Task<ProjectResponse> CreateProject(ProjectRequest projectRequest)
    {
        int userId = _jwtService.ExtractUserIdFromToken();
        var user = await _userRepo.GetByIdAsync(userId);
        if (user == null) return new ProjectResponse { Success = false, Message = "User not found." };

        // Checking for existing project with the same name
        var existingProject = await _db.Projects.FirstOrDefaultAsync(p => p.Name == projectRequest.Name); 
        if (existingProject != null) return new ProjectResponse  { Success = false, Message = "A project with this name already exists." };

        var project = new Project
        {
            Name = projectRequest.Name,
            Description =  projectRequest.Description,
        };

        try
        {
            await _projectRepo.AddAsync(project);
            var userProject = new UserProject
            {
                UserId = user.Id,
                User = await _userRepo.GetByIdAsync(user.Id),
                ProjectId = project.Id,
                Project = await _projectRepo.GetByIdAsync(project.Id)
            };
            
            await _db.UserProjects.AddAsync(userProject);
            await _db.SaveChangesAsync();
            return new ProjectResponse { Success = true, Message = "Project created successfully." };
        }
        catch (Exception ex)
        {
            return new ProjectResponse{ Success = true, Message = $"An error occurred while creating the project: {ex.Message}"};
        }
    }
}