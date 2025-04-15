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
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (MessagesView)bindable;
            control.ConversationLabel.Text = $"Chat with {(string)newValue}";
        }
    );

    public string ConversationWith {
        get => (string)GetValue(ConversationWithProperty);
        set => SetValue(ConversationWithProperty, value);
    }
}