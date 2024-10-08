﻿using JetBrains.Annotations;
using ScottPlot.Palettes;
using ScottPlot.PathStrategies;
using ScottPlot.Plottables;

namespace ScottPlotCookbook.Recipes.PlotTypes;

[UsedImplicitly]
public class Scatter : ICategory
{
    public string Chapter => "Plot Types";

    public string CategoryName => "Scatter Plot";

    public string CategoryDescription => "Scatter plots display points at X/Y locations in coordinate space.";

    public class ScatterQuickstart : RecipeBase
    {
        public override string Name => "Scatter Plot Quickstart";

        public override string Description => "Scatter plots can be created from two arrays containing X and Y values.";

        [Test]
        public override void Execute()
        {
            double[] xs = [1, 2, 3, 4, 5];
            double[] ys = [1, 4, 9, 16, 25];

            MyPlot.Add.Scatter(xs, ys);
        }
    }

    public class ScatterCoordinates : RecipeBase
    {
        public override string Name => "Scatter Plot Coordinates";

        public override string Description => "Scatter plots can be created from a collection of Coordinates.";

        [Test]
        public override void Execute()
        {
            Coordinates[] coordinates =
            [
                new Coordinates(1, 1), new Coordinates(2, 4), new Coordinates(3, 9), new Coordinates(4, 16), new Coordinates(5, 25)
            ];

            MyPlot.Add.Scatter(coordinates);
        }
    }

    public class ScatterDataType : RecipeBase
    {
        public override string Name => "Scatter Plot Data Type";

        public override string Description => "Scatter plots can be created from any numeric data type, not just double.";

        [Test]
        public override void Execute()
        {
            float[] xs = [1, 2, 3, 4, 5];
            int[] ys = [1, 4, 9, 16, 25];

            MyPlot.Add.Scatter(xs, ys);
        }
    }

    public class ScatterList : RecipeBase
    {
        public override string Name => "Scatter Plot of List Data";

        public override string Description
            => "Scatter plots can be created "
               + "from Lists, but be very cafeful not to add or remove items while a "
               + "render is occurring or you may throw an index exception. "
               + "See documentation about the Render Lock system for details.";

        [Test]
        public override void Execute()
        {
            List<double> xs = [1, 2, 3, 4, 5];
            List<double> ys = [1, 4, 9, 16, 25];

            MyPlot.Add.Scatter(xs, ys);
        }
    }

    public class ScatterLine : RecipeBase
    {
        public override string Name => "Scatter Plot with Lines Only";

        public override string Description
            => "The `ScatterLine()` method can be used to create a scatter plot with a line only (marker size is set to 0).";

        [Test]
        public override void Execute()
        {
            double[] xs = Generate.Consecutive(51);
            double[] sin = Generate.Sin(51);
            double[] cos = Generate.Cos(51);

            MyPlot.Add.ScatterLine(xs, sin);
            MyPlot.Add.ScatterLine(xs, cos);
        }
    }

    public class ScatterPoints : RecipeBase
    {
        public override string Name => "Scatter Plot with Points Only";

        public override string Description
            => "The `ScatterPoints()` method can be used to create a scatter plot with markers only (line width is set to 0).";

        [Test]
        public override void Execute()
        {
            double[] xs = Generate.Consecutive(51);
            double[] sin = Generate.Sin(51);
            double[] cos = Generate.Cos(51);

            MyPlot.Add.ScatterPoints(xs, sin);
            MyPlot.Add.ScatterPoints(xs, cos);
        }
    }

    public class ScatterStyling : RecipeBase
    {
        public override string Name => "Scatter Plot Styling";

        public override string Description
            => "Scatter plots can be extensively styled by interacting "
               + "with the object that is returned after a scatter plot is added. "
               + "Assign text to a scatter plot's Label property to allow it to appear in the legend.";

        [Test]
        public override void Execute()
        {
            double[] xs = Generate.Consecutive(51);
            double[] ys1 = Generate.Sin(51);
            double[] ys2 = Generate.Cos(51);

            ScottPlot.Plottables.Scatter sp1 = MyPlot.Add.Scatter(xs, ys1);
            sp1.LegendText = "Sine";
            sp1.LineWidth = 3;
            sp1.Color = Colors.Magenta;
            sp1.MarkerSize = 15;

            ScottPlot.Plottables.Scatter sp2 = MyPlot.Add.Scatter(xs, ys2);
            sp2.LegendText = "Cosine";
            sp2.LineWidth = 2;
            sp2.Color = Colors.Green;
            sp2.MarkerSize = 10;

            MyPlot.ShowLegend();
        }
    }

