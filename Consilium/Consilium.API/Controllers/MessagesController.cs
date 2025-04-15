using Consilium.Shared.Models;
using EmailAuthenticator;
using Microsoft.AspNetCore.Mvc;

namespace Consilium.API.Controllers;

[ApiController]
[Route("[controller]")]
public class MessagesController : ControllerBase {

    private readonly IDBService service;

    public MessagesController(IDBService service) => this.service = service;

    [HttpGet("all")]
    public IEnumerable<string> GetAllConversations() {
        string username = Request.Headers["Email-Auth_Email"]!;
        return service.GetConversations(username);
    }
    [HttpGet("{otherUser}")]
    public IEnumerable<Message> GetAllMessages(string otherUser) {
        string username = Request.Headers["Email-Auth_Email"]!;
        return service.GetMessages(username, otherUser);
    }
    [HttpGet("/check/{otherUser}")]
    public IEnumerable<bool> GetUser(string otherUser) {
        return service.CheckUser(otherUser);
    }

    [HttpPost]
    public async Task<string> PostNewMessage(Message message) {
        string username = Request.Headers["Email-Auth_Email"]!;
        message.Sender = username;
        return await service.AddMessage(message);
    }

}