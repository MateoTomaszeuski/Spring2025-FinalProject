using Consilium.API.InMemoryServices;
using Consilium.Shared.Models;

namespace Consilium.API;

public interface IDBService {
    public int ToDoCount(string email);
    public void AddToDo(TodoItem Todo, string email);
    IEnumerable<TodoList> GetToDoLists(string email);
    TodoList GetTodoList(int tableId, string email);
    public void UpdateToDo(TodoItem Todo, string email);
    public void RemoveToDo(TodoItem Todo, string email);
}