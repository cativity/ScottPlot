using JetBrains.Annotations;
using ScottPlot.Palettes;
using ScottPlot.Plottables;
using ScottPlot.TickGenerators;

namespace ScottPlotCookbook.Recipes.PlotTypes;

[UsedImplicitly]
public class Bar : ICategory
{
    public string Chapter => "Plot Types";

    public string CategoryName => "Bar Plot";

    public string CategoryDescription => "Bar plots represent values as horizontal or vertical rectangles";

    public class Quickstart : RecipeBase
    {
        public override string Name => "Bar Plot Quickstart";

        public override string Description => "Bar plots can be added from a series of values.";

        [Test]
        public override void Execute()
        {
            // add bars
            double[] values = [5, 10, 7, 13];
            MyPlot.Add.Bars(values);

            // tell the plot to autoscale with no padding beneath the bars
            MyPlot.Axes.Margins(bottom: 0);
        }
    }

    public class BarLegend : RecipeBase
    {
        public override string Name => "Bar Plot Legend";

        public override string Description => "A collection of bars can appear in the legend as a single item.";

        [Test]
        public override void Execute()
        {
            double[] xs1 = [1, 2, 3, 4];
            double[] ys1 = [5, 10, 7, 13];
            BarPlot bars1 = MyPlot.Add.Bars(xs1, ys1);
            bars1.LegendText = "Alpha";

            double[] xs2 = [6, 7, 8, 9];
            double[] ys2 = [7, 12, 9, 15];
            BarPlot bars2 = MyPlot.Add.Bars(xs2, ys2);
            bars2.LegendText = "Beta";

            MyPlot.ShowLegend(Alignment.UpperLeft);
            MyPlot.Axes.Margins(bottom: 0);
        }
    }

    public class BarValues : RecipeBase
    {
        public override string Name => "Bar with ns Labels";

        public override string Description => "Set the `Label` property of bars to have text displayed above each bar.";

        [Test]
        public override void Execute()
        {
            double[] values = [5, 10, 7, 13];
            BarPlot barPlot = MyPlot.Add.Bars(values);

            // define the content of labels
            foreach (ScottPlot.Bar bar in barPlot.Bars)
            {
                bar.Label = bar.Value.ToString();
            }

            // customize label style
            barPlot.ValueLabelStyle.Bold = true;
            barPlot.ValueLabelStyle.FontSize = 18;

            MyPlot.Axes.Margins(bottom: 0, top: .2);
        }
    }

    public class BarValuesHorizontal : RecipeBase
    {
        public override string Name => "Bar with ns Labels (horizontal)";

        public override string Description => "Set the `Label` property of bars to have text displayed beside (left or right) of each bar.";

        [Test]
        public override void Execute()
        {
            double[] values = [-20, 10, 7, 13];

            // set the label for each bar
            BarPlot barPlot = MyPlot.Add.Bars(values);

            foreach (ScottPlot.Bar bar in barPlot.Bars)
            {
                bar.Label = "Label " + bar.Value;
            }

            // customize label style
            barPlot.ValueLabelStyle.Bold = true;
            barPlot.ValueLabelStyle.FontSize = 18;
            barPlot.Horizontal = true;

            // add extra margin to account for label
            MyPlot.Axes.SetLimitsX(-45, 35);
            MyPlot.Add.VerticalLine(0, 1, Colors.Black);
        }
    }

    public class BarPosition : RecipeBase
    {
        public override string Name => "Bar Positioning";

        public override string Description => "The exact position and size of each bar may be customized.";

        [Test]
        public override void Execute()
        {
            ScottPlot.Bar[] bars =
            [
                new ScottPlot.Bar { Position = 1, Value = 5, ValueBase = 3, FillColor = Colors.Red },
                new ScottPlot.Bar { Position = 2, Value = 7, ValueBase = 0, FillColor = Colors.Blue },
                new ScottPlot.Bar { Position = 4, Value = 3, ValueBase = 2, FillColor = Colors.Green }
            ];

            MyPlot.Add.Bars(bars);
        }
    }

    public class BarWithError : RecipeBase
    {
        public override string Name => "Bars with Error";

        public override string Description => "Bars can have errorbars.";

