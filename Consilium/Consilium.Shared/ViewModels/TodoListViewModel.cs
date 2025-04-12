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
        Categories = new ObservableCollection<string>() { "Misc.", "School", "Work" };
        FilterCategories = new ObservableCollection<string>(Categories.Append("All"));
        SelectedSortOption = SortOptions[0];
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

    public ObservableCollection<string> SortOptions { get; } = new() {
        "Category Ascending",
        "Category Descending",
        "Completion"
    };

    [ObservableProperty]
    private string selectedSortOption;

    [ObservableProperty]
    private ObservableCollection<string> categories;


    // appending an additional option so that they can go back to the unfiltered view
    [ObservableProperty]
    private ObservableCollection<string> filterCategories;

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

    [ObservableProperty]
    private string selectedCategory = "Misc.";

    partial void OnSelectedCategoryChanged(string value) {
        if (value == "All") {
            TodoItems = ToDoService.GetTodoItems(); // unfiltered
        } else if (!string.IsNullOrWhiteSpace(value)) {
            TodoItems = ToDoService.GetTodosFilteredByCategory(value);
        }
    }

    partial void OnSelectedSortOptionChanged(string value) {
        if (TodoItems is null || TodoItems.Count < 1) return;
        switch (value) {
            case "Category Ascending":
                TodoItems = ToDoService.GetTodosSortedByCategory(true);
                CategoryIsSortedAscending = true;
                break;
            case "Category Descending":
                TodoItems = ToDoService.GetTodosSortedByCategory(false);
                CategoryIsSortedAscending = false;
                break;
            case "Completion":
                TodoItems = ToDoService.GetTodosSortedByCompletion();
                break;
        }
    }

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
    private void FilterByCategory() {
        if (SelectedCategory == "All") {
            TodoItems = ToDoService.GetTodoItems();
        } else {
            TodoItems = ToDoService.GetTodosFilteredByCategory(SelectedCategory);
        }
    }

    [RelayCommand]
    private void SortByCategory() {
        TodoItems = ToDoService.GetTodosSortedByCategory(!CategoryIsSortedAscending);
        CategoryIsSortedAscending = !CategoryIsSortedAscending;
    }

    [RelayCommand]
    private void SortByCompletion() {
        TodoItems = ToDoService.GetTodosSortedByCompletion();
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