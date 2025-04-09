
using Consilium.Shared.Models;
using System.Net.Http.Json;

namespace Consilium.Shared.Services;

public class LogInService : ILogInService {
    private readonly HttpClient client;
    private readonly IPersistenceService service;

    public LogInService(IHttpClientFactory factory, IPersistenceService service) {
        client = factory.CreateClient("ApiClient");
        client.DefaultRequestHeaders.Add("Email-Auth_Email", "bob@example.com");
        this.service = service;
    }

    public async Task<string> LogIn(string email) {
        return await client.PostAsync($"/account?email={email}", null).Result.Content.ReadAsStringAsync();
    }
}