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
        //await ToDoService.InitializeTodosAsync();
        //TodoItems = ToDoService.GetTodoItemsAsync();
        await Task.CompletedTask;
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
    private string message = "";

    private bool categoryIsSortedAscending;


    [RelayCommand]
    private void AddTodo() {
        if (!string.IsNullOrWhiteSpace(NewTodoTitle)) {
            TodoItems.Add(new TodoItem() { Title = NewTodoTitle, Category = NewTodoCategory });
            NewTodoTitle = string.Empty;
        }
    }


    // this method is for when the API/service is working (--Audrey)

    //[RelayCommand]
    //private async Task RemoveTodoAsync(TodoItem todoItem) {
    //    if (todoItem != null) {
    //        Console.WriteLine($"Removing Todo: {todoItem}");
    //        int index = TodoItems.IndexOf(todoItem);
    //        string localMessage = await ToDoService.RemoveToDoAsync(index);
    //        Message = localMessage;
    //        if (localMessage == "Deleted successfully") {
    //            TodoItems.Remove(todoItem);
    //    }
    //}
    //}


    [RelayCommand]
    private void RemoveTodo(TodoItem todoItem) {
        if (todoItem != null) {
            Console.WriteLine($"Removing Todo: {todoItem}");
            int index = TodoItems.IndexOf(todoItem);
            TodoItems.Remove(todoItem);
        }
    }

    [RelayCommand]
    private void SortByCategory() {
        if (categoryIsSortedAscending) {
            TodoItems = [.. TodoItems.OrderByDescending(item => item.Category)];
            categoryIsSortedAscending = false;
        } else {
            TodoItems = [.. TodoItems.OrderBy(item => item.Category)];
            categoryIsSortedAscending = true;
        }
    }

    [RelayCommand]
    private void SortByCompletion() {
        // ascending - puts complete items at the end
        TodoItems = [.. TodoItems.OrderBy(item => item.IsCompleted)];
        categoryIsSortedAscending = false;
    }

    [ObservableProperty]
    private string newSubtaskTitle = "";


    [RelayCommand]
    private void AddSubtask(TodoItem parentTask) {
        if (parentTask != null && !string.IsNullOrWhiteSpace(NewSubtaskTitle)) {
            var subTask = new TodoItem { Title = NewSubtaskTitle, ParentId = parentTask.Id };
            parentTask.Subtasks.Add(subTask);
            parentTask.IsExpanded = true;
            NewSubtaskTitle = string.Empty;
        }
    }

    [RelayCommand]
    private void ToggleSubtaskVisibility(TodoItem parentTask) {
        if (parentTask != null) {

            if (parentTask.SubtaskEntryIsVisible)
                ToggleSubtaskEntryVisibility(parentTask);

            parentTask.IsExpanded = !parentTask.IsExpanded;
        }
    }

    [RelayCommand]
    private void ToggleSubtaskEntryVisibility(TodoItem parentTask) {
        if (parentTask != null) {
            parentTask.SubtaskEntryIsVisible = !parentTask.SubtaskEntryIsVisible;
        }

        NewSubtaskTitle = "";
    }

    [RelayCommand]
    private void RemoveSubtask(TodoItem subTask) {
        if (subTask?.ParentId != null) {
            TodoItem parentTask = TodoItems.First(a => a.Id == subTask.ParentId);
            parentTask.Subtasks.Remove(subTask);
        }
    }

}