using ScottPlot.Panels;
using ScottPlot.Plottables;

namespace ScottPlotTests.RenderTests;

internal class LegendTests
{
    [Test]
    public void TestLegendToggle()
    {
        Plot plt = new Plot();
        Signal sig1 = plt.Add.Signal(Generate.Sin());
        Signal sig2 = plt.Add.Signal(Generate.Cos());

        sig1.LegendText = "Sine";
        sig2.LegendText = "Cosine";

        plt.SaveTestImage(300, 200, "legend-default");

        plt.Legend.IsVisible = true;
        plt.SaveTestImage(300, 200, "legend-enabled");

        plt.Legend.IsVisible = false;
        plt.SaveTestImage(300, 200, "legend-disabled");
    }

    [Test]
    public void TestLegendFontStyle()
    {
        Plot plt = new Plot();

        Signal sig1 = plt.Add.Signal(Generate.Sin());
        Signal sig2 = plt.Add.Signal(Generate.Cos());

        sig1.LegendText = "Sine";
        sig2.LegendText = "Cosine";

        plt.Legend.IsVisible = true;
        plt.Legend.FontSize = 26;
        plt.Legend.FontColor = Colors.Magenta;

        plt.SaveTestImage();
    }

    [Test]
    public void TestLegendImage()
    {
        Plot plt = new Plot();

        Signal sig1 = plt.Add.Signal(Generate.Sin());
        Signal sig2 = plt.Add.Signal(Generate.Cos());

        sig1.LegendText = "Sine";
        sig2.LegendText = "Cosine";

        Image img = plt.GetLegendImage();
        img.SaveTestImage();
    }

    [Test]
    public void TestLegendSvgImage()
    {
        Plot plt = new Plot();

        Signal sig1 = plt.Add.Signal(Generate.Sin());
        Signal sig2 = plt.Add.Signal(Generate.Cos());

        sig1.LegendText = "Sine";
        sig2.LegendText = "Cosine";

        plt.Legend.IsVisible = true;

        string svgXml = plt.GetLegendSvgXml();
        svgXml.SaveTestString(".svg");
    }

    [Test]
    public void TestLegendEmptyWithoutEnabling()
    {
        Plot plt = new Plot();
        plt.GetImage(300, 200);
        plt.GetLegendImage();
        plt.GetLegendSvgXml();
    }

    [Test]
    public void TestLegendEmptyWithEnabling()
    {
        Plot plt = new Plot();
        plt.ShowLegend();
        plt.GetImage(300, 200);
        plt.GetLegendImage();
        plt.GetLegendSvgXml();
    }

    [Test]
    public void TestLegendBasic()
    {
        Plot plt = new Plot();

        Legend leg2 = plt.Add.Legend();
        leg2.Alignment = Alignment.LowerCenter;

        for (int i = 0; i < 5; i++)
        {
            LegendItem item1 = new LegendItem
            {
                LabelText = "ASDFgj",
                LabelFontColor = Colors.Category10[i],
                LabelFontSize = 22,
                LineColor = Colors.Category10[i],
                LineWidth = 3,
            };

            LegendItem item2 = new LegendItem
            {
                LabelText = "ASDF",
                LabelFontColor = Colors.Category10[i],
                LabelFontSize = 22,
                LineColor = Colors.Category10[i],
                LineWidth = 3,
            };

            plt.Legend.ManualItems.Add(item1);
            leg2.ManualItems.Add(item2);
        }

        plt.ShowLegend();

        plt.SaveTestImage();
    }

    [Test]
    public void TestLegendFontOverride()
    {
        Plot plt = new Plot();

        for (int i = 0; i < 5; i++)
        {
            LegendItem item = new LegendItem
            {
                LabelText = $"AgAgAg Item #{i + 1}",
                LabelFontColor = Colors.Category10[i],
                LabelFontSize = 22,
                LineColor = Colors.Category10[i],
                LineWidth = 3,
            };

            plt.Legend.ManualItems.Add(item);
        }

        plt.Legend.FontSize = 12;
        plt.Legend.FontColor = Colors.Magenta;
        plt.Legend.FontName = Fonts.Monospace;

        plt.ShowLegend();
        plt.SaveTestImage();
    }

