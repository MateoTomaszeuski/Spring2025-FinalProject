using Consilium.Shared.ViewModels;

namespace Consilium.Maui.Controls;

public partial class MessagesView : ContentView {
    public MessagesView() {
        InitializeComponent();
        BindingContext = ((App)Application.Current).Services.GetService<MessagesViewModel>();
    }

    public static readonly BindableProperty ConversationWithProperty = BindableProperty.Create(
        nameof(ConversationWith),
        typeof(string),
        typeof(MessagesView),
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (MessagesView)bindable;
            control.ConversationLabel.Text = $"Chat with {(string)newValue}";
        }
    );

    public string ConversationWith {
        get => GetValue(ConversationWithProperty) as string;
        set => SetValue(ConversationWithProperty, value);
    }
}