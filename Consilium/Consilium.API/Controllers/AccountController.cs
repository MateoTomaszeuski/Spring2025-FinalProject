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

    [HttpGet("/validate")]
    public IResult ValidateAccount(string email, string token) {
        service.Validate(email, token);
        return Results.Redirect("/account/signedin");
    }

    [HttpGet("/signedin")]
    public IResult ValidateAccount() {
        return Results.Content("<h1>Signed In</h1><p>Thanks for signing in!</p>", "text/html");
    }



    //[HttpPost(Name = "CreateTodos")]
    //public IResult Post(TodoItem item) {
    //    string username = Request.Headers["Consilium-User"]!;
    //    service.AddToDo(item, username);
    //    return Results.Accepted();
    //}

    //[HttpDelete("remove/{index}", Name = "RemoveTodos")]
    //public IResult Remove(int index) {
    //    try {
    //        string username = Request.Headers["Consilium-User"]!;
    //        service.RemoveToDo(index, username);
    //    } catch (Exception e) {
    //        return Results.BadRequest(e.Message);
    //    }

    //    return Results.Accepted();
    //}
}