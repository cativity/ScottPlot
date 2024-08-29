using Eto.Forms;
using ScottPlot.Eto;

namespace Sandbox.Eto;

public partial class MainWindow : Form
{
    private readonly EtoPlot _etoPlot1 = new EtoPlot();

    private void InitializeComponent()
    {
        Content = _etoPlot1;
        Width = 800;
        Height = 600;
    }
}
