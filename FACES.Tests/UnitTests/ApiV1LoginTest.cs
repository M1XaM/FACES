using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Xunit;
using FACES;

namespace FACES.Tests;
public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>> // Use Program directly
{
    private readonly HttpClient _client;

    public AuthControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient(); // Create a client to send requests to the API
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsOkWithToken()
    {
        // Arrange
        var requestBody = new
        {
            Email = "a@a.a",
            Password = "a"
        };
        var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/v1/login", content);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("token", responseContent); // Check if token is returned
    }

    [Fact]
    public async Task Login_InvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var requestBody = new
        {
            Email = "invaliduser",
            Password = "wrongpassword"
        };
        var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/v1/login", content);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode); // Status Code 401
    }
}