    private LegendItem[] GetSampleLegendItems(int fontSize = 16)
        => [
            new LegendItem
            {
                LabelText = "Default",
                LabelFontSize = fontSize,
            },
            new LegendItem
            {
                LabelText = "Line",
                LabelFontSize = fontSize,
                LineWidth = 2,
                LineColor = Colors.Blue,
                LinePattern = LinePattern.Dotted,
            },
            new LegendItem
            {
                LabelText = "Fill",
                LabelFontSize = fontSize,
                FillColor = Colors.Green.WithAlpha(.5),
            },
            new LegendItem
            {
                LabelText = "Outline",
                LabelFontSize = fontSize,
                OutlineColor = Colors.Blue,
                OutlineWidth = 2,
            },
            new LegendItem
            {
                LabelText = "Fill+Outline",
                LabelFontSize = fontSize,
                FillColor = Colors.Green.WithAlpha(.5),
                OutlineColor = Colors.Blue,
                OutlineWidth = 2,
            },
            new LegendItem
            {
                LabelText = "Marker",
                LabelFontSize = fontSize,
                MarkerShape = MarkerShape.FilledDiamond,
                MarkerFillColor = Colors.Green.WithAlpha(.5),
                MarkerLineColor = Colors.Blue,
                MarkerLineWidth = 2,
                MarkerSize = 15,
            },
            new LegendItem
            {
                LabelText = "Marker+Line",
                LabelFontSize = fontSize,
                MarkerShape = MarkerShape.FilledCircle,
                MarkerFillColor = Colors.Green.WithAlpha(.5),
                MarkerSize = 10,
                LineWidth = 2,
                LineColor = Colors.Blue,
            },
            new LegendItem
            {
                LabelText = "Arrow",
                LabelFontSize = fontSize,
                ArrowLineWidth = 2,
                ArrowFillColor = Colors.Blue,
                ArrowLineColor = Colors.Transparent,
            },
        ];

    [Test]
    public void TestLegendSymbols()
    {
        Plot plt = new Plot();
        GetSampleLegendItems().ToList().ForEach(plt.Legend.ManualItems.Add);
        plt.ShowLegend();
        plt.SaveTestImage();
    }

    [Test]
    public void TestLegendImageSymbols()
    {
        Plot plt = new Plot();
        GetSampleLegendItems().ToList().ForEach(plt.Legend.ManualItems.Add);
        plt.GetLegendImage().SaveTestImage();
    }

    [Test]
    public void TestLegendMultiLine()
    {
        Plot plt = new Plot();

        for (int i = 0; i < 5; i++)
        {
            Signal sig = plt.Add.Signal(Generate.Sin(phase: i / 20.0));
            sig.LineWidth = 2;
            sig.LegendText = i % 2 == 0 ? "Single Line" : "Multi\nLine";
        }

        plt.ShowLegend();
        plt.SaveTestImage();
    }

    [Test]
    public void TestLegendWrappingHorizontal()
    {
        Plot plt = new Plot();

        for (int i = 0; i < 13; i++)
        {
            Color color = Color.RandomHue();
            LegendItem item = new LegendItem { LabelText = $"Item #{i + 1}", LabelFontColor = color, LabelFontSize = 16, LineColor = color, LineWidth = 3, };
            plt.Legend.ManualItems.Add(item);
        }

        plt.Legend.Orientation = Orientation.Horizontal;
        plt.ShowLegend();

        plt.SaveTestImage();
    }

    [Test]
    public void TestLegendWrappingHorizontalTightWrap()
    {
        Plot plt = new Plot();

        for (int i = 0; i < 5; i++)
        {
            LegendItem item = new LegendItem { LabelText = new string('A', (i * 5) + 1), LineColor = Colors.Blue, LineWidth = 3, };
            plt.Legend.TightHorizontalWrapping = true;
            plt.Legend.ManualItems.Add(item);
        }

        plt.Legend.Orientation = Orientation.Horizontal;
        plt.ShowLegend();

        plt.SaveTestImage();
    }

