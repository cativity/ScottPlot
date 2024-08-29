using JetBrains.Annotations;
using ScottPlot.AxisPanels;
using ScottPlot.Plottables;

namespace ScottPlotCookbook.Recipes.Axis;

[UsedImplicitly]
public class MultiAxis : ICategory
{
    public string Chapter => "Axis";

    public string CategoryName => "Multiple Axes";

    public string CategoryDescription => "Tick mark customization and creation of multi-Axis plots";

    public class RightAxis : RecipeBase
    {
        public override string Name => "Right Axis";

        public override string Description
            => "New plots have one axis on every side. "
               + "Axes on the right and top are invisible by default. "
               + "To use the right axis, make it visible, then tell a plottable to use it. ";

        [Test]
        public override void Execute()
        {
            // plot data with very different scales
            Signal sig1 = MyPlot.Add.Signal(Generate.Sin(mult: 0.01));
            Signal sig2 = MyPlot.Add.Signal(Generate.Cos(mult: 100));

            // tell each signal plot to use a different axis
            sig1.Axes.YAxis = MyPlot.Axes.Left;
            sig2.Axes.YAxis = MyPlot.Axes.Right;

            // add additional styling options to each axis
            MyPlot.Axes.Left.Label.Text = "Left Axis";
            MyPlot.Axes.Right.Label.Text = "Right Axis";
            MyPlot.Axes.Left.Label.ForeColor = sig1.Color;
            MyPlot.Axes.Right.Label.ForeColor = sig2.Color;
        }
    }

    public class MultiAxisQuickstart : RecipeBase
    {
        public override string Name => "Multi-Axis";

        public override string Description
            => "Additional axes may be added to plots. "
               + "Plottables are displayed using the coordinate system of the primary axes by default, "
               + "but any plottable can be displayed using any X and Y axis.";

        [Test]
        public override void Execute()
        {
            // plottables use the standard X and Y axes by default
            Signal sig1 = MyPlot.Add.Signal(Generate.Sin(51, 0.01));
            sig1.Axes.XAxis = MyPlot.Axes.Bottom; // standard X axis
            sig1.Axes.YAxis = MyPlot.Axes.Left; // standard Y axis
            MyPlot.Axes.Left.Label.Text = "Primary Y Axis";

            // create a second axis and add it to the plot
            LeftAxis yAxis2 = MyPlot.Axes.AddLeftAxis();

            // add a new plottable and tell it to use the custom Y axis
            Signal sig2 = MyPlot.Add.Signal(Generate.Cos(51, 100));
            sig2.Axes.XAxis = MyPlot.Axes.Bottom; // standard X axis
            sig2.Axes.YAxis = yAxis2; // custom Y axis
            yAxis2.LabelText = "Secondary Y Axis";
        }
    }
}
