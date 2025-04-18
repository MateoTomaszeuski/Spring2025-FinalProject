using Consilium.Shared.ViewModels;

namespace Consilium.Maui.Views;
public partial class ToolsView : ContentPage {
    private readonly ToolsViewModel vm;

    public ToolsView(ToolsViewModel vm) {
        InitializeComponent();
        BindingContext = vm;
        this.vm = vm;
    }
    protected override void OnAppearing() {
        base.OnAppearing();
        OnPropertyChanged(nameof(vm.NotesActive));
        OnPropertyChanged(nameof(vm.CalculatorActive));
        OnPropertyChanged(nameof(vm.PomodoroActive));
        OnPropertyChanged(nameof(vm.CalendarActive));
    }
}