using Consilium.Shared.ViewModels.controls;

namespace Consilium.Maui.Controls;

public partial class NotesView : ContentView {
    public NotesView() {
        InitializeComponent();
        BindingContext = new NotesViewModel();
    }
}