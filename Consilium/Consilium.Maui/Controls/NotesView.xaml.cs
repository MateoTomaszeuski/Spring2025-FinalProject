using Consilium.Shared.ViewModels.Controls;

namespace Consilium.Maui.Controls;

public partial class NotesView : ContentView {
    public NotesView() {
        InitializeComponent();
        BindingContext = ((App)Application.Current).Services.GetService<NotesViewModel>();
    }
}