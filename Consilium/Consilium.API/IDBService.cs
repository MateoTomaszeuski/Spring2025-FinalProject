using Consilium.API.InMemoryServices;
using Consilium.Shared.Models;

namespace Consilium.API;

public interface IDBService {
    public int AddToDo(TodoItem Todo, string email);
    IEnumerable<TodoItem> GetTodoList(string email);
    public void UpdateToDo(TodoItem Todo, string email);
    public void RemoveToDo(TodoItem Todo, string email);
}