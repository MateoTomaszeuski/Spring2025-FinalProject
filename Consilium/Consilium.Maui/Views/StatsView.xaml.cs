using Consilium.Shared.ViewModels;
namespace Consilium.Maui.Views;
public partial class StatsView : ContentPage {
    public StatsView(StatsViewModel vm) {
        InitializeComponent();
        BindingContext = vm;
    }
}