    public class ScatterLinePatterns : RecipeBase
    {
        public override string Name => "Scatter Line Patterns";

        public override string Description => "Several line patterns are available";

        [Test]
        public override void Execute()
        {
            LinePattern[] patterns = Enum.GetValues<LinePattern>();
            ColorblindFriendly palette = new ColorblindFriendly();

            for (int i = 0; i < patterns.Length; i++)
            {
                double yOffset = patterns.Length - i;
                double[] xs = Generate.Consecutive(51);
                double[] ys = Generate.Sin(51, offset: yOffset);

                ScottPlot.Plottables.Scatter sp = MyPlot.Add.Scatter(xs, ys);
                sp.LineWidth = 2;
                sp.MarkerSize = 0;
                sp.LinePattern = patterns[i];
                sp.Color = palette.GetColor(i);

                ScottPlot.Plottables.Text txt = MyPlot.Add.Text(patterns[i].ToString(), 51, yOffset);
                txt.LabelFontColor = sp.Color;
                txt.LabelFontSize = 22;
                txt.LabelBold = true;
                txt.LabelAlignment = Alignment.MiddleLeft;
            }

            MyPlot.Axes.Margins(.05, .5, .05, .05);
        }
    }

    public class ScatterGeneric : RecipeBase
    {
        public override string Name => "Scatter Generic";

        public override string Description => "Scatter plots support generic data types, although double is typically the most performant.";

        [Test]
        public override void Execute()
        {
            int[] xs = [1, 2, 3, 4, 5];
            float[] ys = [1, 4, 9, 16, 25];

            MyPlot.Add.Scatter(xs, ys);
        }
    }

    public class ScatterDateTime : RecipeBase
    {
        public override string Name => "Scatter DateTime";

        public override string Description
            => "A scatter plot may use DateTime units but be sure to setup the respective axis to display using DateTime format.";

        [Test]
        public override void Execute()
        {
            DateTime[] xs = Generate.ConsecutiveDays(100);
            double[] ys = Generate.RandomWalk(xs.Length);

            MyPlot.Add.Scatter(xs, ys);
            MyPlot.Axes.DateTimeTicksBottom();
        }
    }

    public class ScatterStep : RecipeBase
    {
        public override string Name => "Step Plot";

        public override string Description
            => "Scatter plots can be created "
               + "using a step plot display where points are connected with right angles "
               + "instead of diagnal lines. The direction of the steps can be customized.";

        [Test]
        public override void Execute()
        {
            double[] xs = Generate.Consecutive(20);
            double[] ys1 = Generate.Consecutive(20, first: 10);
            double[] ys2 = Generate.Consecutive(20, first: 5);
            double[] ys3 = Generate.Consecutive(20, first: 0);

            ScottPlot.Plottables.Scatter sp1 = MyPlot.Add.Scatter(xs, ys1);
            sp1.ConnectStyle = ConnectStyle.Straight;
            sp1.LegendText = "Straight";

            ScottPlot.Plottables.Scatter sp2 = MyPlot.Add.Scatter(xs, ys2);
            sp2.ConnectStyle = ConnectStyle.StepHorizontal;
            sp2.LegendText = "StepHorizontal";

            ScottPlot.Plottables.Scatter sp3 = MyPlot.Add.Scatter(xs, ys3);
            sp3.ConnectStyle = ConnectStyle.StepVertical;
            sp3.LegendText = "StepVertical";

            MyPlot.ShowLegend();
        }
    }

    public class ScatterWithGaps : RecipeBase
    {
        public override string Name => "Scatter with Gaps";

        public override string Description => "NaN values in a scatter plot's data will appear as gaps in the line.";

        [Test]
        public override void Execute()
        {
            double[] xs = Generate.Consecutive(51);
            double[] ys = Generate.Sin(51);

            // long stretch of empty data
            for (int i = 10; i < 20; i++)
            {
                ys[i] = double.NaN;
            }

            // single missing data point
            ys[30] = double.NaN;

            // single floating data point
            for (int i = 35; i < 40; i++)
            {
                ys[i] = double.NaN;
            }

            for (int i = 40; i < 45; i++)
            {
                ys[i] = double.NaN;
            }

            MyPlot.Add.Scatter(xs, ys);
        }
    }

