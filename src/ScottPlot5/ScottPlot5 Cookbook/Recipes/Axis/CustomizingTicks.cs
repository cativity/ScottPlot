using JetBrains.Annotations;
using ScottPlot.AxisPanels;
using ScottPlot.Plottables;
using ScottPlot.TickGenerators;
using SkiaSharp;

namespace ScottPlotCookbook.Recipes.Axis;

[UsedImplicitly]
public class CustomizingTicks : ICategory
{
    public string Chapter => "Axis";

    public string CategoryName => "Customizing Ticks";

    public string CategoryDescription => "Advanced customization of tick marks and tick labels";

    public class CustomTickFormatter : RecipeBase
    {
        public override string Name => "Custom Tick Formatters";

        public override string Description
            => "Users can customize the logic used to create "
               + "tick labels from tick positions. "
               + "Old versions of ScottPlot achieved this using a ManualTickPositions method.";

        [Test]
        public override void Execute()
        {
            double[] xs = Generate.Consecutive(100, 1, -50);
            MyPlot.Add.Scatter(xs, Generate.Sin(100));
            MyPlot.Add.Scatter(xs, Generate.Cos(100));

            // create a custom tick generator using your custom label formatter
            // tell an axis to use the custom tick generator
            MyPlot.Axes.Bottom.TickGenerator = new NumericAutomatic { LabelFormatter = CustomFormatter };

            return;

            // create a static function containing the string formatting logic
            static string CustomFormatter(double position)
            {
                return position switch
                {
                    0 => "0",
                    > 0 => $"+{position}",
                    _ => $"({-position})"
                };
            }
        }
    }

    public class DateTimeAutomaticTickFormatter : RecipeBase
    {
        public override string Name => "DateTimeAutomatic Tick Formatters";

        public override string Description => "Users can customize the logic used to create datetime tick labels from tick positions. ";

        [Test]
        public override void Execute()
        {
            // plot data using DateTime values on the horizontal axis
            DateTime[] xs = Generate.ConsecutiveHours(100);
            double[] ys = Generate.RandomWalk(100);
            MyPlot.Add.Scatter(xs, ys);

            // setup the bottom axis to use DateTime ticks
            DateTimeXAxis axis = MyPlot.Axes.DateTimeTicksBottom();

            // apply our custom tick formatter
            DateTimeAutomatic? tickGen = axis.TickGenerator as DateTimeAutomatic;
            Assert.That(tickGen, Is.Not.Null);
            tickGen.LabelFormatter = CustomFormatter;

            return;

            // create a custom formatter to return a string with
            // date only when zoomed out and time only when zoomed in
            static string CustomFormatter(DateTime dt)
            {
                bool isMidnight = dt is { Hour: 0, Minute: 0, Second: 0 };

                return isMidnight ? DateOnly.FromDateTime(dt).ToString() : TimeOnly.FromDateTime(dt).ToString();
            }
        }
    }

    public class AltTickGen : RecipeBase
    {
        public override string Name => "Custom Tick Generators";

        public override string Description
            => "Tick generators determine where ticks are to be placed and also "
               + "contain logic for generating tick labels from tick positions. "
               + "Alternative tick generators can be created and assigned to axes. "
               + "Some common tick generators are provided with ScottPlot, "
               + "and users also have the option create their own.";

        [Test]
        public override void Execute()
        {
            MyPlot.Add.Signal(Generate.Sin(51));
            MyPlot.Add.Signal(Generate.Cos(51));

            MyPlot.Axes.Bottom.TickGenerator = new NumericFixedInterval(11);
        }
    }

    public class SetTicks : RecipeBase
    {
        public override string Name => "SetTicks Shortcut";

        public override string Description
            => "The default axes have a SetTicks() helper method which replaces "
               + "the default tick generator with a manual tick generator pre-loaded with the provided ticks.";

        [Test]
        public override void Execute()
        {
            // display sample data
            MyPlot.Add.Signal(Generate.Sin());
            MyPlot.Add.Signal(Generate.Cos());

            // use manually defined ticks
            double[] tickPositions = [10, 25, 40];
            string[] tickLabels = ["Alpha", "Beta", "Gamma"];
            MyPlot.Axes.Bottom.SetTicks(tickPositions, tickLabels);
        }
    }

