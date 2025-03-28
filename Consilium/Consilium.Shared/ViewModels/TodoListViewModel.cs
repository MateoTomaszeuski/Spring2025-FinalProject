﻿using CommunityToolkit.Mvvm.ComponentModel;
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
        //TodoItems = await ToDoService.GetTodoItemsAsync();
        TodoItems = new ObservableCollection<TodoItem>() { new TodoItem() { Id = 1, Title = "Name" }, new TodoItem() { Id = 2, Title = "Item2" } };
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

    [RelayCommand]
    private async Task RemoveTodoAsync(TodoItem todoItem) {
        if (todoItem != null) {
            Console.WriteLine($"Removing Todo: {todoItem}");
            int index = TodoItems.IndexOf(todoItem);
            string localMessage = await ToDoService.RemoveToDoAsync(index);
            Message = localMessage;
            if (localMessage == "Deleted successfully") {
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
            var subTask = new TodoItem { Title = NewTodoTitle, ParentId = parentTask.Id };
            parentTask.SubTasks.Add(subTask);
            NewTodoTitle = string.Empty;
        }
    }

    [RelayCommand]
    private void ToggleSubtaskVisibility(TodoItem parentTask) {
        if (parentTask != null) {
            parentTask.IsExpanded = !parentTask.IsExpanded;
        }
    }
}



//Custom Categories not descriptions
// need Nested Todo items 
// add an assginment id wich can be empty
// be able to sort by category
// be able to sort by completion