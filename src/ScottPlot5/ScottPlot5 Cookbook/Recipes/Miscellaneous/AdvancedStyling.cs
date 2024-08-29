using JetBrains.Annotations;
using ScottPlot.Plottables;

namespace ScottPlotCookbook.Recipes.Miscellaneous;

[UsedImplicitly]
public class AdvancedStyling : ICategory
{
    public string Chapter => "Miscellaneous";

    public string CategoryName => "Advanced Styling";

    public string CategoryDescription => "Features for users seeking extensive customization options.";

    public class AdvancedStylingDataBackground : RecipeBase
    {
        public override string Name => "Data Area Background Image";

        public override string Description => "An image can be used for the background of the data area.";

        [Test]
        public override void Execute()
        {
            // plot sample data
            Signal sig1 = MyPlot.Add.Signal(Generate.Sin());
            Signal sig2 = MyPlot.Add.Signal(Generate.Cos());
            sig1.LineWidth = 3;
            sig2.LineWidth = 3;

            // One could load an image from a file...
            // Image bgImage = new("background.png");

            // But in this example we will use a sample image:
            MyPlot.DataBackground.Image = SampleImages.ScottPlotLogo();
        }
    }

    public class AdvancedStylingFigureBackground : RecipeBase
    {
        public override string Name => "Figure Background Image";

        public override string Description => "An image can be used for the background of a figure.";

        [Test]
        public override void Execute()
        {
            // plot sample data
            Signal sig1 = MyPlot.Add.Signal(Generate.Sin());
            Signal sig2 = MyPlot.Add.Signal(Generate.Cos());

            // One could load an image from a file...
            // Image bgImage = new("background.png");

            // But in this example we will use a sample image:
            MyPlot.FigureBackground.Image = SampleImages.MonaLisa();

            // Color the axes and data so they stand out against the dark background
            MyPlot.Axes.Color(Colors.White);
            sig1.Color = sig1.Color.Lighten(.2);
            sig2.Color = sig2.Color.Lighten(.2);
            sig1.LineWidth = 3;
            sig2.LineWidth = 3;

            // Shade the data area to make it stand out
            MyPlot.DataBackground.Color = Colors.Black.WithAlpha(.5);
        }
    }

    public class ColorInterpolation : RecipeBase
    {
        public override string Name => "Color Interpolation";

        public override string Description => "Colors can be mixed to create a range of colors. This strategy uses linear RGB interpolation.";

        [Test]
        public override void Execute()
        {
            for (int i = 0; i <= 10; i++)
            {
                double fraction = (double)i / 10;
                double x = i;
                double y = Math.Sin(Math.PI * 2 * fraction);
                Ellipse circle = MyPlot.Add.Circle(x, y, 2);
                circle.FillColor = Colors.Blue.MixedWith(Colors.Green, fraction);
                circle.LineColor = Colors.Black.WithAlpha(.5);
            }
        }
    }
}
