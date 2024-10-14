using FACES.RequestModels;
using FACES.ResponseModels;

public interface IUserService
{
    Task<AuthResponse> Login(LoginRequest loginRequest);
    Task<AuthResponse> Registration(FullUserRequest registerRequest);
    Task<UserActionResponse> Profile();
    Task<UserActionResponse> ModifyProfile(FullUserRequest updatedUser);
    Task<UserActionResponse> DeleteProfile();
}