using Consilium.Shared.Models;
using EmailAuthenticator;
using Microsoft.AspNetCore.Mvc;

namespace Consilium.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase {

    private readonly AuthService service;

    public AccountController(AuthService service) => this.service = service;

    [HttpGet("all")]
    public IEnumerable<EmailAccount> GetAllAccounts() {
        return service.GetAllUsers();
    }

    [HttpPost]
    public async Task<string> PostNewAccount(string email) {
        return await service.AddUser(email);
    }

    [HttpGet("validate")]
    public IResult ValidateAccount([FromQuery] string email, [FromQuery] string token) {
        service.Validate(email, token);
        return Results.Redirect("final.codyhowell.dev/signedin");
    }

    [HttpGet("valid")]
    public IResult ValidateAccount() {
        return Results.Ok();
    }

    [HttpGet("signout/global")]
    public IResult SignOutOfAllAccounts() {
        string email = Request.Headers["Email-Auth_Email"]!; // These are validated in middleware to not be null
        service.GlobalSignOut(email);
        return Results.Ok("Done!");
    }

    [HttpGet("signout")]
    public IResult SignOutOfAccount() {
        string email = Request.Headers["Email-Auth_Email"]!;
        string key = Request.Headers["Email-Auth_Key"]!;

        service.KeySignOut(email, key);
        return Results.Ok("Done!");
    }

    [HttpDelete("delete")]
    public IResult DeleteAccount() {
        string email = Request.Headers["Email-Auth_Email"]!;

        service.DeleteUser(email);
        return Results.Ok("Done!");
    }
}