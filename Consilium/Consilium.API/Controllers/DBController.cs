using Consilium.Shared.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Consilium.API.Controllers;

[ApiController]
[Route("[controller]")]
public class todoController(IDBService service) : ControllerBase {

    [HttpGet(Name = "GetTodos")]
    public IEnumerable<TodoItem> Get() {
        string username = Request.Headers["Consilium-User"]!; // Cody - I know this will be there at this point
        return service.GetToDos(username);
    }

    [HttpPatch("update", Name = "PatchTodos")]
    public IResult Update(int index, TodoItem item) {
        string username = Request.Headers["Consilium-User"]!; 
        service.UpdateToDo(index, item, username);
        return Results.Accepted();
    }
}