using Consilium.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Consilium.API.Controllers;

[ApiController]
[Route("[controller]")]
public class todoController : ControllerBase {

    private readonly IDBService service;

    public todoController(IDBService service) => this.service = service;

    [HttpGet(Name = "GetTodos")]
    public IEnumerable<TodoItem> Get() {
        string username = Request.Headers["Email-Auth_Email"]!; // Cody - I know this will be there at this point
        return service.GetTodoList(username);
    }

    [HttpPatch("update", Name = "PatchTodos")]
    public IResult Update(TodoItem item) {
        string username = Request.Headers["Email-Auth_Email"]!;
        try {
            service.UpdateToDo(item, username);
        } catch (Exception e) {
            return Results.BadRequest(e.Message);
        }
        return Results.Accepted();
    }


    [HttpPost(Name = "CreateTodos")]
    public IResult Post(TodoItem item) {
        string username = Request.Headers["Email-Auth_Email"]!;
        int result = service.AddToDo(item, username);
        return Results.Ok(result);
    }

    [HttpDelete("remove/{item}", Name = "RemoveTodos")]
    public IResult Remove(int item) {
        try {
            string username = Request.Headers["Email-Auth_Email"]!;
            service.RemoveToDo(item, username);
        } catch (Exception e) {
            return Results.BadRequest(e.Message);
        }

        return Results.Accepted();
    }
}