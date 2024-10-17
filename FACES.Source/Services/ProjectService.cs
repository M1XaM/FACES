using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

using FACES.Repositories;
using FACES.RequestModels;
using FACES.ResponseModels;
using FACES.Data;
using FACES.Models;

public class ProjectService : IProjectService
{
    private readonly IUserRepository _userRepo;
    private readonly IProjectRepository _projectRepo;
    private readonly IUserProjectRepository _userProjectRepo;
    private readonly IJwtService _jwtService;

    public ProjectService(IUserRepository userRepo, IProjectRepository projectRepo, IUserProjectRepository userProjectRepo, IJwtService jwtService)
    {
        _userRepo = userRepo;
        _projectRepo = projectRepo;
        _userProjectRepo = userProjectRepo;
        _jwtService = jwtService;
    }

    [Authorize]
    public async Task<ProjectResponse> GetUserProjects()
    {
        int userId = _jwtService.ExtractUserIdFromToken();
        var user = await _userRepo.GetByIdAsync(userId);
        if (user == null) return new ProjectResponse { Success = false, Message = "User not found." };
        
        var projects = await _userProjectRepo.GetProjectsByUserId(user.Id);
        return new ProjectResponse { Success = true, Projects = projects };
    }

    [Authorize]
    public async Task<ProjectResponse> CreateProject(ProjectRequest projectRequest)
    {
        int userId = _jwtService.ExtractUserIdFromToken();
        var user = await _userRepo.GetByIdAsync(userId);
        if (user == null) return new ProjectResponse { Success = false, Message = "User not found." };

        // Checking for existing project with the same name
        var existingProject = await _projectRepo.GetProjectByName(projectRequest.Name); 
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
            
            await _userProjectRepo.AddAsync(userProject);
            return new ProjectResponse { Success = true, Message = "Project created successfully." };
        }
        catch (Exception ex)
        {
            return new ProjectResponse{ Success = true, Message = $"An error occurred while creating the project: {ex.Message}"};
        }
    }
}