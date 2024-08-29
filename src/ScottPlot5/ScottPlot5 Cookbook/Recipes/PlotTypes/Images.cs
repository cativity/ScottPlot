using JetBrains.Annotations;

namespace ScottPlotCookbook.Recipes.PlotTypes;

[UsedImplicitly]
public class Images : ICategory
{
    public string Chapter => "Plot Types";

    public string CategoryName => "Images";

    public string CategoryDescription => "Images can be placed on plots in a variety of ways";

    public class ImageRectQuickstart : RecipeBase
    {
        public override string Name => "Image Rectangle";

        public override string Description => "An image can be drawn inside a rectangle defined in coordinate units.";

        [Test]
        public override void Execute()
        {
            // Images may be loaded from files or created dynamically
            Image img = SampleImages.MonaLisa();

            CoordinateRect rect = new CoordinateRect(0, img.Width, 0, img.Height);

            MyPlot.Add.ImageRect(img, rect);
        }
    }
}
