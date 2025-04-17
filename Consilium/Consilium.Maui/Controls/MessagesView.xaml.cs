using Consilium.Shared.ViewModels;
using System.Collections.Specialized;

namespace Consilium.Maui.Controls;

public partial class MessagesView : ContentView {
    private MessagesViewModel vm;

    public MessagesView() {
        InitializeComponent();
        vm = ((App)Application.Current).Services.GetService<MessagesViewModel>();
        BindingContext = vm;

        MessagesCollectionView.Loaded += MessagesCollectionView_Loaded;
    }

    private void MessagesCollectionView_Loaded(object sender, EventArgs e) {
        // first, scroll in case there are already items
        ScrollToLastMessage();

        // then subscribe to the VM's collection changes
        if (BindingContext is MessagesViewModel vm
         && vm.AllMessages is INotifyCollectionChanged notify) {
            notify.CollectionChanged += OnMessagesCollectionChanged;
        }
    }
    private void OnMessagesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
        // only scroll on inserts
        if (e.Action == NotifyCollectionChangedAction.Add)
            ScrollToLastMessage();
    }

    private void ScrollToLastMessage() {
        // grab the ItemsSource as a list
        if (MessagesCollectionView.ItemsSource is System.Collections.IList list
         && list.Count > 0) {
            // get the last item
            var lastItem = list[list.Count - 1];
            // scroll to it
            MessagesCollectionView.ScrollTo(
                lastItem,
                position: ScrollToPosition.End,
                animate: true
            );
        }
    }
}