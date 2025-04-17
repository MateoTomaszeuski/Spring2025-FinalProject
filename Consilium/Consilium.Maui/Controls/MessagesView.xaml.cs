using Consilium.Shared.ViewModels;

namespace Consilium.Maui.Controls;

public partial class MessagesView : ContentView {
    private MessagesViewModel vm;

    public MessagesView() {
        InitializeComponent();
        vm = ((App)Application.Current).Services.GetService<MessagesViewModel>();
        BindingContext = vm;
        ScrollToBottom();

        BindingContextChanged += (_, _) =>
        {
            if (vm != null) {
                vm.MessagesUpdated -= ScrollToBottom;
                vm.MessagesUpdated += ScrollToBottom;
            }
        };
    }

    private void MessageContent_Completed(object sender, EventArgs e) {
        if (vm.SendMessageCommand.CanExecute(null) == true) {
            vm.SendMessageCommand.Execute(null);
        }
    }

    // this is scrolling to the top and I don't know why...
    private void ScrollToBottom() {
        if (vm?.AllMessages?.Count > 0) {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Task.Delay(100);

                var index = vm.AllMessages.Count - 1;
                Console.WriteLine($"Trying to scroll to index: {index}");

                MessagesCollectionView.ScrollTo(index, position: ScrollToPosition.End, animate: false);
            });
        }
    }

    private void MessagesCollectionView_Loaded(object sender, EventArgs e) {
        ScrollToBottom();
    }
}