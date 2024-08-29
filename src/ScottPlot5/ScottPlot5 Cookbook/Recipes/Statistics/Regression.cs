using JetBrains.Annotations;
using ScottPlot.Statistics;

namespace ScottPlotCookbook.Recipes.PlotTypes;

[UsedImplicitly]
public class Regression : ICategory
{
    public string Chapter => "Statistics";

    public string CategoryName => "Regression";

    public string CategoryDescription => "Statistical operations to fit lines to data";

    public class Linear : RecipeBase
    {
        public override string Name => "LinearRegression";

        public override string Description => "Fit a line to a collection of X/Y data points.";

        [Test]
        public override void Execute()
        {
            double[] xs = [1, 2, 3, 4, 5, 6, 7];
            double[] ys = [2, 2, 3, 3, 3.8, 4.2, 4];

            // plot original data as a scatter plot
            ScottPlot.Plottables.Scatter sp = MyPlot.Add.Scatter(xs, ys);
            sp.LineWidth = 0;
            sp.MarkerSize = 10;

            // calculate the regression line
            LinearRegression reg = new LinearRegression(xs, ys);

            // plot the regression line
            Coordinates pt1 = new Coordinates(xs[0], reg.GetValue(xs[0]));
            Coordinates pt2 = new Coordinates(xs[^1], reg.GetValue(xs[^1]));
            ScottPlot.Plottables.LinePlot line = MyPlot.Add.Line(pt1, pt2);
            line.MarkerSize = 0;
            line.LineWidth = 2;
            line.LinePattern = LinePattern.Dashed;

            // note the formula at the top of the plot
            MyPlot.Title(reg.FormulaWithRSquared);
        }
    }
}
