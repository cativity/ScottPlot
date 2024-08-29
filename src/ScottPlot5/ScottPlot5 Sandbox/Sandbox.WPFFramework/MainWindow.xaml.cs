using System.Windows;
using ScottPlot;

namespace Sandbox.WPFFramework;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        WpfPlot1.Plot.Add.Signal(Generate.Sin());
        WpfPlot1.Plot.Add.Signal(Generate.Cos());
    }
}
