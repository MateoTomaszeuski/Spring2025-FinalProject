

using Consilium.Shared.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Runtime.ExceptionServices;

namespace Consilium.Shared.Services;

public class ToDoService : IToDoService {
    private readonly IPersistenceService persistenceService;
    private readonly IClientService client;
    private readonly ILogInService logInService;
    private readonly bool isTest;

    /// <summary>
    /// Used by tests to assign starting values. DO NOT USE DIRECTLY IN PRODUCTION.
    /// </summary>
    public List<TodoItem> TodoItems { get; set; }

    public ToDoService(IPersistenceService persistenceService, IClientService client, ILogInService logInService, bool isTest = false) {
        this.persistenceService = persistenceService;
        this.client = client;
        this.logInService = logInService;
        this.isTest = isTest;
        TodoItems = new();
    }

    public ObservableCollection<TodoItem> GetTodoItems() {
        return new(ListCollapser.CollapseList(TodoItems));
    }

    public async Task AddItemAsync(TodoItem item) {
        bool online = await logInService.CheckAuthStatus();
        if (online || isTest) {
            var response = await client.PostAsync($"todo", item);
            int id = Convert.ToInt16(await response.Content.ReadAsStringAsync());
            item.Id = id;
        }
        persistenceService.SaveList(TodoItems);
        TodoItems.Add(item);
    }

    /// <summary>
    /// Takes in an item with an adjusted CompletionDate and updates the item in the list with 
    /// that value.
    /// </summary>
    public async Task UpdateItemAsync(TodoItem item) {
        bool online = await logInService.CheckAuthStatus();
        if (online || isTest) {
            var response = await client.PatchAsync($"todo/update", item);
        }
        TodoItem listItem = TodoItems.Where(a => a.Id == item.Id).First();
        listItem.CompletionDate = item.CompletionDate;
        persistenceService.SaveList(TodoItems);
    }

    public async Task<string> RemoveToDoAsync(int itemId) {
        bool online = await logInService.CheckAuthStatus();
        if (online || isTest) {
            var response = await client.DeleteAsync($"todo/remove/{itemId}");
        }

        TodoItem primary = TodoItems.First(a => itemId == a.Id);

        List<TodoItem> children = new(TodoItems.Where(a => a.ParentId == primary.Id));
        TodoItem? parent = TodoItems.Where(a => a.Id == primary.ParentId).FirstOrDefault();

        foreach (TodoItem item in children) {
            await RemoveToDoAsync(item.Id);
        }

        if (parent is not null) parent.Subtasks.Remove(primary);

        TodoItems.Remove(primary);
        persistenceService.SaveList(TodoItems);

        return "Deleted successfully";
    }

    public async Task InitializeTodosAsync() {
        bool online = await logInService.CheckAuthStatus();

        var persistanceItems = persistenceService.GetToDoLists() ?? new List<TodoItem>();
        IEnumerable<TodoItem>? items = null;
        if (online || isTest) {
            var response = await client.GetAsync("todo");
            items = await response.Content.ReadFromJsonAsync<IEnumerable<TodoItem>>();
            SyncLocalWithServer(persistanceItems, items);
            response = await client.GetAsync("todo");
            items = await response.Content.ReadFromJsonAsync<IEnumerable<TodoItem>>();
            if (items == null) {
                items = new List<TodoItem>();
            }
            TodoItems = items.ToList();
            return;
        }

        TodoItems = persistanceItems.ToList();
    }

    private void SyncLocalWithServer(IEnumerable<TodoItem> persistanceItems, IEnumerable<TodoItem>? items) {
        List<TodoItem> toAdd = new();
        foreach (var persistance in persistanceItems) {
            if (items == null || !items.Any(a => a.Id == persistance.Id)) {
                toAdd.Add(persistance);
            }
        }
        if (toAdd.Count > 0) {
            foreach (var item in toAdd) {
                TodoItems.Add(item);
            }
        }
    }

    public ObservableCollection<TodoItem> GetTodosSortedByCategory(bool ascending = true) {
        var sorted = ascending
            ? TodoItems.OrderBy(item => item.Category)
            : TodoItems.OrderByDescending(item => item.Category);
        return new(ListCollapser.CollapseList(sorted));
    }

    public ObservableCollection<TodoItem> GetTodosSortedByCompletion() {
        var sorted = TodoItems.OrderBy(item => item.IsCompleted);
        return new(ListCollapser.CollapseList(sorted));
    }

    public ObservableCollection<TodoItem> GetTodosFilteredByCategory(string category) {
        var filtered = TodoItems.Where(item => item.Category == category);
        return new(ListCollapser.CollapseList(filtered));
    }
}