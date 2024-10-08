﻿using System.Windows;
using System.Windows.Input;
using ScottPlot;
using ScottPlot.Plottables;

namespace WPF_Demo.DemoWindows;

public partial class DisplayScaling : Window, IDemoWindow
{
    public string DemoTitle => "Display Scaling";

    public string Description => "Demonstrates how to track mouse position on displays which use DPI scaling.";

    private readonly Crosshair _crosshair;

    public DisplayScaling()
    {
        InitializeComponent();

        WpfPlot1.Plot.Add.Signal(Generate.Sin());
        WpfPlot1.Plot.Add.Signal(Generate.Cos());
        _crosshair = WpfPlot1.Plot.Add.Crosshair(0, 0);

        MouseMove += DisplayScalingMouseMove;
    }

    private void DisplayScalingMouseMove(object sender, MouseEventArgs e)
    {
        Point p = e.GetPosition(WpfPlot1);
        Pixel mousePixel = new Pixel(p.X * WpfPlot1.DisplayScale, p.Y * WpfPlot1.DisplayScale);
        _crosshair.Position = WpfPlot1.Plot.GetCoordinates(mousePixel);
        WpfPlot1.Refresh();
    }
}
