using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Consilium.Shared.Models;
using Consilium.Shared.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection.Emit;
using System.Windows.Input;

namespace Consilium.Shared.ViewModels;
public partial class TodoListViewModel : ObservableObject {
    private HttpClient client;
    public TodoListViewModel(IToDoService toDoService) {
        client = new();
        TodoItems = new();
        ToDoService = toDoService;
    }
    public async Task InitializeItemsAsync() {
        IsLoading = true;
        await Task.Delay(1);
        //TodoItems = await ToDoService.GetTodoItemsAsync();
        IsLoading = false;
    }

    [ObservableProperty]
    private ObservableCollection<string> categories = new ObservableCollection<string>() { "School", "Work", "Misc." };

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string newTodoTitle = "";

    [ObservableProperty]
    private ObservableCollection<TodoItem> todoItems;

    [ObservableProperty]
    private string newCategoryInput = "";
    private IToDoService ToDoService;

    [ObservableProperty]
    private string message = "";


    [RelayCommand]
    private void AddTodo() {
        if (!string.IsNullOrWhiteSpace(NewTodoTitle)) {
            TodoItems.Add(new TodoItem() { Title = NewTodoTitle });
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
    private void SetCategoryForTodoItem(TodoItem todoItem) {
        if (todoItem != null && !string.IsNullOrWhiteSpace(NewCategoryInput)) {
            todoItem.Category = NewCategoryInput;
            NewCategoryInput = string.Empty;
        }
    }

    [RelayCommand]
    private void SortByCategory() {
        var sortedItems = TodoItems.OrderBy(item => item.Category).ToList();
        TodoItems.Clear();
        foreach (var item in sortedItems) {
            TodoItems.Add(item);
        }
    }

    [RelayCommand]
    private void SortByCompletion() {
        var sortedItems = TodoItems.OrderByDescending(item => item.IsCompleted).ToList();
        TodoItems.Clear();
        foreach (var item in sortedItems) {
            TodoItems.Add(item);
        }
    }

    [ObservableProperty]
    private string newSubtaskTitle = "";


    [RelayCommand]
    private void AddSubtask(TodoItem parentTask) {
        if (parentTask != null && !string.IsNullOrWhiteSpace(NewSubtaskTitle)) {
            var subTask = new TodoItem { Title = NewSubtaskTitle, ParentTask = parentTask };
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
    }

    [RelayCommand]
    private void RemoveSubtask(TodoItem subTask) {
        if (subTask?.ParentTask != null) {
            subTask.ParentTask.Subtasks.Remove(subTask);
        }
    }

}

//Custom Categories not descriptions
// need Nested Todo items 
// add an assginment id wich can be empty
// be able to sort by category
// be able to sort by completion