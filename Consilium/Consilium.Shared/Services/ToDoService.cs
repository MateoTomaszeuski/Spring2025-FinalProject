

using Consilium.Shared.Models;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace Consilium.Shared.Services;

public class ToDoService : IToDoService {
    private readonly HttpClient client;
    private List<TodoItem> todoItems;

    public ToDoService(IHttpClientFactory factory) {
        client = factory.CreateClient("ApiClient");
        client.DefaultRequestHeaders.Add("Consilium-User", "password");
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