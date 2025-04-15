using Consilium.Shared.ViewModels;

namespace Consilium.Maui.Views;

public partial class MessagesView : ContentPage {
    public MessagesView(MessagesViewModel vm) {
        InitializeComponent();
        BindingContext = vm;
    }
}