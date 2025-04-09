

using Consilium.Shared.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Runtime.ExceptionServices;

namespace Consilium.Shared.Services;

public class ToDoService : IToDoService {
    private readonly HttpClient client;
    private readonly IPersistenceService service;
    /// <summary>
    /// Used by tests to assign starting values. DO NOT USE DIRECTLY IN PRODUCTION.
    /// </summary>
    public List<TodoItem> TodoItems { get; set; }

    public ToDoService(IHttpClientFactory factory, IPersistenceService service) {
        client = factory.CreateClient("ApiClient");
        client.DefaultRequestHeaders.Add("Email-Auth_Email", "bob@example.com");
        this.service = service;
        TodoItems = new();
    }

    public ObservableCollection<TodoItem> GetTodoItems() {
        return new(ListCollapser.CollapseList(TodoItems));
    }

    public async Task AddItemAsync(TodoItem item) {
        var response = await client.PostAsJsonAsync($"todo", item);
        int id = Convert.ToInt16(await response.Content.ReadAsStringAsync());
        item.Id = id;
        TodoItems.Add(item);
    }

    /// <summary>
    /// Takes in an item with an adjusted CompletionDate and updates the item in the list with 
    /// that value.
    /// </summary>
    public async Task UpdateItemAsync(TodoItem item) {
        var response = await client.PatchAsJsonAsync($"todo/update", item);
        TodoItem listItem = TodoItems.Where(a => a.Id == item.Id).First();
        listItem.CompletionDate = item.CompletionDate;
    }

    public async Task<string> RemoveToDoAsync(int itemId) {
        var response = await client.DeleteAsync($"todo/remove/{itemId}");

        TodoItem child = TodoItems.First(a => itemId == a.Id);
        TodoItem? parent = TodoItems.FirstOrDefault(a => child.ParentId == a.Id);
        TodoItems.Remove(child);

        parent?.Subtasks.Remove(child);

        if (response.IsSuccessStatusCode) {
            return "Deleted successfully";
        } else {
            return "Failed to remove item";
        }
    }

    public async Task InitializeTodosAsync() {
        var response = await client.GetFromJsonAsync<IEnumerable<TodoItem>>("todo");
        TodoItems = response == null ? new() : new(response);
    }
}