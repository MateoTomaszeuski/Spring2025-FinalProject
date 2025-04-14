using Consilium.Shared.Models;

namespace Consilium.Shared.Services;

public interface IPersistenceService {
    bool loggedIn { get; set; }
    TodoList? GetToDoLists();
    void SaveList(TodoList list);
    void SaveToken(string email, string token);
    Task OnStartup();
}