using Consilium.Shared.ViewModels;
namespace Consilium.Maui.Views;
public partial class AssignmentsView : ContentPage {
    public AssignmentsView(AssignmentsViewModel vm) {
        InitializeComponent();
        BindingContext = vm;
        Appearing += async (s, e) =>
        {
            await vm.InitializeViewModelAsync();
        };
    }
}