        [Test]
        public override void Execute()
        {
            ScottPlot.Bar[] bars =
            [
                new ScottPlot.Bar { Position = 1, Value = 5, Error = 1, FillColor = Colors.Red },
                new ScottPlot.Bar { Position = 2, Value = 7, Error = 2, FillColor = Colors.Orange },
                new ScottPlot.Bar { Position = 3, Value = 6, Error = 1, FillColor = Colors.Green },
                new ScottPlot.Bar { Position = 4, Value = 8, Error = 2, FillColor = Colors.Blue }
            ];

            MyPlot.Add.Bars(bars);

            // tell the plot to autoscale with no padding beneath the bars
            MyPlot.Axes.Margins(bottom: 0);
        }
    }

    public class BarTickLabels : RecipeBase
    {
        public override string Name => "Bars with Labeled Ticks";

        public override string Description => "Bars can be labeled by manually specifying axis tick mark positions and labels.";

        [Test]
        public override void Execute()
        {
            MyPlot.Add.Bar(1, 5, 1);
            MyPlot.Add.Bar(2, 7, 2);
            MyPlot.Add.Bar(3, 6, 1);
            MyPlot.Add.Bar(4, 8, 2);

            Tick[] ticks =
            [
                new Tick(1, "Apple"), new Tick(2, "Orange"), new Tick(3, "Pear"), new Tick(4, "Banana")
            ];

            MyPlot.Axes.Bottom.TickGenerator = new NumericManual(ticks);
            MyPlot.Axes.Bottom.MajorTickStyle.Length = 0;
            MyPlot.HideGrid();

            // tell the plot to autoscale with no padding beneath the bars
            MyPlot.Axes.Margins(bottom: 0);
        }
    }

    public class BarStackVertically : RecipeBase
    {
        public override string Name => "Stacked Bar Plot";

        public override string Description => "Bars can be positioned on top of each other.";

        [Test]
        public override void Execute()
        {
            Category10 palette = new Category10();

            ScottPlot.Bar[] bars =
            [
                // first set of stacked bars
                new ScottPlot.Bar { Position = 1, ValueBase = 0, Value = 2, FillColor = palette.GetColor(0) },
                new ScottPlot.Bar { Position = 1, ValueBase = 2, Value = 5, FillColor = palette.GetColor(1) },
                new ScottPlot.Bar { Position = 1, ValueBase = 5, Value = 10, FillColor = palette.GetColor(2) },

                // second set of stacked bars
                new ScottPlot.Bar { Position = 2, ValueBase = 0, Value = 4, FillColor = palette.GetColor(0) },
                new ScottPlot.Bar { Position = 2, ValueBase = 4, Value = 7, FillColor = palette.GetColor(1) },
                new ScottPlot.Bar { Position = 2, ValueBase = 7, Value = 10, FillColor = palette.GetColor(2) }
            ];

            MyPlot.Add.Bars(bars);

            Tick[] ticks =
            [
                new Tick(1, "Spring"), new Tick(2, "Summer")
            ];

            MyPlot.Axes.Bottom.TickGenerator = new NumericManual(ticks);
            MyPlot.Axes.Bottom.MajorTickStyle.Length = 0;
            MyPlot.HideGrid();

            // tell the plot to autoscale with no padding beneath the bars
            MyPlot.Axes.Margins(bottom: 0);
        }
    }

    public class GroupedBarPlot : RecipeBase
    {
        public override string Name => "Grouped Bar Plot";

        public override string Description => "Bars can be grouped by position and color.";

