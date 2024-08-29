using JetBrains.Annotations;

namespace ScottPlotCookbook.Recipes.PlotTypes;

[UsedImplicitly]
public class Text : ICategory
{
    public string Chapter => "Plot Types";

    public string CategoryName => "Text";

    public string CategoryDescription => "Text labels can be placed on the plot in coordinate space";

    public class TextQuickstart : RecipeBase
    {
        public override string Name => "Text Quickstart";

        public override string Description => "Text can be placed anywhere in coordinate space.";

        [Test]
        public override void Execute()
        {
            MyPlot.Add.Signal(Generate.Sin());
            MyPlot.Add.Signal(Generate.Cos());
            MyPlot.Add.Text("Hello, World", 25, 0.5);
        }
    }

    public class Formatting : RecipeBase
    {
        public override string Name => "Text Formatting";

        public override string Description => "Text formatting can be extensively customized.";

        [Test]
        public override void Execute()
        {
            ScottPlot.Plottables.Text text = MyPlot.Add.Text("Hello, World", 42, 69);
            text.LabelFontSize = 26;
            text.LabelBold = true;
            text.LabelRotation = -45;
            text.LabelFontColor = Colors.Yellow;
            text.LabelBackgroundColor = Colors.Navy.WithAlpha(.5);
            text.LabelBorderColor = Colors.Magenta;
            text.LabelBorderWidth = 3;
            text.LabelPadding = 10;
            text.LabelAlignment = Alignment.MiddleCenter;
        }
    }

    public class LabelLineHeight : RecipeBase
    {
        public override string Name => "Line Height";

        public override string Description
            => "Multiline labels have a default line height "
               + "estimated from the typeface and font size, however line height may be manually "
               + "defined by the user.";

        [Test]
        public override void Execute()
        {
            ScottPlot.Plottables.Text label1 = MyPlot.Add.Text("line\nheight", 0, 0);
            label1.LabelLineSpacing = 0;
            label1.LabelFontColor = Colors.Red;
            label1.LabelBorderColor = Colors.Black;

            ScottPlot.Plottables.Text label2 = MyPlot.Add.Text("can\nbe", 1, 0);
            label2.LabelLineSpacing = 10;
            label2.LabelFontColor = Colors.Orange;
            label2.LabelBorderColor = Colors.Black;

            ScottPlot.Plottables.Text label3 = MyPlot.Add.Text("automatic\nor", 2, 0);
            label3.LabelLineSpacing = null;
            label3.LabelFontColor = Colors.Green;
            label3.LabelBorderColor = Colors.Black;

            ScottPlot.Plottables.Text label4 = MyPlot.Add.Text("set\nmanually", 3, 0);
            label4.LabelLineSpacing = 15;
            label4.LabelFontColor = Colors.Blue;
            label4.LabelBorderColor = Colors.Black;

            MyPlot.HideGrid();
            MyPlot.Axes.SetLimitsX(-.5, 4);
        }
    }

    public class TextOffset : RecipeBase
    {
        public override string Name => "Text Offset";

        public override string Description => "The offset properties can be used to fine-tune text position in pixel units";

        [Test]
        public override void Execute()
        {
            for (int i = 0; i < 25; i += 5)
            {
                // place a marker at the point
                ScottPlot.Plottables.Marker marker = MyPlot.Add.Marker(i, 1);

                // place a styled text label at the point
                ScottPlot.Plottables.Text txt = MyPlot.Add.Text($"{i}", i, 1);
                txt.LabelFontSize = 16;
                txt.LabelBorderColor = Colors.Black;
                txt.LabelBorderWidth = 1;
                txt.LabelPadding = 2;
                txt.LabelBold = true;
                txt.LabelBackgroundColor = marker.Color.WithAlpha(.5);

                // offset the text label by the given number of pixels
                txt.OffsetX = i;
                txt.OffsetY = i;
            }

            MyPlot.Axes.SetLimitsX(-5, 30);
        }
    }
}
