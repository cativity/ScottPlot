using JetBrains.Annotations;

namespace ScottPlotCookbook.Recipes.PlotTypes;

[UsedImplicitly]
public class Pie : ICategory
{
    public string Chapter => "Plot Types";

    public string CategoryName => "Pie";

    public string CategoryDescription => "Pie charts illustrate numerical proportions as slices of a circle.";

    public class PieQuickstart : RecipeBase
    {
        public override string Name => "Pie Chart from Values";

        public override string Description => "A pie chart can be created from a few values.";

        [Test]
        public override void Execute()
        {
            double[] values = [5, 2, 8, 4, 8];
            ScottPlot.Plottables.Pie pie = MyPlot.Add.Pie(values);
            pie.ExplodeFraction = .1;
        }
    }

    public class PieSlices : RecipeBase
    {
        public override string Name => "Pie Chart from Slices";

        public override string Description => "A pie chart can be created from a collection of slices.";

        [Test]
        public override void Execute()
        {
            List<PieSlice> slices =
            [
                new PieSlice { Value = 5, FillColor = Colors.Red, Label = "Red" },
                new PieSlice { Value = 2, FillColor = Colors.Orange, Label = "Orange" },
                new PieSlice { Value = 8, FillColor = Colors.Gold, Label = "Yellow" },
                new PieSlice { Value = 4, FillColor = Colors.Green, Label = "Green" },
                new PieSlice { Value = 8, FillColor = Colors.Blue, Label = "Blue" }
            ];

            ScottPlot.Plottables.Pie pie = MyPlot.Add.Pie(slices);
            pie.ExplodeFraction = .1;

            MyPlot.ShowLegend();
        }
    }

    public class PieDonut : RecipeBase
    {
        public override string Name => "Donut from Slices";

        public override string Description => "A donut chart is a pie chart with an open center. Donut charts can be created from a collection of slices.";

        [Test]
        public override void Execute()
        {
            List<PieSlice> slices =
            [
                new PieSlice { Value = 5, FillColor = Colors.Red, Label = "Red" },
                new PieSlice { Value = 2, FillColor = Colors.Orange, Label = "Orange" },
                new PieSlice { Value = 8, FillColor = Colors.Gold, Label = "Yellow" },
                new PieSlice { Value = 4, FillColor = Colors.Green, Label = "Green" },
                new PieSlice { Value = 8, FillColor = Colors.Blue, Label = "Blue" }
            ];

            ScottPlot.Plottables.Pie pie = MyPlot.Add.Pie(slices);
            pie.DonutFraction = .5;

            MyPlot.ShowLegend();
        }
    }

    public class PieSliceLabels : RecipeBase
    {
        public override string Name => "Pie Slice Labels";

        public override string Description
            => "Slice labels can be displayed centered with the slice at a customizable distance from the center of the pie.";

        [Test]
        public override void Execute()
        {
            PieSlice slice1 = new PieSlice { Value = 5, FillColor = Colors.Red, Label = "Red" };
            PieSlice slice2 = new PieSlice { Value = 2, FillColor = Colors.Orange, Label = "Orange" };
            PieSlice slice3 = new PieSlice { Value = 8, FillColor = Colors.Gold, Label = "Yellow" };
            PieSlice slice4 = new PieSlice { Value = 4, FillColor = Colors.Green, Label = "Green" };
            PieSlice slice5 = new PieSlice { Value = 8, FillColor = Colors.Blue, Label = "Blue" };
            List<PieSlice> slices = [slice1, slice2, slice3, slice4, slice5];

            // setup the pie to display slice labels
            ScottPlot.Plottables.Pie pie = MyPlot.Add.Pie(slices);
            pie.ExplodeFraction = .1;
            pie.ShowSliceLabels = true;
            pie.SliceLabelDistance = 1.3;

            // styling can be customized for individual slices
            slice5.LabelStyle.FontSize = 22;
            slice5.LabelStyle.ForeColor = Colors.Magenta;
            slice5.LabelStyle.Bold = true;
        }
    }
}
