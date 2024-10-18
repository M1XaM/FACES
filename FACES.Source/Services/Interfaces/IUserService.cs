using FACES.RequestModels;
using FACES.ResponseModels;

public interface IUserService
{
    Task<AuthServiceResponse> LoginAsync(LoginViewRequest loginRequest);
    Task<AuthServiceResponse> RegistrationAsync(FullUserViewRequest registerRequest);
    Task<UserActionServiceResponse> ProfileAsync();
    Task<UserActionServiceResponse> ModifyProfileAsync(FullUserViewRequest updatedUser);
    Task<UserActionServiceResponse> DeleteProfileAsync();
}