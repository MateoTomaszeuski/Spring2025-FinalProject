using Consilium.Shared.ViewModels;
namespace Consilium.Maui.Views;
public partial class TodoListView : ContentPage {
    public TodoListView(TodoListViewModel vm) {
        InitializeComponent();
        BindingContext = vm;
        Appearing += async (s, e) =>
        {
            await vm.InitializeItems();
            InitCompleted?.Invoke(this, EventArgs.Empty);
        };
    }

    public event EventHandler<EventArgs> InitCompleted;
}