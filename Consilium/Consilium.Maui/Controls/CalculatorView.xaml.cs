using Consilium.Shared.ViewModels.controls;

namespace Consilium.Maui.Controls;

public partial class CalculatorView : ContentView {
    public CalculatorView() {
        InitializeComponent();
        BindingContext = new CalculatorViewModel();
    }
}