﻿using Consilium.Shared.Models;

namespace Consilium.API.InMemoryServices;

public class DBServiceIM : IDBService {
    private Dictionary<string, List<TodoItem>> todos = new() {
        { "password", new List<TodoItem>() { new TodoItem() { Title = "lorem" } } }
    };

    public int AddToDo(TodoItem Todo, string username) {
        if (todos.ContainsKey(username)) {
            todos[username].Add(Todo);
        } else {
            todos.Add(username, new() { Todo });
        }
        return 0;
    }

    public IEnumerable<User> GetAllUsers() {
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

    public void RemoveToDo(TodoItem item, string username) {
        if (todos.ContainsKey(username)) {
            todos[username].Remove(item);
        }
    }

    public void UpdateToDo(TodoItem Todo, string username) {
        if (todos.ContainsKey(username)) {
            int index = todos[username].IndexOf(Todo);
            todos[username][index] = Todo;
        }
    }
}