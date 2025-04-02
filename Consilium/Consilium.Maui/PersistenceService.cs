using Consilium.Shared.Models;
using System.Text.Json;

namespace Consilium.Shared.Services;

public class PersistenceService : IPersistenceService {
    public TodoList? GetToDoLists() {
        string output = Preferences.Get("todo-list", "{}");
        return JsonSerializer.Deserialize<TodoList>(output);
    }

    public void SaveList(TodoList list) {
        string serialized = JsonSerializer.Serialize(list);
        Preferences.Set("todo-list", serialized);
    }
}