using System.Windows;

namespace ScottPlot.WPF;

public static class WpfPlotViewer
{
    public static void Launch(Plot plot, string title = "", int width = 600, int height = 400)
    {
        WpfPlot wpfPlot = new WpfPlot();
        wpfPlot.Reset(plot);

        Window win = new Window
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            Width = width,
            Height = height,
            Title = title,
            Content = wpfPlot,
        };

        win.ShowDialog();
    }
}
