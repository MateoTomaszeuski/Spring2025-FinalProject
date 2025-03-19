using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consilium.Maui.ViewModels;

public partial class ToolsViewModel : ObservableObject
{
    [ObservableProperty]
    private string activeTool = "Notes";

    [RelayCommand]
    public void ChangeToolCommand(string tool)
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