    [Test]
    public void TestLegendWrappingVertical()
    {
        Plot plt = new Plot();

        for (int i = 0; i < 33; i++)
        {
            Color color = Color.RandomHue();
            LegendItem item = new LegendItem { LabelText = $"Item #{i + 1}", LabelFontColor = color, LabelFontSize = 16, LineColor = color, LineWidth = 3, };
            plt.Legend.ManualItems.Add(item);
        }

        plt.Legend.Orientation = Orientation.Vertical;
        plt.ShowLegend();

        plt.SaveTestImage();
    }

    [Test]
    public void TestPlottablesAppearInLegend()
    {
        double[] xs = [1, 2, 3];
        double[] ys = [1, 2, 3];
        double[] err = [1, 2, 3];

        Plot plt = new Plot();

        plt.Add.Arrow(1, 2, 3, 4);
        plt.Add.VerticalLine(0);
        plt.Add.VerticalSpan(2, 3);
        plt.Add.Bar(2, 3);
        plt.Add.Box(new Box());
        plt.Add.DataLogger();
        plt.Add.DataStreamer(100);
        plt.Add.Ellipse(1, 2, 3, 4);
        plt.Add.ErrorBar(xs, ys, err);
        plt.Add.FillY(xs, ys, err);
        plt.Add.Function(SampleData.DunningKrugerCurve);
        plt.Add.Line(1, 2, 3, 4);
        plt.Add.Marker(1, 2);
        plt.Add.Markers(xs, ys);
        plt.Add.Polygon(xs, ys);
        plt.Add.Rectangle(1, 2, 3, 4);
        plt.Add.Scatter(xs, ys);
        plt.Add.Signal(ys);
        plt.Add.SignalConst(ys);
        plt.Add.SignalXY(xs, ys);

        foreach (IPlottable? plottable in plt.GetPlottables())
        {
            if (plottable is IHasLegendText h)
            {
                h.LegendText = plottable.GetType().Name;
            }
            else
            {
                Assert.Fail($"${plottable} does not implement {nameof(IHasLegendText)}");
            }
        }

        // special cases of plottables with child legend items
        Pie pie = plt.Add.Pie(xs);

        foreach (PieSlice? slice in pie.Slices)
        {
            slice.LegendText = "pie slice";
        }

        plt.GetLegendImage().SaveTestImage();
    }

    [Test]
    public void TestLegendPanel()
    {
        Plot plt = new Plot();

        Signal sig1 = plt.Add.Signal(Generate.Sin());
        Signal sig2 = plt.Add.Signal(Generate.Cos());

        sig1.LegendText = "Sine";
        sig2.LegendText = "Cosine";

        plt.HideLegend(); // hide the default legend

        LegendPanel pan = new LegendPanel(plt.Legend) { Edge = Edge.Right, Alignment = Alignment.UpperCenter, };

        plt.Axes.AddPanel(pan);

        plt.SaveTestImage();
    }

    [Test]
    public void TestCustomFontInLegend()
    {
        string ttfFilePath = Paths.GetTtfFilePaths().First();
        Fonts.AddFontFile("Font Name", ttfFilePath, false, false);

        // create a plot with data
        Plot plot = new Plot();
        Signal sig = plot.Add.Signal(Generate.Sin());
        sig.LegendText = "Sine Wave";

        // axis label custom font
        plot.XLabel("Horizontal Axis Label");
        plot.Axes.Bottom.Label.ForeColor = Colors.Red;
        plot.Axes.Bottom.Label.FontSize = 26;
        plot.Axes.Bottom.Label.FontName = "Font Name";

        // automatic legend items custom font (overwrites manual item customizations)
        //plot.Legend.FontColor = Colors.Green;
        //plot.Legend.FontSize = 26;
        //plot.Legend.FontName= "Font Name";

        // manual legend items custom font
        plot.Legend.ManualItems.Add(new LegendItem
        {
            LabelText = "Manual Item",
            LabelFontColor = Colors.Blue,
            LabelFontSize = 26,
            LabelFontName = "Font Name"
        });

        plot.SaveTestImage();
    }
}
