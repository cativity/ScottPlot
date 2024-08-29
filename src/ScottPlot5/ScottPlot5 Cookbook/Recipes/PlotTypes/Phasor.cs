﻿using JetBrains.Annotations;
using ScottPlot.Plottables;

namespace ScottPlotCookbook.Recipes.PlotTypes;

[UsedImplicitly]
public class Phasor : ICategory
{
    public string Chapter => "Plot Types";

    public string CategoryName => "Phasor Plot";

    public string CategoryDescription => "Phasor plots display vectors on a radial axis centered at the origin";

    public class PhasorQuickstart : RecipeBase
    {
        public override string Name => "Phasor Line Plot";

        public override string Description => "A phasor line plot contains a collection of polar coordinates which are rendered as arrows.";

        [Test]
        public override void Execute()
        {
            // Start by placing a polar axis system on the plot
            PolarAxis polarAxis = MyPlot.Add.PolarAxis(30);
            polarAxis.LinePattern = LinePattern.Dotted;

            // A Phasor may be added with predefined points
            PolarCoordinates[] points1 =
            [
                new PolarCoordinates(10, Angle.FromDegrees(15)),
                new PolarCoordinates(20, Angle.FromDegrees(120)),
                new PolarCoordinates(30, Angle.FromDegrees(240)),
            ];

            MyPlot.Add.Phasor(points1);

            // Points on a Phasor may be added or modified after it is created
            ScottPlot.Plottables.Phasor phaser2 = MyPlot.Add.Phasor();
            phaser2.Points.Add(new PolarCoordinates(20, Angle.FromDegrees(35)));
            phaser2.Points.Add(new PolarCoordinates(25, Angle.FromDegrees(140)));
            phaser2.Points.Add(new PolarCoordinates(20, Angle.FromDegrees(260)));
        }
    }

    public class PhasorLabels : RecipeBase
    {
        public override string Name => "Phasor Plot with Labels";

        public override string Description => "Text labels may be applied to individual arrows of a phasor plot.";

        [Test]
        public override void Execute()
        {
            // setup the polar axis
            PolarAxis polarAxis = MyPlot.Add.PolarAxis(30);
            polarAxis.LinePattern = LinePattern.Dotted;

            // create a phasor plot and points in coordinate space
            ScottPlot.Plottables.Phasor phaser = MyPlot.Add.Phasor();
            phaser.Points.Add(new PolarCoordinates(20, Angle.FromDegrees(35)));
            phaser.Points.Add(new PolarCoordinates(25, Angle.FromDegrees(140)));
            phaser.Points.Add(new PolarCoordinates(20, Angle.FromDegrees(260)));

            // add labels for points
            phaser.Labels.Add("Alpha");
            phaser.Labels.Add("Beta");
            phaser.Labels.Add("Gamma");

            // style the labels
            phaser.LabelStyle.FontSize = 24;
            phaser.LabelStyle.ForeColor = Colors.Black;
            phaser.LabelStyle.FontName = Fonts.Monospace;
            phaser.LabelStyle.Bold = true;
        }
    }
}
