

using Consilium.Shared.Models;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace Consilium.Shared.Services;

public class ToDoService : IToDoService {
    private readonly HttpClient client;

    public ToDoService(IHttpClientFactory factory) {
        client = factory.CreateClient("ApiClient");
        client.DefaultRequestHeaders.Add("Consilium-User", "password");
    }

    public async Task<ObservableCollection<TodoItem>> GetTodoItemsAsync() {
        var response = await client.GetFromJsonAsync<IEnumerable<TodoItem>>("todo");
        return response == null ? new() : new(response);
    }

    public async Task<string> RemoveToDoAsync(int itemIndex) {
        var response = await client.DeleteAsync($"todo/remove/{itemIndex}");

        if (response.IsSuccessStatusCode) {
            return "Deleted successfully";
        } else {
            return "Failed to remove item";
        }
    }
}