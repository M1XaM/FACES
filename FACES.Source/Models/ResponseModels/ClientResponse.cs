using FACES.Models;

namespace FACES.ResponseModels;
public class ClientResponse
{
    public required bool Success { get; set; }
    public string? Message { get; set; } = null;
    public List<Client>? Clients { get; set; } = null;
}