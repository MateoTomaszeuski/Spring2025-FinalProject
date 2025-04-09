
using Consilium.Shared.Models;
using System.Net.Http.Json;

namespace Consilium.Shared.Services;

public class LogInService : ILogInService {
    private readonly IClientService client;
    private readonly IPersistenceService service;

    public LogInService(IClientService client, IPersistenceService service) {
        this.client = client;
        this.service = service;
    }

    public async Task<string> LogIn(string email) {
        return await client.PostAsync($"/account?email={email}", null).Result.Content.ReadAsStringAsync();
    }
}