using JetBrains.Annotations;
using ScottPlot.AxisRules;
using ScottPlot.Plottables;

namespace ScottPlotCookbook.Recipes.PlotTypes;

[UsedImplicitly]
public class Shapes : ICategory
{
    public string Chapter => "Plot Types";

    public string CategoryName => "Shapes";

    public string CategoryDescription => "Basic shapes that can be added to plots";

    public class RectangleQuickstart : RecipeBase
    {
        public override string Name => "Rectangle";

        public override string Description => "A rectangle can be added to the plot and styled as desired.";

        [Test]
        public override void Execute()
        {
            // add a rectangle by specifying points
            MyPlot.Add.Rectangle(0, 1, 0, 1);

            // add a rectangle using more expressive shapes
            Coordinates location = new Coordinates(2, 0);
            CoordinateSize size = new CoordinateSize(1, 1);
            CoordinateRect rect = new CoordinateRect(location, size);
            MyPlot.Add.Rectangle(rect);

            // style rectangles after they are added to the plot
            Rectangle rp = MyPlot.Add.Rectangle(4, 5, 0, 1);
            rp.FillStyle.Color = Colors.Magenta.WithAlpha(.2);
            rp.LineStyle.Color = Colors.Green;
            rp.LineStyle.Width = 3;
            rp.LineStyle.Pattern = LinePattern.Dashed;
        }
    }

    public class CircleQuickstart : RecipeBase
    {
        public override string Name => "Circle";

        public override string Description => "A circle can be placed on the plot and styled as desired.";

        [Test]
        public override void Execute()
        {
            ScottPlot.Plottables.Ellipse c1 = MyPlot.Add.Circle(1, 0, .5);
            ScottPlot.Plottables.Ellipse c2 = MyPlot.Add.Circle(2, 0, .5);
            ScottPlot.Plottables.Ellipse c3 = MyPlot.Add.Circle(3, 0, .5);

            c1.FillStyle.Color = Colors.Blue;
            c2.FillStyle.Color = Colors.Blue.Darken(.75);
            c3.FillStyle.Color = Colors.Blue.Lighten(.75);

            c1.LineWidth = 0;
            c2.LineWidth = 0;
            c3.LineWidth = 0;

            // force circles to remain circles
            SquareZoomOut squareRule = new SquareZoomOut(MyPlot.Axes.Bottom, MyPlot.Axes.Left);
            MyPlot.Axes.Rules.Add(squareRule);
        }
    }

    public class EllipseQuickstart : RecipeBase
    {
        public override string Name => "Ellipse";

        public override string Description => "An ellipse can be placed on the plot and styled as desired.";

        [Test]
        public override void Execute()
        {
            for (int i = 0; i < 10; i++)
            {
                ScottPlot.Plottables.Ellipse el = MyPlot.Add.Ellipse(0, 0, 1, 10, i * 10);
                double fraction = i / 10.0;
                el.LineColor = Colors.Blue.WithAlpha(fraction);
            }

            // force circles to remain circles
            SquareZoomOut squareRule = new SquareZoomOut(MyPlot.Axes.Bottom, MyPlot.Axes.Left);
            MyPlot.Axes.Rules.Add(squareRule);
        }
    }

    public class PolygonQuickstart : RecipeBase
    {
        public override string Name => "Polygon Plot Quickstart";

        public override string Description => "Polygon plots can be added from a series of vertices, and must be in clockwise order.";

        [Test]
        public override void Execute()
        {
            Coordinates[] points =
            [
                new Coordinates(0, 0.25), new Coordinates(0.3, 0.75), new Coordinates(1, 1), new Coordinates(0.7, 0.5), new Coordinates(1, 0)
            ];

            MyPlot.Add.Polygon(points);
        }
    }

    public class PolygonStyling : RecipeBase
    {
        public override string Name => "Polygon Plot Styling";

        public override string Description => "Polygon plots can be fully customized.";

        [Test]
        public override void Execute()
        {
            Coordinates[] points =
            [
                new Coordinates(0, 0.25), new Coordinates(0.3, 0.75), new Coordinates(1, 1), new Coordinates(0.7, 0.5), new Coordinates(1, 0)
            ];

            Polygon poly = MyPlot.Add.Polygon(points);
            poly.FillColor = Colors.Green;
            poly.FillHatchColor = Colors.Blue;
            poly.FillHatch = new Gradient { GradientType = GradientType.Linear, AlignmentStart = Alignment.UpperRight, AlignmentEnd = Alignment.LowerLeft, };

            poly.LineColor = Colors.Black;
            poly.LinePattern = LinePattern.Dashed;
            poly.LineWidth = 2;

            poly.MarkerShape = MarkerShape.OpenCircle;
            poly.MarkerSize = 8;
            poly.MarkerFillColor = Colors.Gold;
            poly.MarkerLineColor = Colors.Brown;
        }
    }
}
