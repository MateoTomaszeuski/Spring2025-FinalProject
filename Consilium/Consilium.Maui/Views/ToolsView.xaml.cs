using Consilium.Shared.ViewModels;

namespace Consilium.Maui.Views;
public partial class ToolsView : ContentPage {
    public ToolsView(ToolsViewModel vm) {
        InitializeComponent();
        BindingContext = vm;
    }
}