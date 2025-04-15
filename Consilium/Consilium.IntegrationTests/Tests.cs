
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace Consilium.IntegrationTests;

public class Tests {
    private readonly HttpClient client;
    public Tests() {
        client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:8080");
        //client.BaseAddress = new Uri("http://consilium-api:8080");

    }

    [Test]
    public async Task HelloWorldCall() {
        string response = await client.GetStringAsync("/health");

        await Assert.That(response).IsEqualTo("\"healthy\"");
    }

    [Test]
    public async Task FullAuthFlow() {
        string email = "bob@example.com";
        string key = "APIKEY0987654321";
        string token = "TOKENVALID09876543210987654321";
        var response = await client.GetAsync($"/account/validate?email={email}&token={token}");

        await Assert.That(response.StatusCode.ToString()).IsEqualTo("OK");

        client.DefaultRequestHeaders.Add("Email-Auth_Email", email);
        client.DefaultRequestHeaders.Add("Email-Auth_ApiKey", key);

        var response2 = await client.GetAsync("/account/valid");

        await Assert.That(response2.IsSuccessStatusCode).IsEqualTo(true);
    }
}