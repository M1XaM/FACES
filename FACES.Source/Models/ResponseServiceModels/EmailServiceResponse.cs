namespace FACES.ResponseModels;
public class EmailServiceResponse
{
    public required bool Success { get; set; }
    public string? Message { get; set; } = null;
}