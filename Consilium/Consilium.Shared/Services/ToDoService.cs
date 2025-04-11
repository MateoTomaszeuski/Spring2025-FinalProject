

using Consilium.Shared.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Runtime.ExceptionServices;

namespace Consilium.Shared.Services;

public class ToDoService : IToDoService {
    private readonly IPersistenceService persistenceService;
    private readonly IClientService client;
    /// <summary>
    /// Used by tests to assign starting values. DO NOT USE DIRECTLY IN PRODUCTION.
    /// </summary>
    public List<TodoItem> TodoItems { get; set; }

    public ToDoService(IPersistenceService persistenceService, IClientService client) {

        this.persistenceService = persistenceService;
        this.client = client;
        TodoItems = new();
    }

    public ObservableCollection<TodoItem> GetTodoItems() {
        return new(ListCollapser.CollapseList(TodoItems));
    }

    public async Task AddItemAsync(TodoItem item) {
        var response = await client.PostAsync($"todo", item);
        int id = Convert.ToInt16(await response.Content.ReadAsStringAsync());
        item.Id = id;
        TodoItems.Add(item);
    }

    /// <summary>
    /// Takes in an item with an adjusted CompletionDate and updates the item in the list with 
    /// that value.
    /// </summary>
    public async Task UpdateItemAsync(TodoItem item) {
        var response = await client.PatchAsync($"todo/update", item);
        TodoItem listItem = TodoItems.Where(a => a.Id == item.Id).First();
        listItem.CompletionDate = item.CompletionDate;
    }

    public async Task<string> RemoveToDoAsync(int itemId) {
        var response = await client.DeleteAsync($"todo/remove/{itemId}");

        TodoItem child = TodoItems.First(a => itemId == a.Id);
        List<TodoItem> parent = new(TodoItems.Where(a => child.ParentId == a.Id));
        TodoItems.Remove(child);

        foreach (TodoItem item in parent) {
            await RemoveToDoAsync(item.Id);
        }

        if (response.IsSuccessStatusCode) {
            return "Deleted successfully";
        } else {
            return "Failed to remove item";
        }
    }

    public async Task InitializeTodosAsync() {
        var response = await client.GetAsync("todo");
        IEnumerable<TodoItem>? items = await response.Content.ReadFromJsonAsync<IEnumerable<TodoItem>>();
        TodoItems = items == null ? new() : new(items);
    }
}