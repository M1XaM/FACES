namespace FACES.RequestModels;
public class AddClientRequest
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
}