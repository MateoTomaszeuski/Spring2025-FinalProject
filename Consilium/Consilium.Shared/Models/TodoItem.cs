using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Consilium.Shared.Models;

public partial class TodoItem : ObservableObject, IEquatable<TodoItem> {
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public bool IsCompleted { get; set; }
    public TodoItem? ParentTask { get; set; }

    [ObservableProperty]
    public bool hasSubtasks;

    [ObservableProperty]
    private bool isExpanded;

    [ObservableProperty]
    private bool subtaskEntryIsVisible;

    public TodoItem() {
        Subtasks.CollectionChanged += (s, e) => {
            HasSubtasks = Subtasks.Count > 0;
        };
    }


    public ObservableCollection<TodoItem> Subtasks { get; set; } = new();

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