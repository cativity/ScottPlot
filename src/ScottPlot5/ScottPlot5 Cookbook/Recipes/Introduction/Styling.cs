using JetBrains.Annotations;
using ScottPlot.Colormaps;
using ScottPlot.Palettes;
using ScottPlot.Plottables;

namespace ScottPlotCookbook.Recipes.Introduction;

[UsedImplicitly]
public class Styling : ICategory
{
    public string Chapter => "Introduction";

    public string CategoryName => "Styling Plots";

    public string CategoryDescription => "How to customize appearance of plots";

    public class StyleClass : RecipeBase
    {
        public override string Name => "Style Helper Functions";

        public override string Description
            => "Plots contain many objects which can be customized individually "
               + "by assigning to their public properties, but helper methods exist in the Plot's Style object "
               + "that make it easier to customize many items at once using a simpler API.";

        [Test]
        public override void Execute()
        {
            MyPlot.Add.Signal(Generate.Sin(51));
            MyPlot.Add.Signal(Generate.Cos(51));

            // visible items have public properties that can be customized
            MyPlot.Axes.Bottom.Label.Text = "Horizontal Axis";
            MyPlot.Axes.Left.Label.Text = "Vertical Axis";
            MyPlot.Axes.Title.Label.Text = "Plot Title";

            // some items must be styled directly
            MyPlot.Grid.MajorLineColor = Color.FromHex("#0e3d54");
            MyPlot.FigureBackground.Color = Color.FromHex("#07263b");
            MyPlot.DataBackground.Color = Color.FromHex("#0b3049");

            // the Style object contains helper methods to style many items at once
            MyPlot.Axes.Color(Color.FromHex("#a0acb5"));
        }
    }

    public class AxisCustom : RecipeBase
    {
        public override string Name => "Axis Customization";

        public override string Description => "Axis labels, tick marks, and frame can all be customized.";

        [Test]
        public override void Execute()
        {
            MyPlot.Add.Signal(Generate.Sin(51));
            MyPlot.Add.Signal(Generate.Cos(51));

            MyPlot.Axes.Title.Label.Text = "Plot Title";
            MyPlot.Axes.Title.Label.ForeColor = Colors.RebeccaPurple;
            MyPlot.Axes.Title.Label.FontSize = 32;
            MyPlot.Axes.Title.Label.FontName = Fonts.Serif;
            MyPlot.Axes.Title.Label.Rotation = -5;
            MyPlot.Axes.Title.Label.Bold = false;

            MyPlot.Axes.Left.Label.Text = "Vertical Axis";
            MyPlot.Axes.Left.Label.ForeColor = Colors.Magenta;
            MyPlot.Axes.Left.Label.Italic = true;

            MyPlot.Axes.Bottom.Label.Text = "Horizontal Axis";
            MyPlot.Axes.Bottom.Label.Bold = false;
            MyPlot.Axes.Bottom.Label.FontName = Fonts.Monospace;

            MyPlot.Axes.Bottom.MajorTickStyle.Length = 10;
            MyPlot.Axes.Bottom.MajorTickStyle.Width = 3;
            MyPlot.Axes.Bottom.MajorTickStyle.Color = Colors.Magenta;
            MyPlot.Axes.Bottom.MinorTickStyle.Length = 5;
            MyPlot.Axes.Bottom.MinorTickStyle.Width = 0.5f;
            MyPlot.Axes.Bottom.MinorTickStyle.Color = Colors.Green;
            MyPlot.Axes.Bottom.FrameLineStyle.Color = Colors.Blue;
            MyPlot.Axes.Bottom.FrameLineStyle.Width = 3;

            MyPlot.Axes.Right.FrameLineStyle.Width = 0;
        }
    }

    public class Palette : RecipeBase
    {
        public override string Name => "Palettes";

        public override string Description
            => "A palette is a set of colors, and the Plot's palette "
               + "defines the default colors to use when adding new plottables. ScottPlot comes with many "
               + "standard palettes, but users may also create their own.";

        [Test]
        public override void Execute()
        {
            // change the default palette used when adding new plottables
            MyPlot.Add.Palette = new Nord();

            for (int i = 0; i < 5; i++)
            {
                double[] data = Generate.Sin(100, phase: -i / 20.0f);
                Signal sig = MyPlot.Add.Signal(data);
                sig.LineWidth = 3;
            }
        }
    }

    public class ArrowShapeNames : RecipeBase
    {
        public override string Name => "Arrow Shapes";

        public override string Description => "Many standard arrow shapes are available";

