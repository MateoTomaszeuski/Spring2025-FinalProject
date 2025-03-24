
namespace Consilium.IntegrationTests;

public class Tests
{
    [Test]
    public void Basic()
    {
        var client = new HttpClient();
        client.BaseAddress = new Uri("https://localhost:8080");
        Console.WriteLine("This is a basic test");
    }
    
}