using Consilium.Shared.ViewModels;
namespace Consilium.Maui.Views;
public partial class TodoListView : ContentPage {
    private TodoListViewModel vm;
    public TodoListView(TodoListViewModel vm) {
        InitializeComponent();
        this.vm = vm;
        BindingContext = vm;
        Appearing += async (s, e) =>
        {
            await vm.InitializeItemsAsync();
        };

    }
    private void NewTodoTitle_Completed(object sender, EventArgs e) {
        if (vm.AddTodoCommand.CanExecute(null) == true) {
            vm.AddTodoCommand.Execute(null);
        }
    }
}