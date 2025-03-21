using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;

using System.Windows.Input;

namespace Consilium.Maui.ViewModels;

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
            TodoItems.Add(new TodoItem { Title = NewTodoTitle });
            NewTodoTitle = string.Empty; 
        }
    }

    [RelayCommand]
    private void RemoveTodo(TodoItem todoItem)
    {
        if (todoItem != null)
        {
            Console.WriteLine($"Removing Todo: {todoItem.Title}"); 
            TodoItems.Remove(todoItem);
        }
    }
}



public class TodoItem : ObservableObject
{
    private string ? title;
    private string ? description;
    private bool isCompleted;

    public string Title
    {
        get => title;
        set => SetProperty(ref title, value);
    }

    public string Description
    {
        get => description;
        set => SetProperty(ref description, value);
    }

    public bool IsCompleted
    {
        get => isCompleted;
        set => SetProperty(ref isCompleted, value);
    }
}
