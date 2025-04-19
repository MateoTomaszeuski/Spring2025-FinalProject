using CommunityToolkit.Mvvm.ComponentModel;
using Consilium.Shared.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consilium.Shared.ViewModels;
public partial class SettingsViewModel : ObservableObject {
    private readonly IPersistenceService persistence;

    public SettingsViewModel(IPersistenceService persistence) {
        this.persistence = persistence;
        Themes = new ObservableCollection<string> {
            "Green", "Blue", "Purple", "Pink","Teal" };
        SelectedTheme = Themes.FirstOrDefault(t => t == persistence.GetTheme()) ?? "Green";
    }

    [ObservableProperty]
    private ObservableCollection<string> themes;

    [ObservableProperty]
    private string selectedTheme;

    partial void OnSelectedThemeChanged(string value) {
        if (!string.IsNullOrEmpty(value)) {
            persistence.SaveTheme(value);
        }
    }

}