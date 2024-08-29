using JetBrains.Annotations;
using ScottPlot.Hatches;

namespace ScottPlotCookbook.Recipes.PlotTypes;

[UsedImplicitly]
public class FillY : ICategory
{
    public string Chapter => "Plot Types";

    public string CategoryName => "FillY plot";

    public string CategoryDescription => "FillY plots display the vertical range between two Y values at defined X positions";

    public class FillYFromArrays : RecipeBase
    {
        public override string Name => "FillY From Array Data";

        public override string Description => "FillY plots can be created from X, Y1, and Y2 arrays.";

        [Test]
        public override void Execute()
        {
            RandomDataGenerator dataGen = new RandomDataGenerator(0);

            const int count = 20;
            double[] xs = Generate.Consecutive(count);
            double[] ys1 = dataGen.RandomWalk(count, offset: -5);
            double[] ys2 = dataGen.RandomWalk(count, offset: 5);

            ScottPlot.Plottables.FillY fill = MyPlot.Add.FillY(xs, ys1, ys2);
            fill.FillColor = Colors.Blue.WithAlpha(100);
            fill.LineColor = Colors.Blue;
            fill.MarkerColor = Colors.Blue;
            fill.LineWidth = 2;
        }
    }

    public class FillYFromScatters : RecipeBase
    {
        public override string Name => "FillY From Scatter Plots";

        public override string Description => "FillY plots can be created from two scatter plots that share the same X values.";

        [Test]
        public override void Execute()
        {
            RandomDataGenerator dataGen = new RandomDataGenerator(0);

            const int count = 20;
            double[] xs = Generate.Consecutive(count);
            double[] ys1 = dataGen.RandomWalk(count, offset: -5);
            double[] ys2 = dataGen.RandomWalk(count, offset: 5);

            ScottPlot.Plottables.Scatter scatter1 = MyPlot.Add.Scatter(xs, ys1);
            ScottPlot.Plottables.Scatter scatter2 = MyPlot.Add.Scatter(xs, ys2);

            ScottPlot.Plottables.FillY fill = MyPlot.Add.FillY(scatter1, scatter2);
            fill.FillColor = Colors.Blue.WithAlpha(.1);
            fill.LineWidth = 0;

            // push the fill behind the scatter plots
            MyPlot.MoveToBack(fill);
        }
    }

    public class Function : RecipeBase
    {
        public override string Name => "FillY with Custom Type";

        public override string Description => "FillY plots can be created from data of any type if a conversion function is supplied.";

        [Test]
        public override void Execute()
        {
            // create source data in a nonstandard data type
            List<(int, int, int)> data = [];
            Random rand = new Random(0);

            for (int i = 0; i < 10; i++)
            {
                int x = i;
                int y1 = rand.Next(0, 10);
                int y2 = rand.Next(20, 30);
                data.Add((x, y1, y2));
            }

            // create a filled plot from source data using the custom converter
            ScottPlot.Plottables.FillY fill = MyPlot.Add.FillY(data, MyConverter);
            fill.FillColor = Colors.Blue.WithAlpha(.2);
            fill.LineColor = Colors.Blue;

            return;

            // create a custom converter for the source data type
            static (double, double, double) MyConverter((int, int, int) s) => (s.Item1, s.Item2, s.Item3);
        }
    }

    public class Styling : RecipeBase
    {
        public override string Name => "FillY Plot Styling";

        public override string Description => "FillY plots can be customized using public properties.";

        [Test]
        public override void Execute()
        {
            RandomDataGenerator dataGen = new RandomDataGenerator(0);

            const int count = 20;
            double[] xs = Generate.Consecutive(count);
            double[] ys1 = dataGen.RandomWalk(count, offset: -5);
            double[] ys2 = dataGen.RandomWalk(count, offset: 5);

            ScottPlot.Plottables.FillY fill = MyPlot.Add.FillY(xs, ys1, ys2);
            fill.MarkerShape = MarkerShape.FilledDiamond;
            fill.MarkerSize = 15;
            fill.MarkerColor = Colors.Blue;
            fill.LineColor = Colors.Blue;
            fill.LinePattern = LinePattern.Dotted;
            fill.LineWidth = 2;
            fill.FillColor = Colors.Blue.WithAlpha(.2);
            fill.FillHatch = new Striped(StripeDirection.DiagonalUp);
            fill.FillHatchColor = Colors.Blue.WithAlpha(.4);
            fill.LegendText = "Filled Area";

            MyPlot.ShowLegend();
        }
    }
}
