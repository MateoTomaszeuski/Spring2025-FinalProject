using Consilium.Shared.ViewModels;

namespace Consilium.Maui.Controls;

public partial class MessagesView : ContentView {
    public MessagesView() {
        InitializeComponent();
        BindingContext = ((App)Application.Current).Services.GetService<MessagesViewModel>();
    }

    public static readonly BindableProperty ConversationWithProperty = BindableProperty.Create(
        propertyName: nameof(ConversationWith),
        returnType: typeof(string),
        declaringType: typeof(MessagesView),
        defaultValue: string.Empty,
        propertyChanged: OnConversationWithChanged);

    private static void OnConversationWithChanged(BindableObject bindable, object oldValue, object newValue) {
        var control = (MessagesView)bindable;
        // Update the view model's property if the BindingContext is of type MessagesViewModel.
        if (control.BindingContext is MessagesViewModel viewModel) {
            viewModel.ConversationWith = newValue as string;
        }
    }
    public string ConversationWith {
        get => (string)GetValue(ConversationWithProperty);
        set => SetValue(ConversationWithProperty, value);
    }
}