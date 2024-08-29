using JetBrains.Annotations;
using ScottPlot.Plottables;

namespace ScottPlotCookbook.Recipes.PlotTypes;

[UsedImplicitly]
public class Function : ICategory
{
    public string Chapter => "Plot Types";

    public string CategoryName => "Function";

    public string CategoryDescription
        => "Function plots are a type of line plot where Y positions "
           + "are defined by a function that depends on X rather than a collection of discrete data points.";

    public class FunctionQuickstart : RecipeBase
    {
        public override string Name => "Function Quickstart";

        public override string Description => "Create a function plot from a formula.";

        [Test]
        public override void Execute()
        {
            // Add functions to the plot
            MyPlot.Add.Function(Func1);
            MyPlot.Add.Function(Func2);
            MyPlot.Add.Function(Func3);

            // Manually set axis limits because functions do not have discrete data points
            MyPlot.Axes.SetLimits(-10, 10, -1.5, 1.5);

            return;

            // Functions are defined as delegates with an input and output
            static double Func1(double x) => Math.Sin(x) * Math.Sin(x / 2);
            static double Func2(double x) => Math.Sin(x) * Math.Sin(x / 3);
            static double Func3(double x) => Math.Cos(x) * Math.Sin(x / 5);
        }
    }

    public class FunctionLimitX : RecipeBase
    {
        public override string Name => "Function Limit X";

        public override string Description => "A function can be limited to a range of X values.";

        [Test]
        public override void Execute()
        {
            FunctionPlot f = MyPlot.Add.Function(Func);
            f.MinX = -3;
            f.MaxX = 3;

            MyPlot.Axes.SetLimits(-5, 5, -.2, 1.0);

            return;

            static double Func(double x) => Math.Sin(x) * Math.Sin(x / 2);
        }
    }
}
