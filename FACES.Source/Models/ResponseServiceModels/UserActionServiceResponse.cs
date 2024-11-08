using FACES.Models;

namespace FACES.ResponseModels;
public class UserActionServiceResponse : ServiceResponse
{
    public User? User { get; set; } = null;
}