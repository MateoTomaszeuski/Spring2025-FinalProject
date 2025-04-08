using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Consilium.Shared.Models;
using Consilium.Shared.Services;
using System.Collections.ObjectModel;

namespace Consilium.Shared.ViewModels;
public partial class TodoListViewModel : ObservableObject {
    private IToDoService ToDoService;
    private HttpClient client;
    public TodoListViewModel(IToDoService toDoService) {
        NewTodoCategory = Categories[0];
        client = new();
        TodoItems = new();
        ToDoService = toDoService;
    }
    public async Task InitializeItemsAsync() {
        IsLoading = true;
        await ToDoService.InitializeTodosAsync();
        TodoItems = ToDoService.GetTodoItems();
        IsLoading = false;
    }

    [ObservableProperty]
    private ObservableCollection<string> categories = new ObservableCollection<string>() { "Misc.", "School", "Work" };

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string newTodoTitle = "";

    [ObservableProperty]
    private ObservableCollection<TodoItem> todoItems;

    [ObservableProperty]
    private string newTodoCategory;

    [ObservableProperty]
    private string newSubtaskTitle = "";

    [ObservableProperty]
    private string message = "";

    [ObservableProperty]
    private bool categoryIsSortedAscending;


    [RelayCommand]
    private async Task AddTodo() {
        if (!string.IsNullOrWhiteSpace(NewTodoTitle)) {
            await ToDoService.AddItemAsync(new TodoItem() { Title = NewTodoTitle, Category = NewTodoCategory });
            TodoItems = ToDoService.GetTodoItems();
            NewTodoTitle = string.Empty;
        }
    }

    [RelayCommand]
    private async Task RemoveTodo(TodoItem todoItem) {
        if (todoItem != null) {
            await ToDoService.RemoveToDoAsync(todoItem.Id);
            TodoItems = ToDoService.GetTodoItems();
        }
    }

    [RelayCommand]
    private void SortByCategory() {
        if (CategoryIsSortedAscending) {
            TodoItems = [.. TodoItems.OrderByDescending(item => item.Category)];
            CategoryIsSortedAscending = false;
        } else {
            TodoItems = [.. TodoItems.OrderBy(item => item.Category)];
            CategoryIsSortedAscending = true;
        }
    }

    [RelayCommand]
    private void SortByCompletion() {
        // ascending - puts complete items at the end
        TodoItems = [.. TodoItems.OrderBy(item => item.IsCompleted)];
        CategoryIsSortedAscending = false;
    }

    [RelayCommand]
    private void ToggleSubtaskVisibility(TodoItem parentTask) {
        if (parentTask == null) return;

        if (parentTask.SubtaskEntryIsVisible)
            ToggleSubtaskEntryVisibility(parentTask);

        parentTask.IsExpanded = !parentTask.IsExpanded;
    }

    [RelayCommand]
    private void ToggleSubtaskEntryVisibility(TodoItem parentTask) {
        if (parentTask == null) return;

        foreach (var task in TodoItems) {
            if (task != parentTask && task.SubtaskEntryIsVisible) {
                task.SubtaskEntryIsVisible = false;
            }
        }

        parentTask.SubtaskEntryIsVisible = !parentTask.SubtaskEntryIsVisible;
        NewSubtaskTitle = "";
    }

    [RelayCommand]
    private async Task AddSubtask(TodoItem parentTask) {
        if (parentTask is null || string.IsNullOrWhiteSpace(NewSubtaskTitle)) return;
        await ToDoService.AddItemAsync(new TodoItem { Title = NewSubtaskTitle, ParentId = parentTask.Id });

        TodoItems = ToDoService.GetTodoItems();

        parentTask.IsExpanded = true;
        NewSubtaskTitle = string.Empty;
    }

    [RelayCommand]
    private async Task RemoveSubtask(TodoItem subTask) {
        if (subTask?.ParentId is null) return;

        await ToDoService.RemoveToDoAsync(subTask.Id);
        TodoItems = ToDoService.GetTodoItems();
    }

}