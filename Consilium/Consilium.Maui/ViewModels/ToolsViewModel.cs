using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Consilium.Maui.ViewModels;

public partial class ToolsViewModel : ObservableObject
{
    [ObservableProperty]
    private bool notesActive = false;
    [ObservableProperty]
    private bool calculatorActive = false;
    [ObservableProperty]
    private bool pomodoroActive = false;

    [RelayCommand]
    public void ChangeTool(string tool)
    {
        NotesActive = "Notes" == tool;
        CalculatorActive = "Calculator" == tool;
        PomodoroActive = "Pomodoro" == tool;
    }
}
