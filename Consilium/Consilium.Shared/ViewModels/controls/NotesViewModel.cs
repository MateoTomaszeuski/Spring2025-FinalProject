﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Consilium.Shared.Models;
using Consilium.Shared.Services;
using System.Collections.ObjectModel;


namespace Consilium.Shared.ViewModels.Controls;

public partial class NotesViewModel : ObservableObject, IDisposable {
    private readonly IPersistenceService service;
    [ObservableProperty]
    private string? title;

    [ObservableProperty]
    private string? content;

    public NotesViewModel(IPersistenceService service) {
        Content = service.GetNotes();
        this.service = service;
    }

    public void Dispose() {
        if (Content is null) Content = "";
        service.SaveNotes(Content);
    }
}