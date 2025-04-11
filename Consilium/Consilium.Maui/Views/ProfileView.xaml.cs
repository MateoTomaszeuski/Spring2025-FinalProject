using CommunityToolkit.Maui.Views;
using Consilium.Maui.PopUps;
using Consilium.Shared.ViewModels;

namespace Consilium.Maui.Views;

public partial class ProfileView : ContentPage {
    private ProfileViewModel vm;

    public ProfileView(ProfileViewModel vm) {
        this.vm = vm;
        InitializeComponent();
        BindingContext = vm;
    }

    private void Button_Clicked(object sender, EventArgs e) {
        this.ShowPopup(new LoggedInPopUp());
    }

    private void EmailEntry_Completed(object sender, EventArgs e) {
        if (vm.LogInCommand.CanExecute(null) == true) {
            vm.LogInCommand.Execute(null);
        }
    }
}