    public class CustomTicks : RecipeBase
    {
        public override string Name => "Custom Tick Positions";

        public override string Description
            => "Users desiring more control over major and minor "
               + "tick positions and labels can instantiate a manual tick generator, set it up as desired, "
               + "then assign it to the axis being customized";

        [Test]
        public override void Execute()
        {
            // display sample data
            MyPlot.Add.Signal(Generate.Sin());
            MyPlot.Add.Signal(Generate.Cos());

            // create a manual tick generator and add ticks
            NumericManual ticks = new NumericManual();

            // add major ticks with their labels
            ticks.AddMajor(0, "zero");
            ticks.AddMajor(20, "twenty");
            ticks.AddMajor(50, "fifty");

            // add minor ticks
            ticks.AddMinor(22);
            ticks.AddMinor(25);
            ticks.AddMinor(32);
            ticks.AddMinor(35);
            ticks.AddMinor(42);
            ticks.AddMinor(45);

            // tell the horizontal axis to use the custom tick generator
            MyPlot.Axes.Bottom.TickGenerator = ticks;
        }
    }

    public class RotatedTicks : RecipeBase
    {
        public override string Name => "Rotated Tick Labels";

        public override string Description => "Users can customize tick label rotation.";

        [Test]
        public override void Execute()
        {
            MyPlot.Add.Signal(Generate.Sin());
            MyPlot.Add.Signal(Generate.Cos());

            MyPlot.Axes.Bottom.TickLabelStyle.Rotation = -45;
            MyPlot.Axes.Bottom.TickLabelStyle.Alignment = Alignment.MiddleRight;
        }
    }

    public class RotatedTicksLongLabels : RecipeBase
    {
        public override string Name => "Rotated Tick with Long Labels";

        public override string Description => "The axis size can be increased to accommodate rotated or long tick labels.";

        [Test]
        public override void Execute()
        {
            // create a bar plot
            double[] values = [5, 10, 7, 13, 25, 60];
            MyPlot.Add.Bars(values);
            MyPlot.Axes.Margins(bottom: 0);

            // create a tick for each bar
            Tick[] ticks =
            [
                new Tick(0, "First Long Title"),
                new Tick(1, "Second Long Title"),
                new Tick(2, "Third Long Title"),
                new Tick(3, "Fourth Long Title"),
                new Tick(4, "Fifth Long Title"),
                new Tick(5, "Sixth Long Title")
            ];

            MyPlot.Axes.Bottom.TickGenerator = new NumericManual(ticks);
            MyPlot.Axes.Bottom.TickLabelStyle.Rotation = 45;
            MyPlot.Axes.Bottom.TickLabelStyle.Alignment = Alignment.MiddleLeft;

            // determine the width of the largest tick label
            using SKPaint paint = new SKPaint();

            float largestLabelWidth = ticks.Max(tick => MyPlot.Axes.Bottom.TickLabelStyle.Measure(tick.Label, paint).Size.Width);

            // ensure axis panels do not get smaller than the largest label
            MyPlot.Axes.Bottom.MinimumSize = largestLabelWidth;
            MyPlot.Axes.Right.MinimumSize = largestLabelWidth;
        }
    }

    public class DisableGridLines : RecipeBase
    {
        public override string Name => "Disable Grid Lines";

        public override string Description => "Users can disable grid lines for specific axes.";

        [Test]
        public override void Execute()
        {
            MyPlot.Add.Signal(Generate.Sin());
            MyPlot.Add.Signal(Generate.Cos());

            MyPlot.Grid.XAxisStyle.IsVisible = true;
            MyPlot.Grid.YAxisStyle.IsVisible = false;
        }
    }

    public class MinimumTickSpacing : RecipeBase
    {
        public override string Name => "Minimum Tick Spacing";

        public override string Description
            => "Space between ticks can be increased by setting a value to indicate the minimum distance between tick labels (in pixels).";

        [Test]
        public override void Execute()
        {
            MyPlot.Add.Signal(Generate.Sin(51));
            MyPlot.Add.Signal(Generate.Cos(51));

            MyPlot.Axes.Bottom.TickGenerator = new NumericAutomatic { MinimumTickSpacing = 50 };

            MyPlot.Axes.Left.TickGenerator = new NumericAutomatic { MinimumTickSpacing = 25 };
        }
    }

