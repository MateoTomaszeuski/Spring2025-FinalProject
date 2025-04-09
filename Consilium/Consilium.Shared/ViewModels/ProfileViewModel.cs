using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Consilium.Shared.Services;

namespace Consilium.Shared.ViewModels;
public partial class ProfileViewModel(ILogInService logInService, IPersistenceService persistenceService) : ObservableObject {
    [ObservableProperty]
    private string emailInput = String.Empty;

    [ObservableProperty]
    private string token = String.Empty;

    [RelayCommand]
    private async Task LogIn() {
        if (String.IsNullOrEmpty(EmailInput)) return;

        Token = await logInService.LogIn(EmailInput);
        persistenceService.SaveToken(EmailInput, Token);
    }
}