        [Test]
        public override void Execute()
        {
            Category10 palette = new Category10();

            ScottPlot.Bar[] bars =
            [
                // first group
                new ScottPlot.Bar { Position = 1, Value = 2, FillColor = palette.GetColor(0), Error = 1 },
                new ScottPlot.Bar { Position = 2, Value = 5, FillColor = palette.GetColor(1), Error = 2 },
                new ScottPlot.Bar { Position = 3, Value = 7, FillColor = palette.GetColor(2), Error = 1 },

                // second group
                new ScottPlot.Bar { Position = 5, Value = 4, FillColor = palette.GetColor(0), Error = 2 },
                new ScottPlot.Bar { Position = 6, Value = 7, FillColor = palette.GetColor(1), Error = 1 },
                new ScottPlot.Bar { Position = 7, Value = 13, FillColor = palette.GetColor(2), Error = 3 },

                // third group
                new ScottPlot.Bar { Position = 9, Value = 5, FillColor = palette.GetColor(0), Error = 1 },
                new ScottPlot.Bar { Position = 10, Value = 6, FillColor = palette.GetColor(1), Error = 3 },
                new ScottPlot.Bar { Position = 11, Value = 11, FillColor = palette.GetColor(2), Error = 2 }
            ];

            MyPlot.Add.Bars(bars);

            // build the legend manually
            MyPlot.Legend.IsVisible = true;
            MyPlot.Legend.Alignment = Alignment.UpperLeft;
            MyPlot.Legend.ManualItems.Add(new LegendItem { LabelText = "Monday", FillColor = palette.GetColor(0) });
            MyPlot.Legend.ManualItems.Add(new LegendItem { LabelText = "Tuesday", FillColor = palette.GetColor(1) });
            MyPlot.Legend.ManualItems.Add(new LegendItem { LabelText = "Wednesday", FillColor = palette.GetColor(2) });

            // show group labels on the bottom axis
            Tick[] ticks =
            [
                new Tick(2, "Group 1"), new Tick(6, "Group 2"), new Tick(10, "Group 3")
            ];

            MyPlot.Axes.Bottom.TickGenerator = new NumericManual(ticks);
            MyPlot.Axes.Bottom.MajorTickStyle.Length = 0;
            MyPlot.HideGrid();

            // tell the plot to autoscale with no padding beneath the bars
            MyPlot.Axes.Margins(bottom: 0);
        }
    }

    public class HorizontalBar : RecipeBase
    {
        public override string Name => "Horizontal Bar Plot";

        public override string Description => "Bar plots can be displayed horizontally.";

        [Test]
        public override void Execute()
        {
            ScottPlot.Bar[] bars =
            [
                new ScottPlot.Bar { Position = 1, Value = 5, Error = 1, }, new ScottPlot.Bar { Position = 2, Value = 7, Error = 2, },
                new ScottPlot.Bar { Position = 3, Value = 6, Error = 1, },
                new ScottPlot.Bar { Position = 4, Value = 8, Error = 2, }
            ];

            BarPlot barPlot = MyPlot.Add.Bars(bars);
            barPlot.Horizontal = true;

            MyPlot.Axes.Margins(left: 0);
        }
    }

    public class StackedBars : RecipeBase
    {
        public override string Name => "Stacked Bar Chart";

        public override string Description => "Bars can be stacked to present data in groups.";

        [Test]
        public override void Execute()
        {
            string[] categoryNames = ["Phones", "Computers", "Tablets"];
            Color[] categoryColors = [Colors.C0, Colors.C1, Colors.C2];

            for (int x = 0; x < 4; x++)
            {
                double[] values = Generate.RandomSample(categoryNames.Length, 1000, 5000);

                double nextBarBase = 0;

                for (int i = 0; i < values.Length; i++)
                {
                    ScottPlot.Bar bar = new ScottPlot.Bar
                    {
                        Value = nextBarBase + values[i],
                        FillColor = categoryColors[i],
                        ValueBase = nextBarBase,
                        Position = x,
                    };

                    MyPlot.Add.Bar(bar);

                    nextBarBase += values[i];
                }
            }

            // use custom tick labels on the bottom
            NumericManual tickGen = new NumericManual();

            for (int x = 0; x < 4; x++)
            {
                tickGen.AddMajor(x, $"Q{x + 1}");
            }

            MyPlot.Axes.Bottom.TickGenerator = tickGen;

            // display groups in the legend
            for (int i = 0; i < 3; i++)
            {
                LegendItem item = new LegendItem { LabelText = categoryNames[i], FillColor = categoryColors[i] };
                MyPlot.Legend.ManualItems.Add(item);
            }

            MyPlot.Legend.Orientation = Orientation.Horizontal;
            MyPlot.ShowLegend(Alignment.UpperRight);

            // tell the plot to autoscale with no padding beneath the bars
            MyPlot.Axes.Margins(bottom: 0, top: .3);
        }
    }
}
