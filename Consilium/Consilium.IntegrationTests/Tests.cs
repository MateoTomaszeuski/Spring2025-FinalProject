
using Consilium.Shared.Models;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using TUnit.Assertions;

namespace Consilium.IntegrationTests;

public class Tests : IDisposable {
    private readonly HttpClient client;
    public Tests() {
        client = new HttpClient();
        //client.BaseAddress = new Uri("http://localhost:8080");
        client.BaseAddress = new Uri("http://consilium-api:8080");

        string email = "bob@example.com";
        string key = "APIKEY0987654321";

        client.DefaultRequestHeaders.Add("Email-Auth_Email", email);
        client.DefaultRequestHeaders.Add("Email-Auth_ApiKey", key);
    }

    [Test]
    public async Task HelloWorldCall() {
        string response = await client.GetStringAsync("/health");

        await Assert.That(response).IsEqualTo("\"healthy\"");
    }

    [Test]
    [NotInParallel(Order = 1)]
    public async Task FullAuthFlow() {
        string email = "bob@example.com";
        string token = "TOKENVALID09876543210987654321";
        var response = await client.GetAsync($"/account/validate?email={email}&token={token}");

        await Assert.That(response.StatusCode.ToString()).IsEqualTo("OK");

        var response2 = await client.GetAsync("/account/valid");

        await Assert.That(response2.IsSuccessStatusCode).IsEqualTo(true);
    }

    [Test]
    [NotInParallel(Order = 2)]
    public async Task AddCourseFlow() {
        Course c = new Course() { CourseName = "Test" };
        var response = await client.PostAsJsonAsync("/assignment/course", c);

        await Assert.That(response.IsSuccessStatusCode).IsEqualTo(true);

        var response2 = await client.GetAsync("/assignment/courses");
        List<Course>? courses = await response2.Content.ReadFromJsonAsync<List<Course>>();

        if (courses is null) { throw new Exception("Courses is null"); }

        await Assert.That(courses.Count).IsEqualTo(2);
        await Assert.That(courses[1].Id).IsEqualTo(4);
        await Assert.That(courses[1].CourseName).IsEqualTo("Test");
    }

    public void Dispose() {
        client.Dispose();
    }
}