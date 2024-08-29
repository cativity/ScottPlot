using ScottPlot;
using ScottPlot.Plottables;

namespace WinForms_Demo.Demos;

public partial class ImageBackgrounds : Form, IDemoWindow
{
    public string Title => "Background Images";

    public string Description => "Use a bitmap image for the background of the figure or data area";

    private readonly ImagePosition[] _positions = Enum.GetValues<ImagePosition>();

    public ImageBackgrounds()
    {
        InitializeComponent();

        foreach (ImagePosition mode in _positions)
        {
            cbMode.Items.Add(mode);
        }

        cbMode.SelectedIndex = 2;

        cbData.CheckStateChanged += (_, _) => ResetPlot();
        cbFigure.CheckStateChanged += (_, _) => ResetPlot();
        cbMode.SelectionChangeCommitted += (_, _) => ResetPlot();
        ResetPlot();
    }

    public void ResetPlot()
    {
        formsPlot1.Reset();

        // add sample data
        Signal sig1 = formsPlot1.Plot.Add.Signal(Generate.Sin());
        Signal sig2 = formsPlot1.Plot.Add.Signal(Generate.Cos());
        sig1.LineWidth = 5;
        sig2.LineWidth = 5;
        formsPlot1.Plot.YLabel("Vertical Axis");
        formsPlot1.Plot.XLabel("Horizontal Axis");
        formsPlot1.Plot.Title("Plot with Image Background");

        // assign the bitmap image
        formsPlot1.Plot.FigureBackground.Image = cbFigure.Checked ? SampleImages.ScottPlotLogo() : null;
        formsPlot1.Plot.DataBackground.Image = cbData.Checked ? SampleImages.MonaLisa() : null;

        // set the scaling mode
        formsPlot1.Plot.FigureBackground.ImagePosition = _positions[cbMode.SelectedIndex];
        formsPlot1.Plot.DataBackground.ImagePosition = _positions[cbMode.SelectedIndex];

        // force a redraw
        formsPlot1.Refresh();
    }
}
