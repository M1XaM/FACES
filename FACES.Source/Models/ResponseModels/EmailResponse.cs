namespace FACES.ResponseModels;
public class EmailResponse
{
    public required bool Success { get; set; }
    public string? Message { get; set; } = null;
}