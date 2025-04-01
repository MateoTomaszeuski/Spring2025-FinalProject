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

    public void RemoveToDo(int index, string username) {
        if (todos.ContainsKey(username)) {
            todos[username].RemoveAt(index);
        }
    }

    public int ToDoCount(string username) {
        if (todos.ContainsKey(username)) {
            return todos[username].Count();
        }
        return 0;
    }

    public void UpdateToDo(int index, TodoItem Todo, string username) {
        if (todos.ContainsKey(username)) {
            todos[username][index] = Todo;
        }
    }
}