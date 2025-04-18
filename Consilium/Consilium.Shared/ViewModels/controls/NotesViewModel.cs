using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Consilium.Shared.Models;
using System.Collections.ObjectModel;
using System.Text.Json;


namespace Consilium.Shared.ViewModels.Controls;

public partial class NotesViewModel : ObservableObject {
    private string NotesKey = "saved_notes";

    [ObservableProperty]
    private string title;

    [ObservableProperty]
    private string content;

    public ObservableCollection<Note> Notes { get; } = new();

    [RelayCommand]
    private void AddNote() {
        if (!string.IsNullOrWhiteSpace(Title) || !string.IsNullOrWhiteSpace(Content)) {
            Notes.Add(new Note { Title = Title, Content = Content });
            Title = string.Empty;
            Content = string.Empty;
        }
    }
    [RelayCommand]
    private void DeleteNote(Note note) {
        if (Notes.Contains(note)) {
            Notes.Remove(note);
        }
    }
}