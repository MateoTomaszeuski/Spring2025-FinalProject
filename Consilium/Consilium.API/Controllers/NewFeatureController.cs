using Consilium.API.Metrics;
using Consilium.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Consilium.API.Controllers;

[ApiController]
[Route("[controller]")]
public class NewFeatureController : ControllerBase {
    private readonly ILogger<NewFeatureController> logger;
    private readonly NewFeatureMerics newFeatureMerics;
    private HttpClient client;

    public NewFeatureController(IHttpClientFactory factory, ILogger<NewFeatureController> logger, NewFeatureMerics newFeatureMerics) {
        this.logger = logger;
        this.newFeatureMerics = newFeatureMerics;
        client = factory.CreateClient("FeedbackWebhock");
    }
    [HttpGet]
    public string GetNewFeature() {
        logger.LogInformation("New featrure clicked");
        newFeatureMerics.NonIntegratedViewClicked();
        return "Received";
    }
    [HttpGet("feedback/{feedback}")]
    public async Task<string> GetFeedback(string feedback) {
        logger.LogInformation("Feedback clicked");

        var payload = new { content = feedback };

        var response = await client.PostAsJsonAsync("", payload);

        if (!response.IsSuccessStatusCode) {
            logger.LogError("Discord webhook failed with {StatusCode}", response.StatusCode);
        }

        return "Received";
    }
}