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
        return Results.Redirect("/account/signedin");
    }

    [HttpGet("signedin")]
    public IResult ValidateAccount() {
        return Results.File("../../../Misc/SignedIn.html", "text/html");
    }



}