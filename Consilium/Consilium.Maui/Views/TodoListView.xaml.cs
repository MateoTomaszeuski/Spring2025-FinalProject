using Consilium.Maui.ViewModels;
namespace Consilium.Maui.Views;
public partial class TodoListView : ContentPage
{
	public TodoListView(TodoListViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}