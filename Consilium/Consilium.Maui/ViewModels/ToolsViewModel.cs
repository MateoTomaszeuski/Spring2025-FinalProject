using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Consilium.Maui.ViewModels;

public partial class ToolsViewModel : ObservableObject
{
    [ObservableProperty]
    private string activeTool = "Notes";
    [ObservableProperty]
    private bool notesActive = false;
    [ObservableProperty]
    private bool calculatorActive = false;
    [ObservableProperty]
    private bool pomodoroActive = false;

    [RelayCommand]
    public void ChangeTool(string tool)
    {
        NotesActive = false;
        CalculatorActive = false;
        PomodoroActive = false;
        
        switch (tool)
        {
            case "Calculator":
                CalculatorActive = true;
                break;
            case "Notes":
                NotesActive = true;
                break;
            case "Pomodoro":
                PomodoroActive = true;
                break;
            default:
                CalculatorActive = true;
                break;
        }
        ;
    }
}
