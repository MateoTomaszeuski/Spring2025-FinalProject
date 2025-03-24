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
            TodoItems.Add(new TodoItem());
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



public class TodoItem
{
    public string ? Title { get; set; }
    public string ? Description { get; set; }
    public bool IsCompleted { get; set; }

}
