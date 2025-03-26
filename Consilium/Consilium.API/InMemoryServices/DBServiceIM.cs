using Consilium.Shared.ViewModels;

namespace Consilium.API.InMemoryServices;

public class DBServiceIM : IDBService {
    private Dictionary<string, List<TodoItem>> todos = new() { 
        { "cody", new List<TodoItem>() { new TodoItem() { Title = "lorem"} } } 
    };

    public void AddToDo(TodoItem Todo, string username) {
        if (todos.ContainsKey(username)) {
            todos[username].Add(Todo);
        } else {
            todos.Add(username, new() { Todo });
        }
    }

    public List<TodoItem> GetToDos(string username) {
        if (todos.ContainsKey(username)) {
            List<TodoItem> items = todos[username];
            return items;
        }
        return new();
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