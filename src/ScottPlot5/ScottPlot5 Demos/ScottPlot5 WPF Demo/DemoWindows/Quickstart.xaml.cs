using System.Windows;
using ScottPlot;

namespace WPF_Demo.DemoWindows;

public partial class Quickstart : Window, IDemoWindow
{
    public string DemoTitle => "WPF Quickstart";

    public string Description => "Create a simple plot using the WPF control.";

    public Quickstart()
    {
        InitializeComponent();

        WpfPlot1.Plot.Add.Signal(Generate.Sin());
        WpfPlot1.Plot.Add.Signal(Generate.Cos());
    }
}
