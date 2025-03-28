﻿
using System.Threading.Tasks;

namespace Consilium.IntegrationTests;

public class Tests {
    [Test]
    public async Task HelloWorldCall() {
        var client = new HttpClient();
        client.BaseAddress = new Uri("http://consilium-api:8080");

        string response = await client.GetStringAsync("/");

        await Assert.That(response).IsEqualTo("Welcome to the Consilium Api");
    }
}