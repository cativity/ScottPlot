﻿using JetBrains.Annotations;

namespace ScottPlotCookbook.Recipes.PlotTypes;

[UsedImplicitly]
public class SignalXY : ICategory
{
    public string Chapter => "Plot Types";

    public string CategoryName => "SignalXY Plot";

    public string CategoryDescription
        => "SignalXY are a high performance plot type "
           + "optimized for X/Y pairs where the X values are always ascending. "
           + "For large datasets SignalXY plots are much more performant than "
           + "Scatter plots (which allow unordered X points) but not as performant "
           + "as Signal plots (which require fixed spacing between X points).";

    public class SignalXYQuickstart : RecipeBase
    {
        public override string Name => "SignalXY Quickstart";

        public override string Description => "SignalXY plots are a high performance plot type for X/Y data where the X values are always ascending.";

        [Test]
        public override void Execute()
        {
            // generate sample data with gaps
            List<double> xList = [];
            List<double> yList = [];

            for (int i = 0; i < 5; i++)
            {
                xList.AddRange(Generate.Consecutive(1000, first: 2000 * i));
                yList.AddRange(Generate.RandomSample(1000));
            }

            double[] xs = [.. xList];
            double[] ys = [.. yList];

            // add a SignalXY plot
            MyPlot.Add.SignalXY(xs, ys);
        }
    }

    public class SignalXYGeneric : RecipeBase
    {
        public override string Name => "SignalXY Generic";

        public override string Description => "SignalXY plots support generic data types, although double is typically the most performant.";

        [Test]
        public override void Execute()
        {
            // generate sample data with gaps
            List<int> xList = [];
            List<float> yList = [];

            for (int i = 0; i < 5; i++)
            {
                xList.AddRange(Generate.Consecutive(1000, first: 2000 * i).Select(static x => (int)x));
                yList.AddRange(Generate.RandomSample(1000).Select(static x => (float)x));
            }

            int[] xs = [.. xList];
            float[] ys = [.. yList];

            // add a SignalXY plot
            MyPlot.Add.SignalXY(xs, ys);
        }
    }

    public class SignalXYRenderIndexes : RecipeBase
    {
        public override string Name => "Partial SignalXY Rendering";

        public override string Description
            => "Even if a SignalXY plot references a "
               + "large array of data, rendering can be limited to a range of values. If set,"
               + "only the range of data between the minimum and maximum render indexes will be displayed.";

        [Test]
        public override void Execute()
        {
            double[] xs = Generate.Consecutive(1000);
            double[] ys = Generate.RandomWalk(1000);

            ScottPlot.Plottables.SignalXY sigAll = MyPlot.Add.SignalXY(xs, ys);
            sigAll.LegendText = "Full";
            sigAll.Data.YOffset = 80;

            ScottPlot.Plottables.SignalXY sigLeft = MyPlot.Add.SignalXY(xs, ys);
            sigLeft.LegendText = "Left";
            sigLeft.Data.YOffset = 60;
            sigLeft.Data.MaximumIndex = 700;

            ScottPlot.Plottables.SignalXY sigRight = MyPlot.Add.SignalXY(xs, ys);
            sigRight.LegendText = "Right";
            sigRight.Data.YOffset = 40;
            sigRight.Data.MinimumIndex = 300;

            ScottPlot.Plottables.SignalXY sigMid = MyPlot.Add.SignalXY(xs, ys);
            sigMid.LegendText = "Mid";
            sigMid.Data.YOffset = 20;
            sigMid.Data.MinimumIndex = 300;
            sigMid.Data.MaximumIndex = 700;

            MyPlot.ShowLegend(Alignment.UpperRight);
            MyPlot.Axes.Margins(top: .5);
        }
    }

    public class SignalXYOffset : RecipeBase
    {
        public override string Name => "SignalXY Offset";

        public override string Description => "A fixed offset can be applied to SignalXY plots.";

