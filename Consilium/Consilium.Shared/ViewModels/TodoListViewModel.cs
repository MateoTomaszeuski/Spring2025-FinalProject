﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;

using System.Windows.Input;

namespace Consilium.Shared.ViewModels;

public partial class TodoListViewModel: ObservableObject {

    [ObservableProperty]
    private string newTodoTitle;

    [ObservableProperty]
    private ObservableCollection<TodoItem> todoItems;

    [ObservableProperty]
    private ObservableCollection<string> descriptions;

    public TodoListViewModel()
    {
        TodoItems = new ObservableCollection<TodoItem>();
        Descriptions = new ObservableCollection<string>
        {
            "Work Task",
            "Personal Task",
            "School Task",
        };
    }
    [RelayCommand]
    private void AddTodo()
    {
        if (!string.IsNullOrWhiteSpace(NewTodoTitle))
        {
            TodoItems.Add(new TodoItem() { Title= NewTodoTitle});
            NewTodoTitle = string.Empty; 
        }
    }

    [RelayCommand]
    private void RemoveTodo(TodoItem todoItem)
    {
        if (todoItem != null)
        {
            Console.WriteLine($"Removing Todo: {todoItem}"); 
            TodoItems.Remove(todoItem);
        }
    }
}



public class TodoItem : IEquatable<TodoItem>
{
    public string ? Title { get; set; }
    public string ? Description { get; set; }
    public bool IsCompleted { get; set; }

    public bool Equals(TodoItem? other) {
        return Title == other.Title;
    }
}

//Custom Categories not descriptions
// need Nested Todo items 
// add an assginment id wich can be empty
// be able to sort by category
// be able to sort by completion
