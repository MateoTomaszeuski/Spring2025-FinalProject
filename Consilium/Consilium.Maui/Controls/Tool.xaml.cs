namespace Consilium.Maui.Controls;

public partial class Tools : ContentView
{
    public Tools()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title),
        typeof(string),
        typeof(Tools),
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (Tools)bindable;

            string title = (string)newValue;
            control.ToolTitle.Text = title;
            switch (title)
            {
                case "Notes":
                    control.ToolDescription.Text = "Notes";
                    break;
                case "Calculator":
                    control.ToolDescription.Text = "Calculator";
                    break;
                case "Pomodoro":
                    control.ToolDescription.Text = "Pomodoro";
                    break;
                default:
                    control.ToolDescription.Text = "Notes";
                    break;
            };
        });

    public string Title
    {
        get => GetValue(TitleProperty) as string;
        set => SetValue(TitleProperty, value);
    }
}