using JetBrains.Annotations;

namespace ScottPlotCookbook.Recipes.Miscellaneous;

[UsedImplicitly]
public class Layout : ICategory
{
    public string Chapter => "Layout";

    public string CategoryName => "Layout Options";

    public string CategoryDescription => "How to customize the layout of a plot";

    public class Frameless : RecipeBase
    {
        public override string Name => "Frameless Plot";

        public override string Description => "How to create a plot containing only the data area and no axes.";

        [Test]
        public override void Execute()
        {
            MyPlot.Add.Signal(Generate.Sin(51));
            MyPlot.Add.Signal(Generate.Cos(51));

            // make the data area cover the full figure
            MyPlot.Layout.Frameless();

            // set the data area background so we can observe its size
            MyPlot.DataBackground.Color = Colors.WhiteSmoke;
        }
    }

    public class FixedPadding : RecipeBase
    {
        public override string Name => "Fixed Padding";

        public override string Description => "The plot can be arranged to achieve a fixed amount of padding on each side of the data area";

        [Test]
        public override void Execute()
        {
            // add sample data to the plot
            MyPlot.Add.Signal(Generate.Sin(51));
            MyPlot.Add.Signal(Generate.Cos(51));

            // use a fixed amount of of pixel padding on each side
            PixelPadding padding = new PixelPadding(100, 50, 100, 50);
            MyPlot.Layout.Fixed(padding);

            // darken the figure background so we can observe its dimensions
            MyPlot.FigureBackground.Color = Colors.LightBlue;
            MyPlot.DataBackground.Color = Colors.White;
        }
    }

    public class FixedRectangle : RecipeBase
    {
        public override string Name => "Fixed Rectangle";

        public override string Description => "The plot can be arranged so the data is drawn inside a fixed rectangle defined in pixel units";

        [Test]
        public override void Execute()
        {
            // add sample data to the plot
            MyPlot.Add.Signal(Generate.Sin(51));
            MyPlot.Add.Signal(Generate.Cos(51));

            // set the data area to render inside a fixed rectangle
            PixelSize size = new PixelSize(300, 200);
            Pixel offset = new Pixel(50, 50);
            PixelRect rect = new PixelRect(size, offset);
            MyPlot.Layout.Fixed(rect);

            // darken the figure background so we can observe its dimensions
            MyPlot.FigureBackground.Color = Colors.LightBlue;
            MyPlot.DataBackground.Color = Colors.White;
        }
    }
}
