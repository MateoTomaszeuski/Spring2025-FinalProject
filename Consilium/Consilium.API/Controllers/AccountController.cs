using Consilium.API.Metrics;
using EmailAuthenticator;
using Microsoft.AspNetCore.Mvc;

namespace Consilium.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase {

    private readonly AuthService service;
    private readonly ILogger<AuthService> logger;
    private readonly AccountMetrics accountMetrics;

    public AccountController(AuthService service, ILogger<AuthService> logger, AccountMetrics accountMetrics) {
        this.service = service;
        this.logger = logger;
        this.accountMetrics = accountMetrics;
    }

    [HttpGet("all")]
    public IEnumerable<EmailAccount> GetAllAccounts() {
        logger.LogInformation("Getting all accounts");
        return service.GetAllUsers();
    }

    [HttpPost]
    public async Task<string> PostNewAccount(string email) {
        logger.LogInformation("Creating new account for {email}", email);
        accountMetrics.AddedAccount();
        return await service.AddUser(email);
    }

    [HttpGet("validate")]
    public IResult ValidateAccount([FromQuery] string email, [FromQuery] string token) {
        service.Validate(email, token);
        logger.LogInformation("Validating account for {email}", email);
        return Results.Redirect("https://final.codyhowell.dev/signedin");
    }

    /// <summary>
    /// Will either return 
    /// </summary>
    /// <returns></returns>
    [HttpGet("valid")]
    public IResult ValidateAccount() {
        logger.LogInformation("Validating account");
        return Results.Ok();
    }

    [HttpGet("signout/global")]
    public IResult SignOutOfAllAccounts() {
        string email = Request.Headers["Email-Auth_Email"]!; // These are validated in middleware to not be null
        service.GlobalSignOut(email);
        logger.LogInformation("Signing out of all accounts for {email}", email);
        return Results.Ok("Done!");
    }

    [HttpGet("signout")]
    public IResult SignOutOfAccount() {
        string email = Request.Headers["Email-Auth_Email"]!;
        string key = Request.Headers["Email-Auth_Key"]!;
        logger.LogInformation("Signing out of account for {email}", email);
        service.KeySignOut(email, key);
        return Results.Ok("Done!");
    }

    [HttpDelete("delete")]
    public IResult DeleteAccount() {
        string email = Request.Headers["Email-Auth_Email"]!;

        logger.LogInformation("Deleting account for {email}", email);
        service.DeleteUser(email);
        accountMetrics.RemovedAccount();
        return Results.Ok("Done!");
    }
}