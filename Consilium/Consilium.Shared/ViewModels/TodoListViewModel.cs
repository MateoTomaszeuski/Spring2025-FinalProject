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
        //_ = SpinAsync();
        TodoItems = await ToDoService.GetTodoItemsAsync();
        IsLoading = false;
    }


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

    [RelayCommand]
    private async Task RemoveTodoAsync(TodoItem todoItem) {
        if (todoItem != null) {
            Console.WriteLine($"Removing Todo: {todoItem}");
            int index = TodoItems.IndexOf(todoItem);
            Message = await ToDoService.RemoveToDoAsync(index);
            if (Message == "Deleted successfully") {
                TodoItems.Remove(todoItem);
            }
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
    [RelayCommand]
    private void AddSubTask(TodoItem parentTask) {
        if (parentTask != null && !string.IsNullOrWhiteSpace(NewTodoTitle)) {
            var subTask = new TodoItem { Title = NewTodoTitle };
            parentTask.SubTasks.Add(subTask);
            NewTodoTitle = string.Empty;
        }
    }

    //private async Task SpinAsync() {
    //    while (IsLoading) {
    //        await SpinIcon.RotateTo(360, 1000);
    //        SpinIcon.Rotation = 0;
    //    }
    //}

    //public Label SpinIcon { get; set; }

}



//Custom Categories not descriptions
// need Nested Todo items 
// add an assginment id wich can be empty
// be able to sort by category
// be able to sort by completion