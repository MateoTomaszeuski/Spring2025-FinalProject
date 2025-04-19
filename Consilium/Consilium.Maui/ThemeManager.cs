using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Maui.Controls;
using Consilium.Maui.Resources.Themes;


namespace Consilium.Maui;

public static class ThemeManager {
    public static void ApplyTheme(string themeName) {
        ResourceDictionary themeDictionary = themeName switch {
            "GreenTheme" => new GreenTheme(),
            "BlueTheme" => new BlueTheme(),
            _ => new GreenTheme()
        };

        if (Application.Current?.Resources.MergedDictionaries is IList<ResourceDictionary> dicts) {
            // Remove previous theme dictionaries
            for (int i = dicts.Count - 1; i >= 0; i--) {
                if (dicts[i].GetType().Name.EndsWith("Theme"))
                    dicts.RemoveAt(i);
            }

            dicts.Add(themeDictionary);
        }
    }
}

