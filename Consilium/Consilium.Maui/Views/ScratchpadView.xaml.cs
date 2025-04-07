using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics; 
using System.Collections.Generic;

namespace Consilium.Maui.Views; 
public partial class ScratchpadView : ContentPage {
    //private List<Point> _points = new List<Point>();

    public ScratchpadView() {
        InitializeComponent();
        //    DrawingCanvas.PaintSurface += OnCanvasPaintSurface;  
        //}

        //private void OnCanvasPaintSurface(object sender, PaintSurfaceEventArgs e) {
        //    var canvas = e.Surface.Canvas;
        //    canvas.Clear();  

        //    //var paint = new SolidColorPaint {
        //    //    Color = Colors.Black,  
        //    //    StrokeSize = 5  
        //    //};

        //    if (_points.Count > 1) {
        //        canvas.DrawPolyline(_points, paint);
        //    }
        //}
    }
    //private void OnPanUpdated(object sender, PanUpdatedEventArgs e) {
    //    if (e.StatusType == GestureStatus.Running) {
    //        _points.Add(new Point(e.TotalX, e.TotalY));

    //        DrawingCanvas.Invalidate();
    //    }
    //}

    //private void OnClearClicked(object sender, EventArgs e) {
        //_points.Clear();
        //DrawingCanvas.Invalidate();  
    }

