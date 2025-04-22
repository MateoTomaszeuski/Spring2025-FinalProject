using Consilium.API.Metrics;
using Consilium.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Consilium.API.Controllers;

[ApiController]
[Route("[controller]")]
public class NewFeatureController : ControllerBase {
    private readonly ILogger<NewFeatureController> logger;
    private readonly NewFeatureMerics newFeatureMerics;

    public NewFeatureController(ILogger<NewFeatureController> logger, NewFeatureMerics newFeatureMerics) {
        this.logger = logger;
        this.newFeatureMerics = newFeatureMerics;
    }
    [HttpGet]
    public string GetAllAccounts() {
        logger.LogInformation("Getting all accounts");
        newFeatureMerics.NonIntegratedViewClicked();
        return "Received";
    }
}