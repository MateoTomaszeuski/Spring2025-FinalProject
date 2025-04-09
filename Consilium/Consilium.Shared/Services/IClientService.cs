using System.Security.AccessControl;

namespace Consilium.Shared.Services;

public interface IClientService {
    Task<HttpResponseMessage> Get(string url);
    Task<HttpResponseMessage> Post(string url, object? content);
    Task<HttpResponseMessage> Patch(string url, object? content);
    Task<HttpResponseMessage> Delete(string url);
    void UpdateHeaders(string email, string token);
}