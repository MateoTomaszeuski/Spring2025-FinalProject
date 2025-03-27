using System.Collections.ObjectModel;

namespace Consilium.Shared.Models;

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