    public class ScatterSmooth : RecipeBase
    {
        public override string Name => "Scatter Plot with Smooth Lines";

        public override string Description
            => "Scatter plots draw straight lines "
               + "between points by default, but setting the Smooth property allows the "
               + "scatter plot to connect points with smooth lines. Lines are smoothed "
               + "using cubic spline interpolation.";

        [Test]
        public override void Execute()
        {
            double[] xs = Generate.Consecutive(10);
            double[] ys = Generate.RandomSample(10, 5, 15);

            ScottPlot.Plottables.Scatter sp = MyPlot.Add.Scatter(xs, ys);
            sp.Smooth = true;
            sp.LegendText = "Smooth";
            sp.LineWidth = 2;
            sp.MarkerSize = 10;
        }
    }

    public class ScatterSmoothTension : RecipeBase
    {
        public override string Name => "Smooth Line Tension";

        public override string Description
            => "Tension of smooth lines can be "
               + "adjusted for some smoothing strategies. "
               + "Low tensions lead to 'overshoot' and high tensions produce curves"
               + "which appear more like straight lines.";

        [Test]
        public override void Execute()
        {
            double[] xs = Generate.RandomWalk(10);
            double[] ys = Generate.RandomWalk(10);

            Markers mk = MyPlot.Add.Markers(xs, ys);
            mk.MarkerShape = MarkerShape.OpenCircle;
            mk.Color = Colors.Black;

            foreach (double tension in (double[])[0.3, 0.5, 1.0, 3.0])
            {
                ScottPlot.Plottables.Scatter sp = MyPlot.Add.ScatterLine(xs, ys);
                sp.Smooth = true;
                sp.SmoothTension = tension;
                sp.LegendText = $"Tension {tension}";
                sp.LineWidth = 2;
            }

            MyPlot.ShowLegend(Alignment.UpperLeft);
        }
    }

    public class ScatterQuad : RecipeBase
    {
        public override string Name => "Smooth Scatter without Overshoot";

        public override string Description
            => "The quadratic half point path strategy "
               + "allows scatter plots to be displayed with smooth lines connecting points, but "
               + "lines are eased in and out of points so they never 'overshoot' the values vertically.";

        [Test]
        public override void Execute()
        {
            double[] xs = Generate.Consecutive(10);
            double[] ys = Generate.RandomSample(10, 5, 15);

            ScottPlot.Plottables.Scatter sp = MyPlot.Add.Scatter(xs, ys);
            sp.PathStrategy = new QuadHalfPoint();
            sp.LegendText = "Smooth";
            sp.LineWidth = 2;
            sp.MarkerSize = 10;
        }
    }

    public class ScatterLimitIndex : RecipeBase
    {
        public override string Name => "Limiting Display with Render Indexes";

        public override string Description
            => "Although a scatter plot may contain "
               + "a very large amount of data, much of it may be unpopulated. The user can "
               + "define min and max render indexes, and only values within that range will "
               + "be displayed when the scatter plot is rendered.";

        [Test]
        public override void Execute()
        {
            double[] xs = Generate.Consecutive(51);
            double[] ys = Generate.Sin(51);

            ScottPlot.Plottables.Scatter sp = MyPlot.Add.Scatter(xs, ys);
            sp.MinRenderIndex = 10;
            sp.MaxRenderIndex = 40;
        }
    }

    public class ScatterFill : RecipeBase
    {
        public override string Name => "Scatter Plot with Fill";

        public override string Description => "The area beneath a scatter plot can be filled.";

        [Test]
        public override void Execute()
        {
            double[] xs = Generate.Consecutive(51);
            double[] ys = Generate.Sin(51);

            ScottPlot.Plottables.Scatter sp = MyPlot.Add.Scatter(xs, ys);
            sp.FillY = true;
            sp.FillYColor = sp.Color.WithAlpha(.2);
        }
    }

    public class ScatterFillValue : RecipeBase
    {
        public override string Name => "Scatter Plot Filled to a ns";

        public override string Description => "The base of the fill can be defined.";

        [Test]
        public override void Execute()
        {
            double[] xs = Generate.Consecutive(51);
            double[] ys = Generate.Sin(51);

            ScottPlot.Plottables.Scatter sp = MyPlot.Add.Scatter(xs, ys);
            sp.FillY = true;
            sp.FillYColor = sp.Color.WithAlpha(.2);
            sp.FillYValue = 0.6;
        }
    }