        [Test]
        public override void Execute()
        {
            ArrowShape[] arrowShapes = [.. Enum.GetValues<ArrowShape>()];

            for (int i = 0; i < arrowShapes.Length; i++)
            {
                Coordinates arrowTip = new Coordinates(0, -i);
                Coordinates arrowBase = arrowTip.WithDelta(1, 0);

                Arrow arrow = MyPlot.Add.Arrow(arrowBase, arrowTip);
                arrow.ArrowShape = arrowShapes[i].GetShape();

                Text txt = MyPlot.Add.Text(arrowShapes[i].ToString(), arrowBase.WithDelta(.1, 0));
                txt.LabelFontColor = arrow.ArrowLineColor;
                txt.LabelAlignment = Alignment.MiddleLeft;
                txt.LabelFontSize = 18;
            }

            MyPlot.Axes.SetLimits(-1, 3, -arrowShapes.Length, 1);
            MyPlot.HideGrid();
        }
    }

    public class LineStyles : RecipeBase
    {
        public override string Name => "Line Styles";

        public override string Description => "Many plot types have a LineStyle which can be customized.";

        [Test]
        public override void Execute()
        {
            LinePattern[] linePatterns = [.. Enum.GetValues<LinePattern>()];

            for (int i = 0; i < linePatterns.Length; i++)
            {
                LinePattern pattern = linePatterns[i];

                LinePlot line = MyPlot.Add.Line(0, -i, 1, -i);
                line.LinePattern = pattern;
                line.LineWidth = 2;
                line.Color = Colors.Black;

                Text txt = MyPlot.Add.Text(pattern.ToString(), 1.1, -i);
                txt.LabelFontSize = 18;
                txt.LabelBold = true;
                txt.LabelFontColor = Colors.Black;
                txt.LabelAlignment = Alignment.MiddleLeft;
            }

            MyPlot.Axes.Margins(right: 1);
            MyPlot.HideGrid();
            MyPlot.Layout.Frameless();

            MyPlot.ShowLegend();
        }
    }

    public class Scaling : RecipeBase
    {
        public override string Name => "Scale Factor";

        public override string Description
            => "All components of an image can be scaled up or down in size "
               + "by adjusting the ScaleFactor property. This is very useful for creating images that look nice "
               + "on high DPI displays with display scaling enabled.";

        [Test]
        public override void Execute()
        {
            MyPlot.ScaleFactor = 2;
            MyPlot.Add.Signal(Generate.Sin());
            MyPlot.Add.Signal(Generate.Cos());
        }
    }

    public class Hairline : RecipeBase
    {
        public override string Name => "Hairline Mode";

        public override string Description
            => "Hairline mode allows axis frames, tick marks, and grid lines "
               + "to always be rendered a single pixel wide regardless of scale factor. Enable hairline mode to allow "
               + "interactive plots to feel smoother when a large scale factor is in use.";

        [Test]
        public override void Execute()
        {
            MyPlot.ScaleFactor = 2;
            MyPlot.Add.Signal(Generate.Sin());
            MyPlot.Add.Signal(Generate.Cos());

            MyPlot.Axes.Hairline(true);
        }
    }

    public class DarkMode : RecipeBase
    {
        public override string Name => "Dark Mode";

        public override string Description
            => "Plots can be created using dark mode by setting the colors of major plot components to ones consistent with a dark theme.";

        [Test]
        public override void Execute()
        {
            // set the color palette used when coloring new items added to the plot
            MyPlot.Add.Palette = new Penumbra();

            // add things to the plot
            for (int i = 0; i < 5; i++)
            {
                Signal sig = MyPlot.Add.Signal(Generate.Sin(51, phase: -.05 * i));
                sig.LineWidth = 3;
                sig.LegendText = $"Line {i + 1}";
            }

            MyPlot.XLabel("Horizontal Axis");
            MyPlot.YLabel("Vertical Axis");
            MyPlot.Title("ScottPlot 5 in Dark Mode");
            MyPlot.ShowLegend();

            // change figure colors
            MyPlot.FigureBackground.Color = Color.FromHex("#181818");
            MyPlot.DataBackground.Color = Color.FromHex("#1f1f1f");

            // change axis and grid colors
            MyPlot.Axes.Color(Color.FromHex("#d7d7d7"));
            MyPlot.Grid.MajorLineColor = Color.FromHex("#404040");

            // change legend colors
            MyPlot.Legend.BackgroundColor = Color.FromHex("#404040");
            MyPlot.Legend.FontColor = Color.FromHex("#d7d7d7");
            MyPlot.Legend.OutlineColor = Color.FromHex("#d7d7d7");
        }
    }

    public class ColormapColorSteps : RecipeBase
    {
        public override string Name => "Colormap Steps";

        public override string Description
            => "Colormaps can be used to generate a collection of discrete colors that can be applied to plottable objects.";

        [Test]
        public override void Execute()
        {
            IColormap colormap = new Turbo();

            for (int count = 1; count < 10; count++)
            {
                double[] xs = Generate.Consecutive(count);
                double[] ys = Generate.Repeating(count, count);
                Color[] colors = colormap.GetColors(count);

                for (int i = 0; i < count; i++)
                {
                    Ellipse circle = MyPlot.Add.Circle(xs[i], ys[i], 0.45);
                    circle.FillColor = colors[i];
                    circle.LineWidth = 0;
                }
            }

            MyPlot.YLabel("number of colors");
        }
    }
}
