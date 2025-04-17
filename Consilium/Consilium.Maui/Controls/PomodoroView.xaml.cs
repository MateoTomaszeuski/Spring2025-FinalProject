using Consilium.Shared.ViewModels;
using Consilium.Shared.ViewModels.controls;
using Plugin.Maui.Audio;

namespace Consilium.Maui.Controls;

public partial class PomodoroView : ContentView {
    private readonly IAudioManager? _audioManager;

    public PomodoroView() {
        InitializeComponent();
        _audioManager = ServiceHelper.GetService<IAudioManager>();
        BindingContext = new PomodoroViewModel();
    }

    public PomodoroView(IAudioManager audioManager) : this() {
        _audioManager = audioManager;
    }

    private async void OnPlaySoundButtonClicked(object sender, EventArgs e) {
        if (_audioManager is null) return;

        using var stream = await FileSystem.OpenAppPackageFileAsync("timer.mp3");
        var player = _audioManager.CreatePlayer(stream);
        player.Volume = 1.0;
        player.Play();
    }
}