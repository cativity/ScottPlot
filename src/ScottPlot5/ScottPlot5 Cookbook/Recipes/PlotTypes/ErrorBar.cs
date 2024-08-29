using JetBrains.Annotations;

namespace ScottPlotCookbook.Recipes.PlotTypes;

[UsedImplicitly]
public class ErrorBar : ICategory
{
    public string Chapter => "Plot Types";

    public string CategoryName => "Error Bars";

    public string CategoryDescription => "Error Bars communicate the range of possible values for a measurement";

    public class ErrorBarQuickstart : RecipeBase
    {
        public override string Name => "Error Bar Quickstart";

        public override string Description => "Error Bars go well with scatter plots.";

        [Test]
        public override void Execute()
        {
            const int points = 30;

            double[] xs = Generate.Consecutive(points);
            double[] ys = Generate.RandomWalk(points);
            double[] err = Generate.RandomSample(points, 0.1, 1);

            ScottPlot.Plottables.Scatter scatter = MyPlot.Add.Scatter(xs, ys);
            ScottPlot.Plottables.ErrorBar errorbars = MyPlot.Add.ErrorBar(xs, ys, err);
            errorbars.Color = scatter.Color;
        }
    }

    public class CustomErrors : RecipeBase
    {
        public override string Name => "ErrorBar Values";

        public override string Description => "Error size can be set for all dimensions.";

        [Test]
        public override void Execute()
        {
            const int points = 10;

            double[] xs = Generate.Consecutive(points);
            double[] ys = Generate.RandomWalk(points);
            ScottPlot.Plottables.Scatter scatter = MyPlot.Add.Scatter(xs, ys);
            scatter.LineStyle.Width = 0;

            ScottPlot.Plottables.ErrorBar eb =
                new ScottPlot.Plottables.ErrorBar(xs,
                                                  ys,
                                                  Generate.RandomSample(points, .5),
                                                  Generate.RandomSample(points, .5),
                                                  Generate.RandomSample(points),
                                                  Generate.RandomSample(points)) { Color = scatter.Color };

            MyPlot.Add.Plottable(eb);
        }
    }
}
