using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Consilium.Shared.Services;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;

namespace Consilium.Shared.ViewModels;
public partial class ProfileViewModel : ObservableObject {
    public ProfileViewModel(ILogInService logInService, IPersistenceService persistenceService) {
        this.logInService = logInService;
        this.persistenceService = persistenceService;
    }

    [ObservableProperty]
    private bool loggedIn = false;
    [ObservableProperty]
    private bool showLoggedIn;

    [ObservableProperty]
    private string emailInput = String.Empty;

    [ObservableProperty]
    private string token = String.Empty;

    [ObservableProperty]
    private string? message;
    private readonly ILogInService logInService;
    private readonly IPersistenceService persistenceService;

    [RelayCommand]
    private async Task LogIn() {
        if (String.IsNullOrEmpty(EmailInput)) return;

        Token = await logInService.LogIn(EmailInput);
        persistenceService.SaveToken(EmailInput, Token);

        if (Token != "Too many unauthorized keys") ShowLoggedInPopup();
        LoggedIn = await persistenceService.CheckStatus();
        ShowLoggedIn = !LoggedIn;
        if (LoggedIn) Message = "You successfully Logged In!";

        // rather than just a label, we can make the login feedback snackbar or toast notifications
    }
    [RelayCommand]
    private void ShowLoggedInPopup() {
        WeakReferenceMessenger.Default.Send(new ShowPopupMessage());
    }
    public async Task InitializeAsync() {
        LoggedIn = await persistenceService.CheckStatus();
        ShowLoggedIn = !LoggedIn;
    }
}