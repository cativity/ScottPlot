using JetBrains.Annotations;
using ScottPlot.DataGenerators;
using Timer = System.Windows.Forms.Timer;

namespace WinForms_Demo.Demos;

[UsedImplicitly]
public partial class DataLogger2 : Form, IDemoWindow
{
    public string Title => "Data Logger (Extended)";

    public string Description => "Extended Data Logger example that uses a circular buffer for improved performance.";

    private readonly Timer _addNewDataTimer = new Timer { Interval = 10, Enabled = true };
    private readonly Timer _updatePlotTimer = new Timer { Interval = 50, Enabled = true };

    private readonly RandomWalker _walker1 = new RandomWalker(0, multiplier: 0.01);
    private readonly RandomWalker _walker2 = new RandomWalker(1, multiplier: 1000);

    public DataLogger2()
    {
        InitializeComponent();

        _addNewDataTimer.Tick += (_, _) =>
        {
            const int count = 5;

            for (int i = 0; i < count; i++)
            {
                double val1 = _walker1.Next();
                loggerPlotHorz.Logger1.Add(val1);
                loggerPlotVert.Logger1.Add(val1);

                double val2 = _walker2.Next();
                loggerPlotHorz.Logger2.Add(val2);
                loggerPlotVert.Logger2.Add(val2);
            }
        };

        _updatePlotTimer.Tick += (_, _) =>
        {
            loggerPlotHorz.RefreshPlot();
            loggerPlotVert.RefreshPlot();
        };
    }

    private void CbRunningCheckedChanged(object sender, EventArgs e)
    {
        _addNewDataTimer.Enabled = cbRunning.Checked;
        _updatePlotTimer.Enabled = cbRunning.Checked;
        loggerPlotHorz.Tracking = !cbRunning.Checked;
        loggerPlotVert.Tracking = !cbRunning.Checked;
    }
}
