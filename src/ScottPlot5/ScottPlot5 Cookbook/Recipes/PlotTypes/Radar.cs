﻿using JetBrains.Annotations;

namespace ScottPlotCookbook.Recipes.PlotTypes;

[UsedImplicitly]
public class Radar : ICategory
{
    public string Chapter => "Plot Types";

    public string CategoryName => "Radar Plot";

    public string CategoryDescription
        => "Radar charts (also called a spider charts or star charts) "
           + "represent multi-axis data as a 2D shape on axes arranged circularly around a center point.";

    public class RadarQuickstart : RecipeBase
    {
        public override string Name => "Radar Plot Quickstart";

        public override string Description
            => "Radar charts (also called a spider charts or star charts) "
               + "represent multi-axis data as a 2D shape on axes arranged circularly around a center point.";

        private static readonly string[] _labels = ["Axis 1", "Axis 2", "Axis 3", "Axis 4", "Axis 5"];

        [Test]
        public override void Execute()
        {
            // create a collection of objects to describe the data being displayed (each has 5 values)
            List<RadarSeries> radarSeries =
            [
                new RadarSeries { Values = [5, 4, 5, 2, 3], Label = "Green", FillColor = Colors.Green.WithAlpha(.5) },
                new RadarSeries { Values = [2, 3, 2, 4, 2], Label = "Blue", FillColor = Colors.Blue.WithAlpha(.5) }
            ];

            // add radar data to the plot
            ScottPlot.Plottables.Radar radar = MyPlot.Add.Radar(radarSeries);

            // customize radar axis labels (5 axes because each RadarSeries has 5 values)
            radar.Labels = _labels.Select(static s => new LabelStyle { Text = s, Alignment = Alignment.MiddleCenter })
                                  .ToArray();

            MyPlot.Axes.Frameless();
            MyPlot.Axes.Margins(0.5, 0.5);
            MyPlot.ShowLegend();
            MyPlot.HideGrid();
        }
    }
}
