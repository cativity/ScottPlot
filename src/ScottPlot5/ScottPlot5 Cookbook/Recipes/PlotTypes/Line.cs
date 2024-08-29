using JetBrains.Annotations;
using ScottPlot.Colormaps;

namespace ScottPlotCookbook.Recipes.PlotTypes;

[UsedImplicitly]
public class LinePlot : ICategory
{
    public string Chapter => "Plot Types";

    public string CategoryName => "Line Plot";

    public string CategoryDescription => "Line plots can be placed on the plot in coordinate space using a Start, End, and an optional LineStyle.";

    public class LineQuickStart : RecipeBase
    {
        public override string Name => "Line Plot Quickstart";

        public override string Description => "Line plots are placed with a start and end location in coordinate space. Their styles can be customized.";

        [Test]
        public override void Execute()
        {
            MyPlot.Add.Line(1, 12, 12, 0);
            MyPlot.Add.Line(7, 9, 42, 9);
            MyPlot.Add.Line(30, 17, 30, 1);
        }
    }

    public class LinePlotStyles : RecipeBase
    {
        public override string Name => "Line Plot Shapes";

        public override string Description => "Line plots can be styled using a LineStyle.";

        [Test]
        public override void Execute()
        {
            Viridis colormap = new Viridis();

            for (int i = 0; i < 10; i++)
            {
                // add a line
                Coordinates start = Generate.RandomCoordinates();
                Coordinates end = Generate.RandomCoordinates();
                ScottPlot.Plottables.LinePlot line = MyPlot.Add.Line(start, end);

                // customize the line
                line.LineColor = Generate.RandomColor(colormap);
                line.LineWidth = Generate.RandomInteger(1, 4);
                line.LinePattern = Generate.RandomLinePattern();

                // customize markers
                line.MarkerLineColor = line.LineStyle.Color;
                line.MarkerShape = Generate.RandomMarkerShape();
                line.MarkerSize = Generate.RandomInteger(5, 15);
            }
        }
    }

    public class LinePlotLegendQWER : RecipeBase
    {
        public override string Name => "Line Plot Legend";

        public override string Description => "Line plots with labels appear in the legend.";

        [Test]
        public override void Execute()
        {
            ScottPlot.Plottables.Signal sin = MyPlot.Add.Signal(Generate.Sin());
            sin.LegendText = "Sine";

            ScottPlot.Plottables.Signal cos = MyPlot.Add.Signal(Generate.Cos());
            cos.LegendText = "Cosine";

            ScottPlot.Plottables.LinePlot line = MyPlot.Add.Line(1, 12, 12, 0);
            line.LineWidth = 3;
            line.MarkerSize = 10;
            line.LegendText = "Line Plot";

            MyPlot.ShowLegend(Alignment.UpperRight);
        }
    }
}
