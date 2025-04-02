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

    [HttpGet("{id}/items", Name = "GetItems")]
    public TodoList GetItems(int id) {
        string username = Request.Headers["Email-Auth_Email"]!; // Cody - I know this will be there at this point
        return service.GetTodoList(id, username);
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
        service.AddToDo(item, username);
        return Results.Accepted();
    }

    [HttpDelete("remove", Name = "RemoveTodos")]
    public IResult Remove(TodoItem item) {
        try {
            string username = Request.Headers["Email-Auth_Email"]!;
            service.RemoveToDo(item, username);
        } catch (Exception e) {
            return Results.BadRequest(e.Message);
        }

        return Results.Accepted();
    }
}