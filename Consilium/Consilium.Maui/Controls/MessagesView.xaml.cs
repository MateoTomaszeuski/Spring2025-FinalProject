using Consilium.Shared.ViewModels;

namespace Consilium.Maui.Controls;

public partial class MessagesView : ContentView {
    public MessagesView() {
        InitializeComponent();
        BindingContext = ((App)Application.Current).Services.GetService<MessagesViewModel>();
    }
}