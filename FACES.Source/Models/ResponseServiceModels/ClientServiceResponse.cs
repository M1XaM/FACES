using FACES.Models;

namespace FACES.ResponseModels;
public class ClientServiceResponse : ServiceResponse
{
    public IEnumerable<Client>? Clients { get; set; } = null;
}