        [Test]
        public override void Execute()
        {
            double[] xs = Generate.Consecutive(1000);
            double[] ys = Generate.Sin(1000);

            _ = MyPlot.Add.SignalXY(xs, ys);
            //ScottPlot.Plottables.SignalXY sig1 = MyPlot.Add.SignalXY(xs, ys);

            ScottPlot.Plottables.SignalXY sig2 = MyPlot.Add.SignalXY(xs, ys);
            sig2.Data.XOffset = 250;
            sig2.Data.YOffset = .5;
        }
    }

    public class SignalXYOffsetScaleY : RecipeBase
    {
        public override string Name => "SignalXY Scaling";

        public override string Description => "SignalXY plots can be scaled vertically according to a user-defined amount.";

        [Test]
        public override void Execute()
        {
            // plot values between -1 and 1
            double[] values = Generate.Sin(51);
            double[] xs = Generate.Consecutive(51);
            ScottPlot.Plottables.SignalXY signalXY = MyPlot.Add.SignalXY(xs, values);

            // increase the vertical scaling
            signalXY.Data.YScale = 500;
        }
    }

    public class VerticalSignalXY : RecipeBase
    {
        public override string Name => "Vertical SignalXY";

        public override string Description
            => "Although SignalXY plots typically display data left-to-right, it is possible to use this plot type to display data bottom-to-top.";

        [Test]
        public override void Execute()
        {
            double[] xs = Generate.Consecutive(1000);
            double[] ys = Generate.RandomWalk(1000);

            ScottPlot.Plottables.SignalXY sig1 = MyPlot.Add.SignalXY(xs, ys);
            sig1.Data.Rotated = true;
        }
    }

    public class SignalXYVerticalInvertedX : RecipeBase
    {
        public override string Name => "Vertical SignalXY with Inverted X Axis";

        public override string Description
            => "Demonstrates how to display a rotated "
               + "SignalXY plot (so it goes from bottom to top) which is also displayed "
               + "on an inverted horizontal axis (where positive values are on the left).";

        [Test]
        public override void Execute()
        {
            // add a signal plot
            double[] xs = Generate.Consecutive(5_000);
            double[] ys = Generate.Sin(xs.Length, oscillations: 4);

            // rotate it so it is vertical
            ScottPlot.Plottables.SignalXY signal = MyPlot.Add.SignalXY(xs, ys);
            signal.Data.Rotated = true;

            // invert the horizontal axis
            MyPlot.Axes.SetLimitsX(1, -1);
        }
    }

    public class SignalXYVerticalInvertedY : RecipeBase
    {
        public override string Name => "Vertical SignalXY with Inverted Y Axis";

        public override string Description
            => "Demonstrates how to display a rotated SignalXY plot on an inverted vertical axis so data goes from top to bottom.";

        [Test]
        public override void Execute()
        {
            // add a signal plot
            double[] xs = Generate.Consecutive(5_000);
            double[] ys = Generate.Sin(xs.Length, oscillations: 4);

            // rotate it so it is vertical
            ScottPlot.Plottables.SignalXY signal = MyPlot.Add.SignalXY(xs, ys);
            signal.Data.Rotated = true;

            // invert the vertical axis
            MyPlot.Axes.SetLimitsY(5000, 0);
        }
    }

    public class SignalXYMarkers : RecipeBase
    {
        public override string Name => "SignalXY with Markers";

        public override string Description
            => "Users can enable a marker to be displayed at each data point. However, this can reduce performance for extremely large datasets.";

        [Test]
        public override void Execute()
        {
            double[] xs = Generate.Consecutive(51);
            double[] ys = Generate.Sin(51);

            ScottPlot.Plottables.SignalXY sig = MyPlot.Add.SignalXY(xs, ys);
            sig.MarkerStyle.Shape = MarkerShape.FilledCircle;
            sig.MarkerStyle.Size = 5;
        }
    }
}
