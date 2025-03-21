using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Consilium.Maui.ViewModels;

public partial class ToolsViewModel : ObservableObject
{
    public ToolsViewModel()
    {
        PropertyChanged += OnActiveToolChanged
    }
    [ObservableProperty]
    private string activeTool = "Notes";

    [ObservableProperty]
    private bool notesActive = false;
    [ObservableProperty]
    private bool calculatorActive = false;
    [ObservableProperty]
    private bool pomodoroActive = false;

    private void OnActiveToolChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ActiveTool))
        {
            Console.WriteLine($"ActiveTool changed to: {ActiveTool}");
        }
    }

    [RelayCommand]
    public void ChangeTool(string tool)
    {
        switch (tool)
        {
            case "Notes":
                ActiveTool = "Notes";
                break;
            case "Calculator":
                ActiveTool = "Calculator";
                break;
            case "Pomodoro":
                ActiveTool = "Pomodoro";
                break;
            default:
                ActiveTool = "Notes";
                break;
        }
        ;
    }
}
