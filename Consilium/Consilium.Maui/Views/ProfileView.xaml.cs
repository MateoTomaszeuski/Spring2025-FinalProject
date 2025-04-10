using Consilium.Shared.ViewModels;

namespace Consilium.Maui.Views;

public partial class ProfileView : ContentPage {
    public ProfileView(ProfileViewModel vm) {
        InitializeComponent();
        BindingContext = vm;
    }
}