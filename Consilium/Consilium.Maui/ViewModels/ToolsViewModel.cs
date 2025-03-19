using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Consilium.Maui.ViewModels;

public partial class ToolsViewModel : ObservableObject
{
    [ObservableProperty]
    private string activeTool = "Notes";

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
        };
    }
}
