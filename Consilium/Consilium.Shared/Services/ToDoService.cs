

using Consilium.Shared.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http.Json;

namespace Consilium.Shared.Services;

public class ToDoService : IToDoService {
    private readonly HttpClient client;
    private readonly IPersistenceService service;
    private List<TodoItem> todoItems;

    private Dictionary<string, TodoList> lists;
    public TodoList this[string s] => lists[s];

    public ToDoService(IHttpClientFactory factory, IPersistenceService service) {
        client = factory.CreateClient("ApiClient");
        client.DefaultRequestHeaders.Add("Email-Auth_Email", "bob@example.com");
        this.service = service;
        todoItems = new();
    }

    public ObservableCollection<TodoItem> GetTodoItemsAsync() {
        return new(todoItems);
    }

    public async Task<string> RemoveToDoAsync(int itemIndex) {
        var response = await client.DeleteAsync($"todo/remove/{itemIndex}");

        if (response.IsSuccessStatusCode) {
            return "Deleted successfully";
        } else {
            return "Failed to remove item";
        }
    }

    public async Task InitializeTodosAsync() {
        var response = await client.GetFromJsonAsync<IEnumerable<TodoItem>>("todo");
        todoItems = response == null ? new() : new(response);
    }

}