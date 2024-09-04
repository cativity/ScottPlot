using JetBrains.Annotations;
using ScottPlot.Plottables;

namespace ScottPlotCookbook.Recipes.PlotTypes;

[UsedImplicitly]
public class AxisLines : ICategory
{
    public string Chapter => "Plot Types";

    public string CategoryName => "Axis Lines";

    public string CategoryDescription => "Axis lines indicate a position on an axis.";

    public class AxisLineQuickstart : RecipeBase
    {
        public override string Name => "Axis Lines";

        public override string Description => "Axis lines are vertical or horizontal lines that span an entire axis.";

        [Test]
        public override void Execute()
        {
            MyPlot.Add.Signal(Generate.Sin());
            MyPlot.Add.Signal(Generate.Cos());

            MyPlot.Add.VerticalLine(24);
            MyPlot.Add.HorizontalLine(0.73);
        }
    }

    public class AxisLineLabel : RecipeBase
    {
        public override string Name => "Axis Line Label";

        public override string Description => "Axis lines have labels that can be used to display arbitrary text on the axes they are attached to.";

        [Test]
        public override void Execute()
        {
            MyPlot.Add.Signal(Generate.Sin());
            MyPlot.Add.Signal(Generate.Cos());

            // by default labels are drawn on the same side as the axis label

            VerticalLine axLine1 = MyPlot.Add.VerticalLine(24);
            axLine1.Text = "Line 1";

            HorizontalLine axLine2 = MyPlot.Add.HorizontalLine(0.75);
            axLine2.Text = "Line 2";

            // labels may be drawn on the side opposite of the axis label

            VerticalLine axLine3 = MyPlot.Add.VerticalLine(37);
            axLine3.Text = "Line 3";
            axLine3.LabelOppositeAxis = true;

            HorizontalLine axLine4 = MyPlot.Add.HorizontalLine(-.75);
            axLine4.Text = "Line 4";
            axLine4.LabelOppositeAxis = true;

            // extra padding on the right and top ensures labels have room
            MyPlot.Axes.Right.MinimumSize = 30;
            MyPlot.Axes.Top.MinimumSize = 30;
        }
    }

    public class AxisLineLabelPositioning : RecipeBase
    {
        public override string Name => "Axis Line Label Positioning";

        public override string Description => "Axis line labels can have custom positioning, including rotation and alignment.";

        [Test]
        public override void Execute()
        {
            MyPlot.Add.Signal(Generate.Sin());
            MyPlot.Add.Signal(Generate.Cos());

            VerticalLine axLine1 = MyPlot.Add.VerticalLine(42);
            axLine1.Text = "Line 1";
            axLine1.LabelRotation = -90;
            axLine1.LabelAlignment = Alignment.MiddleRight;

            HorizontalLine axLine2 = MyPlot.Add.HorizontalLine(0.75);
            axLine2.Text = "Line 2";
            axLine2.LabelRotation = 0;
            axLine2.LabelAlignment = Alignment.MiddleRight;

            VerticalLine axLine3 = MyPlot.Add.VerticalLine(20);
            axLine3.Text = "Line 3";
            axLine3.LabelRotation = -45;
            axLine3.LabelAlignment = Alignment.UpperRight;

            // extra padding on the bottom and left for the rotated labels
            MyPlot.Axes.Bottom.MinimumSize = 60;
            MyPlot.Axes.Left.MinimumSize = 60;
        }
    }

    public class AxisLineStyle : RecipeBase
    {
        public override string Name => "Axis Line Style";

        public override string Description => "Axis lines have extensive customization options.";

        [Test]
        public override void Execute()
        {
            MyPlot.Add.Signal(Generate.Sin());
            MyPlot.Add.Signal(Generate.Cos());

            VerticalLine vl1 = MyPlot.Add.VerticalLine(24);
            vl1.LineWidth = 3;
            vl1.Color = Colors.Magenta;

            HorizontalLine hl1 = MyPlot.Add.HorizontalLine(0.75);
            hl1.LineWidth = 2;
            hl1.Color = Colors.Green;
            hl1.LinePattern = LinePattern.Dashed;

            HorizontalLine hl2 = MyPlot.Add.HorizontalLine(-.23);
            hl2.LineColor = Colors.Navy;
            hl2.LineWidth = 5;
            hl2.Text = "Hello";
            hl2.LabelFontSize = 24;
            hl2.LabelBackgroundColor = Colors.Blue;
            hl2.LabelFontColor = Colors.Yellow;
            hl2.LinePattern = LinePattern.DenselyDashed;
        }
    }

    public class AxisLineInLegend : RecipeBase
    {
        public override string Name => "Axis Line In Legend";

        public override string Description
            => "Axis lines will be added to the legend if their Text property is set unless their ExcludeFromLegend property is true.";

        [Test]
        public override void Execute()
        {
            MyPlot.Add.Signal(Generate.Sin());
            MyPlot.Add.Signal(Generate.Cos());

            VerticalLine axLine1 = MyPlot.Add.VerticalLine(24);
            axLine1.Text = "Line 1";

            _ = MyPlot.Add.HorizontalLine(0.75);

            VerticalLine axLine3 = MyPlot.Add.VerticalLine(37);
            axLine3.Text = "Line 3";
            axLine3.ExcludeFromLegend = true;

            HorizontalLine axLine4 = MyPlot.Add.HorizontalLine(0.25);
            axLine4.Text = "Line 4";

            HorizontalLine axLine5 = MyPlot.Add.HorizontalLine(-.75);
            axLine5.Text = "Line 5";
            axLine5.ExcludeFromLegend = true;

            MyPlot.ShowLegend();
        }
    }

    public class AxisLineIgnoreLimits : RecipeBase
    {
        public override string Name => "Ignore When Autoscaling";

        public override string Description
            => "Calling Plot.Axes.AutoScale() or middle-clicking the plot will set the axis limits "
               + "to fit the data. By default the position of axis lines and spans are included in automatic axis limit calculations, "
               + "but a flag can be set to ignore certain plottables when automatically scaling the plot.";

        [Test]
        public override void Execute()
        {
            MyPlot.Add.Signal(Generate.Sin(51));
            MyPlot.Add.Signal(Generate.Cos(51));

            HorizontalLine hline = MyPlot.Add.HorizontalLine(0.23);
            hline.IsDraggable = true;
            hline.EnableAutoscale = false;

            HorizontalSpan hSpan = MyPlot.Add.HorizontalSpan(-10, 20);
            hSpan.IsDraggable = true;
            hSpan.EnableAutoscale = false;
        }
    }
}
