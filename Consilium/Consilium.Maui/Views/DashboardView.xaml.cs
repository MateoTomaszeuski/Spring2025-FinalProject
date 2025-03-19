using Consilium.Maui.ViewModels;
namespace Consilium.Maui.Views;
public partial class DashboardView : ContentPage
{
	public DashboardView(DashboardViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}