using Consilium.Shared.ViewModels;

namespace Consilium.Maui.Controls;

public partial class MessagesView : ContentView
{
	public MessagesView(MessagesViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}