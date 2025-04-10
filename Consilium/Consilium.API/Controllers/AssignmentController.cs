﻿using Consilium.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Consilium.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AssignmentController : ControllerBase {

    private readonly IDBService service;

    public AssignmentController(IDBService service) => this.service = service;

    [HttpGet]
    public void GetAllAssignments() {
        string username = Request.Headers["Email-Auth_Email"]!;
    }
}