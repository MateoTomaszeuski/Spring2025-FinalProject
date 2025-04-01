using Consilium.API.InMemoryServices;
using Consilium.Shared.Models;

namespace Consilium.API;

public interface IDBService {
    public int ToDoCount(string username);
    public void AddToDo(TodoItem Todo, string username);
    IEnumerable<TodoList> GetToDoLists(string username);
    public void UpdateToDo(int index, TodoItem Todo, string username);
    public void RemoveToDo(int index, string username);
}