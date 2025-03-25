using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Consilium.Shared.ViewModels;

public partial class TodoListViewModel : ObservableObject {
    [ObservableProperty]
    private string newTodoTitle;

    [ObservableProperty]
    private ObservableCollection<TodoItem> todoItems;

    [ObservableProperty]
    private string newCategoryInput;

    public TodoListViewModel() {
        TodoItems = new ObservableCollection<TodoItem>();
    }

    [RelayCommand]
    private void AddTodo() {
        if (!string.IsNullOrWhiteSpace(NewTodoTitle)) {
            TodoItems.Add(new TodoItem() { Title = NewTodoTitle });
            NewTodoTitle = string.Empty;
        }
    }

    [RelayCommand]
    private void RemoveTodo(TodoItem todoItem) {
        if (todoItem != null) {
            Console.WriteLine($"Removing Todo: {todoItem}");
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
    [RelayCommand]
    private void AddSubTask(TodoItem parentTask) {
        if (parentTask != null && !string.IsNullOrWhiteSpace(NewTodoTitle)) {
            var subTask = new TodoItem { Title = NewTodoTitle };
            parentTask.SubTasks.Add(subTask);
            NewTodoTitle = string.Empty;
        }
    }
}


public class TodoItem : IEquatable<TodoItem> {
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public bool IsCompleted { get; set; }
    public ObservableCollection<TodoItem> SubTasks { get; set; }

    public TodoItem() {
        SubTasks = new ObservableCollection<TodoItem>();
    }

    public bool Equals(TodoItem? other) {
        if (other == null) return false;
        return Title == other.Title;
    }
}



//Custom Categories not descriptions
// need Nested Todo items 
// add an assginment id wich can be empty
// be able to sort by category
// be able to sort by completion