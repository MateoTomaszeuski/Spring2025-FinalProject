using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Consilium.Shared.Models;

public partial class TodoItem : ObservableObject, IEquatable<TodoItem> {
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public bool IsCompleted { get; set; }
    public int? ParentId { get; set; }

    [ObservableProperty]
    private bool isExpanded;

    // property for CategoryId?

    public ObservableCollection<TodoItem> SubTasks { get; set; } = new();

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