using System.Windows.Forms;
using ScottPlot;
using ScottPlot.Plottables;
using ScottPlot.WinForms;

namespace Sandbox.WinFormsFramework;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        FormsPlot formsPlot1 = new FormsPlot { Dock = DockStyle.Fill };
        Controls.Add(formsPlot1);

        formsPlot1.Plot.Add.Signal(Generate.Sin());
        formsPlot1.Plot.Add.Signal(Generate.Cos());

        Annotation an = formsPlot1.Plot.Add.Annotation("test", Alignment.UpperCenter);

        formsPlot1.Plot.RenderManager.RenderStarting += (sender, rp) =>
        {
            AxisLimits thisRenderLimits = rp.Plot.Axes.GetLimits();
            AxisLimits lastRenderLimits = rp.Plot.LastRender.AxisLimits;

            // test this by resizing the window
            an.Text = thisRenderLimits == lastRenderLimits ? "limits unchanged" : thisRenderLimits.ToString(2);
        };
    }
}