    public class ScatterFillAboveBelow : RecipeBase
    {
        public override string Name => "Scatter Plot Filled Above and Below";

        public override string Description => "Filled areas above and below the FillY value can be individually customized";

        [Test]
        public override void Execute()
        {
            double[] xs = Generate.Consecutive(51);
            double[] ys = Generate.Sin(51);

            ScottPlot.Plottables.Scatter sp = MyPlot.Add.Scatter(xs, ys);
            sp.FillY = true;
            sp.FillYValue = 0;
            sp.FillYAboveColor = Colors.Green.WithAlpha(.2);
            sp.FillYBelowColor = Colors.Red.WithAlpha(.2);
        }
    }

    public class ScatterFillGradient : RecipeBase
    {
        public override string Name => "Scatter Plot with Gradient Fill";

        public override string Description => "The area beneath a scatter plot can be filled with a custom gradient of colors.";

        [Test]
        public override void Execute()
        {
            double[] xs = Generate.Consecutive(51);
            double[] ys = Generate.Sin(51);

            ScottPlot.Plottables.Scatter poly = MyPlot.Add.ScatterLine(xs, ys);

            poly.FillY = true;
            poly.ColorPositions.Add(new ScottPlot.Plottables.Scatter.ColorPosition(Colors.Red, 0));
            poly.ColorPositions.Add(new ScottPlot.Plottables.Scatter.ColorPosition(Colors.Orange, 10));
            poly.ColorPositions.Add(new ScottPlot.Plottables.Scatter.ColorPosition(Colors.Yellow, 20));
            poly.ColorPositions.Add(new ScottPlot.Plottables.Scatter.ColorPosition(Colors.Green, 30));
            poly.ColorPositions.Add(new ScottPlot.Plottables.Scatter.ColorPosition(Colors.Blue, 40));
            poly.ColorPositions.Add(new ScottPlot.Plottables.Scatter.ColorPosition(Colors.Violet, 50));
        }
    }

    public class ScatterScaleAndOffset : RecipeBase
    {
        public override string Name => "Scatter Scale and Offset";

        public override string Description
            => "Scatter plot points can be multiplied by custom X and Y scale factors, or shifted horizontally or vertically using X and Y offset values.";

        [Test]
        public override void Execute()
        {
            double[] xs = Generate.Consecutive(51);
            double[] ys = Generate.Sin(51);
            ScottPlot.Plottables.Scatter sp = MyPlot.Add.Scatter(xs, ys);
            sp.ScaleX = 100;
            sp.ScaleY = 10;
            sp.OffsetX = 500;
            sp.OffsetY = 5;
        }
    }

    public class StackedFilledLinePlot : RecipeBase
    {
        public override string Name => "Stacked Filled Line Plot";

        public override string Description => "A stacked filled line plot effect can be achieved by overlapping ScatterLines that fill area.";

        [Test]
        public override void Execute()
        {
            // create sample data
            double[] xs = [1, 2, 3, 4];
            double[] ys1 = [1, 3, 1, 2];
            double[] ys2 = [3, 7, 3, 1];
            double[] ys3 = [5, 2, 5, 6];

            // shift each plot vertically by the sum of all plots before it
            ys2 = Enumerable.Range(0, ys2.Length).Select(x => ys2[x] + ys1[x]).ToArray();
            ys3 = Enumerable.Range(0, ys2.Length).Select(x => ys3[x] + ys2[x]).ToArray();

            // plot the padded data points as ScatterLine
            ScottPlot.Plottables.Scatter sp3 = MyPlot.Add.ScatterLine(xs, ys3, Colors.Black);
            ScottPlot.Plottables.Scatter sp2 = MyPlot.Add.ScatterLine(xs, ys2, Colors.Black);
            ScottPlot.Plottables.Scatter sp1 = MyPlot.Add.ScatterLine(xs, ys1, Colors.Black);

            // set plot style
            sp1.LineWidth = 2;
            sp2.LineWidth = 2;
            sp3.LineWidth = 2;
            sp1.FillY = true;
            sp2.FillY = true;
            sp3.FillY = true;
            sp1.FillYColor = Colors.Green;
            sp2.FillYColor = Colors.Orange;
            sp3.FillYColor = Colors.Blue;

            // use tight margins so data goes to the edge of the plot
            MyPlot.Axes.Margins(0, 0, 0, 0.1);
        }
    }
}
