using Eto.Forms;
using ScottPlot;

namespace Sandbox.Eto;

partial class MainWindow : Form
{
    public MainWindow()
    {
        InitializeComponent();

        _etoPlot1.UserInputProcessor.IsEnabled = true;

        _etoPlot1.Plot.Add.Signal(Generate.Sin());
        _etoPlot1.Plot.Add.Signal(Generate.Cos());
    }
}
