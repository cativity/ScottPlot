using JetBrains.Annotations;
using ScottPlot;
using ScottPlot.Plottables;

namespace WinForms_Demo.Demos;

[UsedImplicitly]
public partial class CustomMenu : Form, IDemoWindow
{
    public string Title => "Custom Right-Click Context Menu";

    public string Description => "Demonstrates how to replace the default right-click menu with a user-defined one that performs custom actions.";

    public CustomMenu()
    {
        InitializeComponent();
        BtnCustomClick(this, EventArgs.Empty);
    }

    private void BtnDefaultClick(object sender, EventArgs e)
    {
        // Reset menu to default options
        formsPlot1.Menu.Reset();

        formsPlot1.Plot.Title("Default Right-Click Menu");
        formsPlot1.Refresh();
    }

    private void BtnCustomClick(object sender, EventArgs e)
    {
        // clear existing menu items
        formsPlot1.Menu.Clear();

        // add menu items with custom actions
        formsPlot1.Menu.Add("Add Scatter",
                            static formsplot1 =>
                            {
                                formsplot1.Plot.Add.Scatter(Generate.RandomCoordinates(5));
                                formsplot1.Plot.Axes.AutoScale();
                                formsplot1.Refresh();
                            });

        formsPlot1.Menu.Add("Add Line",
                            static formsplot1 =>
                            {
                                LinePlot line = formsplot1.Plot.Add.Line(Generate.RandomLine());
                                line.LineWidth = 2;
                                line.MarkerSize = 20;
                                formsplot1.Plot.Axes.AutoScale();
                                formsplot1.Refresh();
                            });

        formsPlot1.Menu.Add("Add Text",
                            static formsplot1 =>
                            {
                                Text txt = formsplot1.Plot.Add.Text("Test", Generate.RandomLocation());
                                txt.LabelFontSize = 10 + Generate.RandomInteger(20);
                                txt.LabelFontColor = Generate.RandomColor(128);
                                txt.LabelBold = true;
                                formsplot1.Plot.Axes.AutoScale();
                                formsplot1.Refresh();
                            });

        formsPlot1.Menu.AddSeparator();

        formsPlot1.Menu.Add("Clear",
                            static formsplot1 =>
                            {
                                formsplot1.Plot.Clear();
                                formsplot1.Plot.Axes.AutoScale();
                                formsplot1.Refresh();
                            });

        formsPlot1.Plot.Title("Custom Right-Click Menu");
        formsPlot1.Refresh();
    }
}
