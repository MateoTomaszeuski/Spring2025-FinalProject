using Consilium.Maui.ViewModels;
namespace Consilium.Maui.Views;
public partial class AssignmentsView : ContentPage
{
	public AssignmentsView(AssignmentsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}