using JetBrains.Annotations;

namespace ScottPlotCookbook.Recipes.PlotTypes;

[UsedImplicitly]
public class Callout : ICategory
{
    public string Chapter => "Plot Types";

    public string CategoryName => "Callout";

    public string CategoryDescription => "Callouts display a label and are connected with an arrow that marks a point on the plot.";

    public class CalloutQuickstart : RecipeBase
    {
        public override string Name => "Callout Quickstart";

        public override string Description => "Callouts display a label and are connected with an arrow that marks a point on the plot.";

        [Test]
        public override void Execute()
        {
            double[] xs = Generate.Consecutive(15);
            double[] ys = Generate.Sin(15);
            MyPlot.Add.Scatter(xs, ys);

            MyPlot.Add.Callout("Hello", new Coordinates(7.5, .8), new Coordinates(xs[6], ys[6]));

            MyPlot.Add.Callout("World", new Coordinates(10, 0), new Coordinates(xs[13], ys[13]));
        }
    }
}
