using JetBrains.Annotations;
using ScottPlot;
using ScottPlot.Plottables;
using Timer = System.Windows.Forms.Timer;

namespace WinForms_Demo.Demos;

[UsedImplicitly]
public partial class HeatmapLive : Form, IDemoWindow
{
    private readonly Heatmap _hMap;
    private readonly Timer _timer;
    private readonly double[,] _heatmapData;
    private int _updateCount;

    public string Title => "Live Heatmap";

    public string Description => "Demonstrates how to display a heatmap with data that changes over time";

    public HeatmapLive()
    {
        InitializeComponent();

        _heatmapData = Generate.Sin2D(23, 13, multiple: 3);
        _hMap = new Heatmap(_heatmapData);
        formsPlot1.Plot.PlottableList.Add(_hMap);

        _timer = new Timer { Enabled = true, Interval = 100 };
        _timer.Tick += (_, _) => ChangeData();

        formsPlot1.Refresh();
    }

    private void ChangeData()
    {
        Text = $"Updated {++_updateCount} times";

        Random rand = new Random();

        for (int y = 0; y < _heatmapData.GetLength(0); y++)
        {
            for (int x = 0; x < _heatmapData.GetLength(1); x++)
            {
                _heatmapData[y, x] += rand.NextDouble() - .5;
            }
        }

        _hMap.Update();
        formsPlot1.Refresh();
    }
}
