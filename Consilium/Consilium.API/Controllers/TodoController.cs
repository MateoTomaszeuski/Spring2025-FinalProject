using Consilium.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Consilium.API.Controllers;

[ApiController]
[Route("[controller]")]
public class todoController : ControllerBase {

    private readonly IDBService service;

    public todoController(IDBService service) => this.service = service;

    [HttpGet(Name = "GetTodos")]
    public IEnumerable<TodoList> Get() {
        string username = Request.Headers["Email-Auth_Email"]!; // Cody - I know this will be there at this point
        return service.GetToDoLists(username);
    }

    [HttpPatch("update", Name = "PatchTodos")]
    public IResult Update(int index, TodoItem item) {
        string username = Request.Headers["Email-Auth_Email"]!;
        service.UpdateToDo(index, item, username);
        return Results.Accepted();
    }


    [HttpPost(Name = "CreateTodos")]
    public IResult Post(TodoItem item) {
        string username = Request.Headers["Email-Auth_Email"]!;
        service.AddToDo(item, username);
        return Results.Accepted();
    }

    [HttpDelete("remove/{index}", Name = "RemoveTodos")]
    public IResult Remove(int index) {
        try {
            string username = Request.Headers["Email-Auth_Email"]!;
            service.RemoveToDo(index, username);
        } catch (Exception e) {
            return Results.BadRequest(e.Message);
        }

        return Results.Accepted();
    }
}