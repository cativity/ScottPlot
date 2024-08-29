using JetBrains.Annotations;
using ScottPlot.Plottables;

namespace ScottPlotCookbook.Recipes.Introduction;

[UsedImplicitly]
public class Legend : ICategory
{
    public string Chapter => "Introduction";

    public string CategoryName => "Configuring Legends";

    public string CategoryDescription => "A legend is a key typically displayed in the corner of a plot";

    public class LegendQuickstart : RecipeBase
    {
        public override string Name => "Legend Quickstart";

        public override string Description => "Many plottables have a Label property that can be set so they appear in the legend.";

        [Test]
        public override void Execute()
        {
            Signal sig1 = MyPlot.Add.Signal(Generate.Sin(51));
            sig1.LegendText = "Sin";

            Signal sig2 = MyPlot.Add.Signal(Generate.Cos(51));
            sig2.LegendText = "Cos";

            MyPlot.ShowLegend();
        }
    }

    public class ManualLegend : RecipeBase
    {
        public override string Name => "Manual Legend Items";

        public override string Description => "Legends may be constructed manually.";

        [Test]
        public override void Execute()
        {
            MyPlot.Add.Signal(Generate.Sin(51));
            MyPlot.Add.Signal(Generate.Cos(51));
            MyPlot.Legend.IsVisible = true;

            LegendItem item1 = new LegendItem
            {
                LineColor = Colors.Magenta,
                MarkerFillColor = Colors.Magenta,
                MarkerLineColor = Colors.Magenta,
                LineWidth = 2,
                LabelText = "Alpha"
            };

            LegendItem item2 = new LegendItem
            {
                LineColor = Colors.Green,
                MarkerFillColor = Colors.Green,
                MarkerLineColor = Colors.Green,
                LineWidth = 4,
                LabelText = "Beta"
            };

            LegendItem[] items = [item1, item2];
            MyPlot.ShowLegend(items);
        }
    }

    public class LegendStyle : RecipeBase
    {
        public override string Name => "Legend Customization";

        public override string Description => "Access the Legend object directly for advanced customization options.";

        [Test]
        public override void Execute()
        {
            Signal sig1 = MyPlot.Add.Signal(Generate.Sin(51));
            sig1.LegendText = "Sin";

            Signal sig2 = MyPlot.Add.Signal(Generate.Cos(51));
            sig2.LegendText = "Cos";

            MyPlot.Legend.IsVisible = true;
            MyPlot.Legend.Alignment = Alignment.UpperCenter;

            MyPlot.Legend.OutlineColor = Colors.Navy;
            MyPlot.Legend.OutlineWidth = 5;
            MyPlot.Legend.BackgroundColor = Colors.LightBlue;

            MyPlot.Legend.ShadowColor = Colors.Blue.WithOpacity(.2);
            MyPlot.Legend.ShadowOffset = new PixelOffset(10, 10);

            MyPlot.Legend.FontSize = 32;
            MyPlot.Legend.FontName = Fonts.Serif;
        }
    }

    public class LegendOrientation : RecipeBase
    {
        public override string Name => "Legend Orientation";

        public override string Description => "Legend items may be arranged horizontally instead of vertically";

        [Test]
        public override void Execute()
        {
            Signal sig1 = MyPlot.Add.Signal(Generate.Sin(51, phase: .2));
            Signal sig2 = MyPlot.Add.Signal(Generate.Sin(51, phase: .4));
            Signal sig3 = MyPlot.Add.Signal(Generate.Sin(51, phase: .6));

            sig1.LegendText = "Signal 1";
            sig2.LegendText = "Signal 2";
            sig3.LegendText = "Signal 3";

            MyPlot.ShowLegend(Alignment.UpperLeft, Orientation.Horizontal);
        }
    }

    public class LegendWrapping : RecipeBase
    {
        public override string Name => "Legend Wrapping";

        public override string Description => "Legend items may wrap to improve display for a large number of items";

        [Test]
        public override void Execute()
        {
            for (int i = 1; i <= 10; i++)
            {
                double[] data = Generate.Sin(51, phase: .02 * i);
                Signal sig = MyPlot.Add.Signal(data);
                sig.LegendText = $"#{i}";
            }

            MyPlot.Legend.IsVisible = true;
            MyPlot.Legend.Orientation = Orientation.Horizontal;
        }
    }