    public class TickDensity : RecipeBase
    {
        public override string Name => "Tick Density";

        public override string Description
            => "Tick density can be adjusted as a fraction of the default value. "
               + "Unlike MinimumTickSpacing, this strategy is aware of the size of "
               + "tick labels and adjusts accordingly.";

        [Test]
        public override void Execute()
        {
            MyPlot.Add.Signal(Generate.Sin(51));
            MyPlot.Add.Signal(Generate.Cos(51));

            MyPlot.Axes.Bottom.TickGenerator = new NumericAutomatic { TickDensity = 0.2 };

            MyPlot.Axes.Left.TickGenerator = new NumericAutomatic { TickDensity = 0.2 };
        }
    }

    public class TickCount : RecipeBase
    {
        public override string Name => "Tick Count";

        public override string Description
            => "A target number of ticks can be provided and the automatic "
               + "tick generator will attempt to place that number of ticks. "
               + "This strategy allows tick density to decrease as the image size increases.";

        [Test]
        public override void Execute()
        {
            MyPlot.Add.Signal(Generate.Sin(51));
            MyPlot.Add.Signal(Generate.Cos(51));

            MyPlot.Axes.Bottom.TickGenerator = new NumericAutomatic { TargetTickCount = 3 };

            MyPlot.Axes.Left.TickGenerator = new NumericAutomatic { TargetTickCount = 3 };
        }
    }

    public class StandardMinorTickDistribution : RecipeBase
    {
        public override string Name => "Minor Tick Density";

        public override string Description
            => "Minor tick marks are automatically generated at intervals between major tick marks. "
               + "By default they are evenly spaced, but their density may be customized.";

        [Test]
        public override void Execute()
        {
            // plot sample data
            double[] xs = Generate.Consecutive(100);
            double[] ys = Generate.NoisyExponential(100);
            Scatter sp = MyPlot.Add.Scatter(xs, ys);
            sp.LineWidth = 0;

            // create a minor tick generator with 10 minor ticks per major tick
            EvenlySpacedMinorTickGenerator minorTickGen = new EvenlySpacedMinorTickGenerator(10);

            // create a numeric tick generator that uses our custom minor tick generator

            // tell the left axis to use our custom tick generator
            MyPlot.Axes.Left.TickGenerator = new NumericAutomatic { MinorTickGenerator = minorTickGen };
        }
    }

    public class LogScaleTicks : RecipeBase
    {
        public override string Name => "Log Scale Tick Marks";

        public override string Description
            => "The appearance of logarithmic scaling can be achieved by log-scaling the data to be displayed then customizing the minor ticks and grid.";

        [Test]
        public override void Execute()
        {
            // start with original data
            double[] xs = Generate.Consecutive(100);
            double[] ys = Generate.NoisyExponential(100);

            // log-scale the data and account for negative values
            double[] logYs = ys.Select(Math.Log10).ToArray();

            // add log-scaled data to the plot
            Scatter sp = MyPlot.Add.Scatter(xs, logYs);
            sp.LineWidth = 0;

            // create a minor tick generator that places log-distributed minor ticks
            LogMinorTickGenerator minorTickGen = new LogMinorTickGenerator();

            // create a numeric tick generator that uses our custom minor tick generator

            // tell the left axis to use our custom tick generator
            MyPlot.Axes.Left.TickGenerator = new NumericAutomatic
            {
                MinorTickGenerator = minorTickGen, // tell our major tick generator to only show major ticks that are whole integers
                IntegerTicksOnly = true, // tell our custom tick generator to use our new label formatter
                LabelFormatter = LogTickLabelFormatter
            };

            // show grid lines for minor ticks
            MyPlot.Grid.MajorLineColor = Colors.Black.WithOpacity(.15);
            MyPlot.Grid.MinorLineColor = Colors.Black.WithOpacity(.05);
            MyPlot.Grid.MinorLineWidth = 1;

            return;

            // create a custom tick formatter to set the label text for each tick
            static string LogTickLabelFormatter(double y) => $"{Math.Pow(10, y):N0}";
        }
    }
}
