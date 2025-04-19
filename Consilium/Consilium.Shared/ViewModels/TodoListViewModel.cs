using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Consilium.Shared.Models;
using Consilium.Shared.Services;
using System.Collections.ObjectModel;

namespace Consilium.Shared.ViewModels;
public partial class TodoListViewModel : ObservableObject {
    private IToDoService todoService;
    public TodoListViewModel(IToDoService toDoService) {
        Categories = new ObservableCollection<string>() { "Misc.", "School", "Work" };
        FilterCategories = new ObservableCollection<string>(Categories.Append("All"));
        SelectedSortOption = SortOptions[0];
        NewTodoCategory = Categories[0];
        TodoItems = new();
        this.todoService = toDoService;
    }
    public async Task InitializeItemsAsync() {
        IsLoading = true;
        await todoService.InitializeTodosAsync();
        TodoItems = todoService.GetTodoItems();
        if (TodoItems.Count < 1) {
            Message = "No items found.";
        } else {
            Message = string.Empty;
        }
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
    private string selectedCategory = "All";

    partial void OnSelectedCategoryChanged(string value) {
        if (value == "All") {
            TodoItems = todoService.GetTodoItems(); // unfiltered
        } else if (!string.IsNullOrWhiteSpace(value)) {
            TodoItems = todoService.GetTodosFilteredByCategory(value);
        }
    }

    partial void OnSelectedSortOptionChanged(string value) {
        if (TodoItems is null || TodoItems.Count < 1) return;
        switch (value) {
            case "Category Ascending":
                TodoItems = todoService.GetTodosSortedByCategory(true);
                CategoryIsSortedAscending = true;
                break;
            case "Category Descending":
                TodoItems = todoService.GetTodosSortedByCategory(false);
                CategoryIsSortedAscending = false;
                break;
            case "Completion":
                TodoItems = todoService.GetTodosSortedByCompletion();
                break;
        }
    }

    [RelayCommand]
    private async Task AddTodo() {
        if (!string.IsNullOrWhiteSpace(NewTodoTitle)) {
            await todoService.AddItemAsync(new TodoItem(todoService) { Title = NewTodoTitle, Category = NewTodoCategory });
            TodoItems = todoService.GetTodoItems();

            // reapply the filter so users can see the list as they had it before
            OnSelectedCategoryChanged(SelectedCategory);
            NewTodoTitle = string.Empty;
        }
    }

    [RelayCommand]
    private async Task RemoveTodo(TodoItem todoItem) {
        if (todoItem != null) {
            await todoService.RemoveToDoAsync(todoItem.Id);
            TodoItems = todoService.GetTodoItems();
        }
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
        if (parentTask is null || string.IsNullOrWhiteSpace(NewSubtaskTitle) || SelectedCategory is null) return;
        await todoService.AddItemAsync(new TodoItem(todoService) { Title = NewSubtaskTitle, ParentId = parentTask.Id });

        TodoItems = todoService.GetTodoItems();

        parentTask.IsExpanded = true;
        NewSubtaskTitle = string.Empty;
    }

    [RelayCommand]
    private async Task RemoveSubtask(TodoItem subTask) {
        if (subTask?.ParentId is null) return;

        await todoService.RemoveToDoAsync(subTask.Id);
        TodoItems = todoService.GetTodoItems();
    }

    [RelayCommand]
    private async Task DeleteAllCompleted() {
        foreach (var item in TodoItems) {
            if (item.IsCompleted) {
                await todoService.RemoveToDoAsync(item.Id);
            }
        }
        TodoItems = todoService.GetTodoItems();
    }
}