    public class LegendMultiple : RecipeBase
    {
        public override string Name => "Multiple Legends";

        public override string Description => "Multiple legends may be added to a plot";

        [Test]
        public override void Execute()
        {
            for (int i = 1; i <= 5; i++)
            {
                double[] data = Generate.Sin(51, phase: .02 * i);
                Signal sig = MyPlot.Add.Signal(data);
                sig.LegendText = $"Signal #{i}";
                sig.LineWidth = 2;
            }

            // default legend
            ScottPlot.Legend leg1 = MyPlot.ShowLegend();
            leg1.Alignment = Alignment.LowerRight;
            leg1.Orientation = Orientation.Vertical;

            // additional legend
            ScottPlot.Legend leg2 = MyPlot.Add.Legend();
            leg2.Alignment = Alignment.UpperCenter;
            leg2.Orientation = Orientation.Horizontal;
        }
    }

    public class LegendOutside : RecipeBase
    {
        public override string Name => "Legend Outside the Plot";

        public override string Description => "Use the ShowLegend() overload that accepts an Edge to display the legend outside the data area.";

        [Test]
        public override void Execute()
        {
            Signal sig1 = MyPlot.Add.Signal(Generate.Sin());
            Signal sig2 = MyPlot.Add.Signal(Generate.Cos());

            sig1.LegendText = "Sine";
            sig2.LegendText = "Cosine";

            MyPlot.ShowLegend(Edge.Right);
        }
    }

    private static string GetFontsBasePath() => Path.Combine(Paths.RepoFolder, "src/ScottPlot5/ScottPlot5 Demos/ScottPlot5 WinForms Demo/Fonts");

    public class LegendCustomFontAutomaticItems : RecipeBase
    {
        public override string Name => "Automatic Legend Items Custom Font";

        public override string Description => "Use custom fonts from TTF files in the legend.";

        [Test]
        public override void Execute()
        {
            Fonts.AddFontFile("Alumni Sans", Path.Combine(GetFontsBasePath(), "AlumniSans/AlumniSans-Regular.ttf"), false, false);

            Signal sig1 = MyPlot.Add.Signal(Generate.Sin(51));
            sig1.LegendText = "Sin";

            Signal sig2 = MyPlot.Add.Signal(Generate.Cos(51));
            sig2.LegendText = "Cos";

            MyPlot.Legend.FontName = "Alumni Sans";
            MyPlot.Legend.FontSize = 48;
            MyPlot.Legend.FontColor = Colors.Red;

            MyPlot.ShowLegend();
        }
    }

    public class LegendCustomFontManualItems : RecipeBase
    {
        public override string Name => "Manual Legend Items Custom Font";

        public override string Description => "Use custom fonts from TTF files in the legend (manual legend items).";

        [Test]
        public override void Execute()
        {
            Fonts.AddFontFile("Alumni Sans", Path.Combine(GetFontsBasePath(), "AlumniSans/AlumniSans-Regular.ttf"), false, false);
            Fonts.AddFontFile("Noto Serif Display", Path.Combine(GetFontsBasePath(), "NotoSerifDisplay/NotoSerifDisplay-Regular.ttf"), false, false);

            Signal sig1 = MyPlot.Add.Signal(Generate.Sin(51));
            sig1.LegendText = "Sin";

            Signal sig2 = MyPlot.Add.Signal(Generate.Cos(51));
            sig2.LegendText = "Cos";

            MyPlot.Legend.ManualItems.Add(new LegendItem
            {
                LabelText = "Manual Item 1",
                LabelFontName = "Alumni Sans",
                LabelFontSize = 48,
                LabelFontColor = Colors.Red
            });

            MyPlot.Legend.ManualItems.Add(new LegendItem
            {
                LabelText = "Manual Item 2",
                LabelFontName = "Noto Serif Display",
                LabelFontSize = 32,
                LabelFontColor = Colors.Blue
            });

            MyPlot.ShowLegend();
        }
    }
}
