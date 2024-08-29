using JetBrains.Annotations;
using ScottPlot.Plottables;

namespace ScottPlotCookbook.Recipes.PlotTypes;

[UsedImplicitly]
public class ArrowCoordinated : ICategory
{
    public string Chapter => "Plot Types";

    public string CategoryName => "Arrow";

    public string CategoryDescription => "Arrows point to a location in coordinate space.";

    public class ArrowQuickstart : RecipeBase
    {
        public override string Name => "Arrow Quickstart";

        public override string Description => "Arrows can be placed on plots to point to a location in coordinate space and extensively customized.";

        [Test]
        public override void Execute()
        {
            // create a line
            Coordinates arrowTip = new Coordinates(0, 0);
            Coordinates arrowBase = new Coordinates(1, 1);
            CoordinateLine arrowLine = new CoordinateLine(arrowBase, arrowTip);

            // add a simple arrow
            MyPlot.Add.Arrow(arrowLine);

            // arrow line and fill styles can be customized
            Arrow arrow2 = MyPlot.Add.Arrow(arrowLine.WithDelta(1, 0));
            arrow2.ArrowLineColor = Colors.Red;
            arrow2.ArrowMinimumLength = 100;
            arrow2.ArrowLineColor = Colors.Black;
            arrow2.ArrowFillColor = Colors.Transparent;

            // the shape of the arrowhead can be adjusted
            Arrow skinny = MyPlot.Add.Arrow(arrowLine.WithDelta(2, 0));
            skinny.ArrowFillColor = Colors.Green;
            skinny.ArrowLineWidth = 0;
            skinny.ArrowWidth = 3;
            skinny.ArrowheadLength = 20;
            skinny.ArrowheadAxisLength = 20;
            skinny.ArrowheadWidth = 7;

            Arrow fat = MyPlot.Add.Arrow(arrowLine.WithDelta(3, 0));
            fat.ArrowFillColor = Colors.Blue;
            fat.ArrowLineWidth = 0;
            fat.ArrowWidth = 18;
            fat.ArrowheadLength = 20;
            fat.ArrowheadAxisLength = 20;
            fat.ArrowheadWidth = 30;

            // offset backs the arrow away from the tip coordinate
            MyPlot.Add.Marker(arrowLine.WithDelta(4, 0).End);
            Arrow arrow4 = MyPlot.Add.Arrow(arrowLine.WithDelta(4, 0));
            arrow4.ArrowOffset = 15;

            MyPlot.Axes.SetLimits(-1, 6, -1, 2);
        }
    }
}
