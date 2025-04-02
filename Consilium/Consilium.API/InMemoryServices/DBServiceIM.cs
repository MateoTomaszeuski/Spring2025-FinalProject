using Consilium.Shared.Models;

namespace Consilium.API.InMemoryServices;

public class DBServiceIM : IDBService {
    private Dictionary<string, List<TodoItem>> todos = new() {
        { "password", new List<TodoItem>() { new TodoItem() { Title = "lorem" } } }
    };

    public void AddToDo(TodoItem Todo, string username) {
        if (todos.ContainsKey(username)) {
            todos[username].Add(Todo);
        } else {
            todos.Add(username, new() { Todo });
        }
    }

    public IEnumerable<User> GetAllUsers() {
        throw new NotImplementedException();
    }

    public TodoList GetTodoList(int tableId, string email) {
        throw new NotImplementedException();
    }

    public IEnumerable<TodoList> GetToDoLists(string username) {
        throw new NotImplementedException();
    }
    public IEnumerable<TodoItem> GetToDos(string username) {
        if (todos.ContainsKey(username)) {
            List<TodoItem> items = todos[username];
            return items;
        }
        return new List<TodoItem>();
    }

    public void RemoveToDo(TodoItem item, string username) {
        if (todos.ContainsKey(username)) {
            todos[username].Remove(item);
        }
    }

    public int ToDoCount(string username) {
        if (todos.ContainsKey(username)) {
            return todos[username].Count();
        }
        return 0;
    }

    public void UpdateToDo(TodoItem Todo, string username) {
        if (todos.ContainsKey(username)) {
            int index = todos[username].IndexOf(Todo);
            todos[username][index] = Todo;
        }
    }
}