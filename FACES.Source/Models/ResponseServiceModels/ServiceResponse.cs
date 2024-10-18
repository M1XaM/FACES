public class ServiceResponse : IServiceResponse
{
    public required bool Success { get; set; }
    public string? Message { get; set; } = null;
}
