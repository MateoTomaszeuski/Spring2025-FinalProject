﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Consilium.Shared.Services;

namespace Consilium.Shared.ViewModels;
public partial class ProfileViewModel(ILogInService logInService, IPersistenceService persistenceService) : ObservableObject {
    [ObservableProperty]
    private string emailInput = String.Empty;

    [ObservableProperty]
    private string token = String.Empty;

    [ObservableProperty]
    private string? message;

    [RelayCommand]
    private async Task LogIn() {
        if (String.IsNullOrEmpty(EmailInput)) return;

        Token = await logInService.LogIn(EmailInput);
        persistenceService.SaveToken(EmailInput, Token);

        if (Token == "Too many unauthorized keys")
            Message = Token;
        else {
            Message = "Success!";
        }
        // rather than just a label, we can make the login feedback snackbar or toast notifications
    }
}