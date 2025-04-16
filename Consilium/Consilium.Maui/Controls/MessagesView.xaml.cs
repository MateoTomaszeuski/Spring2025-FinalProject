using Consilium.Shared.ViewModels;

namespace Consilium.Maui.Controls;

public partial class MessagesView : ContentView {
    public MessagesView() {
        InitializeComponent();
        var vm = ((App)Application.Current).Services.GetService<MessagesViewModel>();
        BindingContext = vm;
    }
}