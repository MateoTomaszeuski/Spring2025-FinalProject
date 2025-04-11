using CommunityToolkit.Mvvm.ComponentModel;
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

        // if the call returns "too many tokens" message, show an error message (snackbar or toast notification)
    }
}