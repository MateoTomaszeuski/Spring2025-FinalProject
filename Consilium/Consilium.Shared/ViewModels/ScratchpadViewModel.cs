using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;
using SkiaSharp;


namespace Consilium.Shared.ViewModels;


    public class ScratchpadViewModel : ObservableObject {
    private SKPath _currentPath;
    private ObservableCollection<SKPath> _drawingPaths;

    public ObservableCollection<SKPath> DrawingPaths {
        get => _drawingPaths;
        set => SetProperty(ref _drawingPaths, value);
    }

    public ScratchpadViewModel() {
        _drawingPaths = new ObservableCollection<SKPath>(); // Initialize the collection of paths
    }

    public void StartDrawing(float x, float y) {
        _currentPath = new SKPath();  // Start a new path
        _currentPath.MoveTo(x, y);    // Move to the touch point
    }

    public void UpdateDrawing(float x, float y) {
        _currentPath?.LineTo(x, y);  // Add a line to the current path as the user moves
    }

    public void EndDrawing() {
        if (_currentPath != null) {
            DrawingPaths.Add(_currentPath);  // Add the completed path to the collection
            _currentPath = null;  // Reset the current path
        }
    }

    public void ClearCanvas() {
        DrawingPaths.Clear();  // Clear all paths
    }
}
}