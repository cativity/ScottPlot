using JetBrains.Annotations;
using ScottPlot.Plottables;

namespace ScottPlotCookbook.Recipes.PlotTypes;

[UsedImplicitly]
public class Box : ICategory
{
    public string Chapter => "Plot Types";

    public string CategoryName => "Box Plot";

    public string CategoryDescription => "Box plots show a distribution at a glance";

    public class BoxPlotQuickstart : RecipeBase
    {
        public override string Name => "Box Plot Quickstart";

        public override string Description => "Box plots can be created individually and added to the plot.";

        [Test]
        public override void Execute()
        {
            ScottPlot.Box box = new ScottPlot.Box
            {
                Position = 5,
                BoxMin = 81,
                BoxMax = 93,
                WhiskerMin = 76,
                WhiskerMax = 107,
                BoxMiddle = 84,
            };

            MyPlot.Add.Box(box);

            MyPlot.Axes.SetLimits(0, 10, 70, 110);
        }
    }

    public class BoxPlotGroups : RecipeBase
    {
        public override string Name => "Box Plot Groups";

        public override string Description
            => "Each collection of boxes added to the plot "
               + "gets styled the same and appears as a single item in the legend. "
               + "Add multiple bar series plots with defined X positions to give the "
               + "appearance of grouped data.";

        [Test]
        public override void Execute()
        {
            List<ScottPlot.Box> boxes1 =
            [
                Generate.RandomBox(1),
                Generate.RandomBox(2),
                Generate.RandomBox(3)
            ];

            List<ScottPlot.Box> boxes2 =
            [
                Generate.RandomBox(5),
                Generate.RandomBox(6),
                Generate.RandomBox(7)
            ];

            BoxPlot bp1 = MyPlot.Add.Boxes(boxes1);
            bp1.LegendText = "Group 1";

            BoxPlot bp2 = MyPlot.Add.Boxes(boxes2);
            bp2.LegendText = "Group 2";

            MyPlot.ShowLegend(Alignment.UpperRight);
        }
    }
}
