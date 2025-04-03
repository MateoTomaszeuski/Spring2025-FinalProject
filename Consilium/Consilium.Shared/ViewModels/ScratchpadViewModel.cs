using CommunityToolkit.Mvvm.ComponentModel;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Consilium.Shared.ViewModels;


public class ScratchpadViewModel : ObservableObject {
    private SKPath? currentPath;
    private ObservableCollection<SKPath> drawingPaths;

    public ObservableCollection<SKPath> DrawingPaths {
        get => drawingPaths;
        set => SetProperty(ref drawingPaths, value);
    }

    public ScratchpadViewModel() {
        drawingPaths = new ObservableCollection<SKPath>(); // Initialize the collection of paths
    }

    public void StartDrawing(float x, float y) {
        currentPath = new SKPath();  // Start a new path
        currentPath.MoveTo(x, y);    // Move to the touch point
    }

    public void UpdateDrawing(float x, float y) {
        currentPath?.LineTo(x, y);  // Add a line to the current path as the user moves
    }

    public void EndDrawing() {
        if (currentPath != null) {
            DrawingPaths.Add(currentPath);  // Add the completed path to the collection
            currentPath = null;  // Reset the current path
        }
    }

    public void ClearCanvas() {
        DrawingPaths.Clear();  // Clear all paths
    }
}