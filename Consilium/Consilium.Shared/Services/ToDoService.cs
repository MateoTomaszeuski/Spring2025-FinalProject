

using Consilium.Shared.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http.Json;

namespace Consilium.Shared.Services;

public class ToDoService : IToDoService {
    private readonly HttpClient client;
    private readonly IPersistenceService service;
    private List<TodoItem> todoItems;

    public ToDoService(IHttpClientFactory factory, IPersistenceService service) {
        client = factory.CreateClient("ApiClient");
        client.DefaultRequestHeaders.Add("Email-Auth_Email", "bob@example.com");
        this.service = service;
        todoItems = new();
    }

    public ObservableCollection<TodoItem> GetTodoItemsAsync() {
        return new(ListCollapser.CollapseList(todoItems));
    }

    public async Task AddItem(TodoItem item) {
        var response = await client.PostAsJsonAsync($"todo", item);
        int id = Convert.ToInt16(response.Content.ReadAsStringAsync());
        item.Id = id;
        todoItems.Add(item);
    }

    /// <summary>
    /// Takes in an item with an adjusted CompletionDate and updates the item in the list with 
    /// that value.
    /// </summary>
    public async Task UpdateItem(TodoItem item) {
        var response = await client.PatchAsJsonAsync($"todo/update", item);
        TodoItem listItem = todoItems.Where(a => a.Id == item.Id).First();
        listItem.CompletionDate = item.CompletionDate;
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