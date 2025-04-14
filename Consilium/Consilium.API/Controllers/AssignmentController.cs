using Consilium.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Consilium.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AssignmentController : ControllerBase {

    private readonly IDBService service;

    public AssignmentController(IDBService service) => this.service = service;

    [HttpGet]
    public IEnumerable<Assignment> GetAllAssignments() {
        string username = Request.Headers["Email-Auth_Email"]!;
        return service.GetAllAssignments(username);

    }

    [HttpPost]
    public IResult PostAssignment(Assignment assignment) {
        string username = Request.Headers["Email-Auth_Email"]!;
        int value = service.AddAssignment(assignment, username);

        if (value == -1) {
            return Results.Unauthorized();
        } else {
            return Results.Ok(value);
        }
    }
}