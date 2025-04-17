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
    private bool showLogIn;
    [ObservableProperty]
    private bool showUnAuthorized = false;
    [ObservableProperty]
    private bool showLogOut = false;
    [ObservableProperty]
    private string emailInput = String.Empty;
    [ObservableProperty]
    private string token = String.Empty;
    [ObservableProperty]
    private string? message;
    [ObservableProperty]
    private string username = string.Empty;
    private readonly ILogInService logInService;
    private readonly IPersistenceService persistenceService;


    public Func<string, Task>? ShowSnackbarAsync { get; set; }


    [RelayCommand]
    private async Task LogIn() {
        if (String.IsNullOrEmpty(EmailInput)) return;

        Token = await logInService.LogIn(EmailInput);
        persistenceService.SaveToken(EmailInput, Token);

        if (Token != "Too many unauthorized keys") {
            ShowLoggedInPopup();
            LoggedIn = true;
            Username = persistenceService.GetUserName();
            ShowLogIn = false;
            ShowUnAuthorized = !await persistenceService.CheckAuthStatus();
            ShowLogOut = LoggedIn;
            if (ShowSnackbarAsync is not null)
                await ShowSnackbarAsync("Successfully logged in!");
        }
    }
    [RelayCommand]
    private async Task LogOut() {
        await logInService.LogOut();
        LoggedIn = false;
        Username = string.Empty;
        ShowLogIn = true;
        ShowLogOut = false;
        ShowUnAuthorized = false;
        if (ShowSnackbarAsync is not null)
            await ShowSnackbarAsync("Successfully logged out!");
    }

    [RelayCommand]
    private async Task SignOutAllDevices() {
        await logInService.GlobalLogOut();
        Username = string.Empty;
        LoggedIn = false;
    }

    [RelayCommand]
    private async Task CheckUnAuthorized() {
        ShowUnAuthorized = !await persistenceService.CheckAuthStatus() && LoggedIn;
        ShowLogOut = LoggedIn;
    }
    [RelayCommand]
    private void ShowLoggedInPopup() {
        WeakReferenceMessenger.Default.Send(new ShowPopupMessage());
    }
    public async Task InitializeAsync() {
        LoggedIn = persistenceService.CheckLoginStatus();
        ShowUnAuthorized = !await persistenceService.CheckAuthStatus() && LoggedIn;
        ShowLogIn = !LoggedIn;
        Username = persistenceService.GetUserName();
        ShowLogOut = LoggedIn;
    }
}