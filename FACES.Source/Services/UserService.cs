using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

using FACES.Repositories;
using FACES.RequestModels;
using FACES.ResponseModels;
using FACES.Data;
using FACES.Models;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepo;
    private readonly IJwtService _jwtService;

    public UserService(IUserRepository userRepo, IJwtService jwtService)
    {
        _userRepo = userRepo;
        _jwtService = jwtService;
    }

    public async Task<AuthResponse> Login(LoginRequest loginRequest)
    {
        var user = await _userRepo.GetUserByEmail(loginRequest.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
        {
            return new AuthResponse { Success = false, Message = "Invalid email or password." };
        }
        
        var token = _jwtService.GenerateJwtToken(user.Id.ToString());
        return new AuthResponse { Success = true, Token = token };
    }

    public async Task<AuthResponse> Registration(FullUserRequest registrationRequest)
    {
        // Check if the email already exists
        var existingUser = await _userRepo.GetUserByEmail(registrationRequest.Email);
        if (existingUser != null)
        {
            return new AuthResponse { Success = false, Message = "Email is already in use." };
        }

        var newUser = new User
        {
            FirstName = registrationRequest.FirstName,
            LastName = registrationRequest.LastName,
            Email = registrationRequest.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(registrationRequest.Password)
        };

        await _userRepo.AddAsync(newUser);
        var token = _jwtService.GenerateJwtToken(newUser.Id.ToString());
        return new AuthResponse { Success = true, Token = token, Message = "Registration successful."};
    }

    [Authorize]
    public async Task<UserActionResponse> Profile()
    {
        int userId = _jwtService.ExtractUserIdFromToken();
        
        var user = await _userRepo.GetByIdAsync(userId);
        if (user == null) return new UserActionResponse { Success = false, Message = "User not found." };

        return new UserActionResponse { Success = true, User = user };
    }

    [Authorize]
    public async Task<UserActionResponse> ModifyProfile(FullUserRequest updatedUser)
    {
        int userId = _jwtService.ExtractUserIdFromToken();

        var user = await _userRepo.GetByIdAsync(userId);
        if (user == null) return new UserActionResponse { Success = false, Message = "User not found." };

        user.FirstName = updatedUser.FirstName;
        user.LastName = updatedUser.LastName;
        user.Email = updatedUser.Email;
        user.Password = updatedUser.Password;
        await _userRepo.UpdateAsync(user);

        return new UserActionResponse { Success = true };
    }

    [Authorize]
    public async Task<UserActionResponse> DeleteProfile()
    {
        int userId = _jwtService.ExtractUserIdFromToken();
        var user = await _userRepo.GetByIdAsync(userId);

        if (user == null) return new UserActionResponse { Success = false, Message = "User invalid."};

        await _userRepo.DeleteAsync(user.Id);
        return new UserActionResponse { Success = true };
    }
}