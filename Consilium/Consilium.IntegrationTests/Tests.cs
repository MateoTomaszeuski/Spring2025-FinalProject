
using System.Threading.Tasks;

namespace Consilium.IntegrationTests;

public class Tests
{
    [Test]
    public async Task HelloWorldCall()
    {
        var client = new HttpClient();
        client.BaseAddress = new Uri("https://localhost:8080");

        var response = await client.GetAsync("/");
        string output = await response.Content.ReadAsStringAsync();

        await Assert.That(output).IsEqualTo("Welcome to the Consilium Api");
    }
}