using FACES.Models;

namespace FACES.ResponseModels;
public class ClientServiceResponse : ServiceResponse
{
    public List<Client>? Clients { get; set; } = null;
}