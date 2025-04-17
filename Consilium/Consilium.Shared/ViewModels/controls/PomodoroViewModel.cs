using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Consilium.Shared.ViewModels.controls;
public partial class PomodoroViewModel : ObservableObject {
    private Timer? _timer;
    private int _currentTimer;

    [ObservableProperty]
    private int workTime = 1200; // Default 20 minutes

    [ObservableProperty]
    private int breakTime = 300; // Default 5 minutes

    [ObservableProperty]
    private string currentAction = "Working";

    [ObservableProperty]
    private bool isTimerRunning = false;

    public PomodoroViewModel() {
        _currentTimer = WorkTime;
    }

    [RelayCommand]
    public void StartTimer() {
        if (IsTimerRunning) return;

        IsTimerRunning = true;
        _timer = new Timer(TimerCallback, null, 0, 1000);
    }

    [RelayCommand]
    public void StopTimer() {
        _timer?.Dispose();
        _timer = null;
        IsTimerRunning = false;
    }

    [RelayCommand]
    public void ResetTimer() {
        StopTimer();
        CurrentAction = "Working";
        _currentTimer = WorkTime;
        OnPropertyChanged(nameof(CurrentTimerDisplay));
    }

    private void TimerCallback(object? state) {
        if (_currentTimer > 0) {
            _currentTimer--;
            OnPropertyChanged(nameof(CurrentTimerDisplay));
        } else {
            SwitchAction();
        }
    }

    private void SwitchAction() {
        if (CurrentAction == "Working") {
            CurrentAction = "Break";
            _currentTimer = BreakTime;
        } else {
            CurrentAction = "Working";
            _currentTimer = WorkTime;
        }
        OnPropertyChanged(nameof(CurrentTimerDisplay));
    }

    public string CurrentTimerDisplay =>
        TimeSpan.FromSeconds(_currentTimer)
               .ToString(@"mm\:ss");
}