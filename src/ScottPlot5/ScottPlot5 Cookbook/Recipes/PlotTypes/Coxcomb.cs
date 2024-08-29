using JetBrains.Annotations;

namespace ScottPlotCookbook.Recipes.PlotTypes;

[UsedImplicitly]
public class Coxcomb : ICategory
{
    public string Chapter => "Plot Types";

    public string CategoryName => "Coxcomb Plot";

    public string CategoryDescription => "A Coxcomb chart is a pie graph where the angle of slices is constant but the radii are not.";

    public class CoxcombQuickstart : RecipeBase
    {
        public override string Name => "Coxcomb Plot Quickstart";

        public override string Description => "A Coxcomb chart is a pie graph where the angle of slices is constant but the radii are not.";

        [Test]
        public override void Execute()
        {
            List<PieSlice> slices =
            [
                new PieSlice { Value = 5, Label = "Red", FillColor = Colors.Red },
                new PieSlice { Value = 2, Label = "Orange", FillColor = Colors.Orange },
                new PieSlice { Value = 8, Label = "Gold", FillColor = Colors.Gold },
                new PieSlice { Value = 4, Label = "Green", FillColor = Colors.Green.WithOpacity(0.5) },
                new PieSlice { Value = 8, Label = "Blue", FillColor = Colors.Blue.WithOpacity(0.5) }
            ];

            MyPlot.Add.Coxcomb(slices);

            MyPlot.Axes.Frameless();
            MyPlot.ShowLegend();
            MyPlot.HideGrid();
        }
    }
}
