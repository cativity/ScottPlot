using JetBrains.Annotations;
using ScottPlot.Plottables;

namespace ScottPlotCookbook.Recipes.PlotTypes;

[UsedImplicitly]
public class AxisSpans : ICategory
{
    public string Chapter => "Plot Types";

    public string CategoryName => "Axis Spans";

    public string CategoryDescription => "Axis spans indicate a range of an axis.";

    public class AxisSpanQuickstart : RecipeBase
    {
        public override string Name => "Axis Span Quickstart";

        public override string Description
            => "Axis spans label a range of an axis. "
               + "Vertical spans shade the full width of a vertical range, "
               + "and horizontal spans shade the full height of a horizontal range.";

        [Test]
        public override void Execute()
        {
            MyPlot.Add.Signal(Generate.Sin());
            MyPlot.Add.Signal(Generate.Cos());

            HorizontalSpan hSpan = MyPlot.Add.HorizontalSpan(10, 20);
            VerticalSpan vSpan = MyPlot.Add.VerticalSpan(0.25, 0.75);

            hSpan.LegendText = "Horizontal Span";
            vSpan.LegendText = "Vertical Span";
            MyPlot.ShowLegend();
        }
    }

    public class AxisSpanStyling : RecipeBase
    {
        public override string Name => "Axis Span Styling";

        public override string Description => "Axis spans can be extensively customized.";

        [Test]
        public override void Execute()
        {
            MyPlot.Add.Signal(Generate.Sin());
            MyPlot.Add.Signal(Generate.Cos());

            HorizontalSpan hs = MyPlot.Add.HorizontalSpan(10, 20);
            hs.LegendText = "Hello";
            hs.LineStyle.Width = 2;
            hs.LineStyle.Color = Colors.Magenta;
            hs.LineStyle.Pattern = LinePattern.Dashed;
            hs.FillStyle.Color = Colors.Magenta.WithAlpha(.2);
        }
    }
}
