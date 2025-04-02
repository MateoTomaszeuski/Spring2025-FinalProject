using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Consilium.Shared.Models;

public partial class TodoItem : ObservableObject {
    public int Id { get; set; }
    public string? Title { get; set; }
    public bool IsCompleted => CompletionDate is null;
    public int TodoListId { get; set; }
    public int? ParentId { get; set; }
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

    public TodoItem() {
        Subtasks.CollectionChanged += (s, e) =>
        {
            HasSubtasks = Subtasks.Count > 0;
        };
    }


    public ObservableCollection<TodoItem> Subtasks { get; set; } = new();

    //public bool Equals(TodoItem? other) {
    //    if (other == null) return false;

    //    return Title == other.Title && 
    //        IsCompleted == other.IsCompleted && 
    //        //(ParentTask?.Equals(other.ParentTask) ?? false) && 
    //        Category == other.Category && 
    //        Description == other.Description;
    //}
}



//Custom Categories not descriptions
// need Nested Todo items 
// add an assginment id wich can be empty
// be able to sort by category
// be able to sort by completion