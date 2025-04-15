using Consilium.Shared.Models;

namespace Consilium.API.InMemoryServices;

public class DBServiceIM : IDBService {
    private Dictionary<string, List<TodoItem>> todos = new() {
        { "password", new List<TodoItem>() { new TodoItem() { Title = "lorem" } } }
    };

    public void AddAssignment(Assignment assignment, string email) {
        throw new NotImplementedException();
    }

    public Task<string> AddMessage(Message message) {
        throw new NotImplementedException();
    }

    public int AddToDo(TodoItem Todo, string username) {
        if (todos.ContainsKey(username)) {
            todos[username].Add(Todo);
        } else {
            todos.Add(username, new() { Todo });
        }
        return 0;
    }

    public IEnumerable<bool> CheckUser(string otherUser) {
        throw new NotImplementedException();
    }

    public void DeleteAssignment(int id, string email) {
        throw new NotImplementedException();
    }

    public IEnumerable<Assignment> GetAllAssignments(string email) {
        throw new NotImplementedException();
    }

    public IEnumerable<Course> GetAllCourses(string username) {
        throw new NotImplementedException();
    }

    public IEnumerable<User> GetAllUsers() {
        throw new NotImplementedException();
    }

    public IEnumerable<string> GetConversations(string username) {
        throw new NotImplementedException();
    }

    public IEnumerable<Assignment> GetIncompleteAssignments(string email) {
        throw new NotImplementedException();
    }

    public IEnumerable<Message> GetMessages(string username, string otherUser) {
        throw new NotImplementedException();
    }

    public IEnumerable<TodoItem> GetTodoList(string email) {
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

    public void RemoveToDo(int item, string username) {
        if (todos.ContainsKey(username)) {
            List<TodoItem> items = todos[username];
            items.Remove(items.First(a => a.Id == item));
        }
    }

    public void UpdateAssignment(Assignment assignment, string email) {
        throw new NotImplementedException();
    }

    public void UpdateToDo(TodoItem Todo, string username) {
        if (todos.ContainsKey(username)) {
            int index = todos[username].IndexOf(Todo);
            todos[username][index] = Todo;
        }
    }

    int IDBService.AddAssignment(Assignment assignment, string email) {
        throw new NotImplementedException();
    }
}