using FACES.Models;

namespace FACES.ResponseModels;
public class ClientServiceResponse
{
    public required bool Success { get; set; }
    public string? Message { get; set; } = null;
    public List<Client>? Clients { get; set; } = null;
}