using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Consilium.Shared.Models;

public partial class TodoItem : ObservableObject {
    public int Id { get; set; } // this ID comes from the DB (not sure we even need it)
    public int? ParentId { get; set; }
    public string? Title { get; set; }
    public int TodoListId { get; set; }
    public int? AssignmentId { get; set; }
    public DateTime? CompletionDate { get; set; }

    [ObservableProperty]
    private string? category;

    [ObservableProperty]
    public bool hasSubtasks;

    [ObservableProperty]
    private bool isExpanded;

    [ObservableProperty]
    private bool subtaskEntryIsVisible;

    [ObservableProperty]
    private bool isCompleted;

    partial void OnIsCompletedChanged(bool value) {
        if (value) {
            CompletionDate = DateTime.Now;
        } else {
            CompletionDate = null;
        }
    }

    public TodoItem() {
        Subtasks.CollectionChanged += (s, e) =>
        {
            HasSubtasks = Subtasks.Count > 0;
        };
    }


    public ObservableCollection<TodoItem> Subtasks { get; set; } = new();

    public bool Equals(TodoItem? other) {
        if (other == null) return false;

        return Id == other.Id;
    }
}



//Custom Categories not descriptions
// need Nested Todo items 
// add an assginment id wich can be empty
// be able to sort by category
// be able to sort by completion