using Consilium.Shared.ViewModels;

namespace Consilium.Maui.Controls;

public partial class MessagesView : ContentView {
    private MessagesViewModel vm;

    public MessagesView() {
        InitializeComponent();
        vm = ((App)Application.Current).Services.GetService<MessagesViewModel>();
        BindingContext = vm;
    }

    private void MessageContent_Completed(object sender, EventArgs e) {
        if (vm.SendMessageCommand.CanExecute(null) == true) {
            vm.SendMessageCommand.Execute(null);
        }
    }
}