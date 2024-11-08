using FACES.RequestModels;
using FACES.ResponseModels;

public interface IUserService
{
    Task<AuthServiceResponse> LoginAsync(LoginViewModel loginRequest);
    Task<AuthServiceResponse> RegistrationAsync(FullUserViewModel registerRequest);
    Task<UserActionServiceResponse> ProfileAsync();
    Task<UserActionServiceResponse> ModifyProfileAsync(FullUserViewModel updatedUser);
    Task<UserActionServiceResponse> DeleteProfileAsync();
}