using Consilium.Shared.Models;
using EmailAuthenticator;
using Microsoft.AspNetCore.Mvc;

namespace Consilium.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase {

    private readonly AuthService service;

    public AccountController(AuthService service) => this.service = service;

    [HttpGet]
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

    [HttpGet("global/signout")]
    public IResult SignOutOfAccounts() {
        string email = Request.Headers["Email-Auth_Email"]!; // Cody - This is validated in IdentityMiddleware to not be null
        service.GlobalSignOut(email);
        return Results.Ok("Done!");
    }

}