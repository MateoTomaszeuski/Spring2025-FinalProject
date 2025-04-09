

using System.Net.Http.Json;

namespace Consilium.Shared.Services;

public class ClientService : IClientService {
    private HttpClient client;
    public ClientService(IHttpClientFactory factory) {
        client = factory.CreateClient("ApiClient");
    }
    public void UpdateHeaders(string email, string token) {
        client.DefaultRequestHeaders.Add("Email-Auth_Email", email);
        client.DefaultRequestHeaders.Add("Email-Auth_Token", token);
    }

    public async Task<HttpResponseMessage> Delete(string url) {
        return await client.DeleteAsync(url);
    }

    public async Task<HttpResponseMessage> Get(string url) {
        return await client.GetAsync(url);
    }

    public async Task<HttpResponseMessage> Patch(string url, object? content) {
        return await client.PatchAsJsonAsync(url, content);
    }

    public async Task<HttpResponseMessage> Post(string url, object? content) {
        return await client.PostAsJsonAsync(url, content);
    }
}