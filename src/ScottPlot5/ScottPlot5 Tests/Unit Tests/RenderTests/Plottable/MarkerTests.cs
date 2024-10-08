﻿using ScottPlot.Palettes;
using ScottPlot.Plottables;

namespace ScottPlotTests.RenderTests.Plottable;

internal class MarkerTests
{
    [Test]
    public void TestMarkerLegend()
    {
        Plot plt = new Plot();

        Marker m = plt.Add.Marker(123, 456);
        m.LegendText = "test";

        plt.ShowLegend();
        plt.Legend.GetItems().Should().HaveCount(1);

        plt.SaveTestImage();
    }

    [Test]
    public void TestAllMarkers()
    {
        Plot plt = new Plot();

        MarkerShape[] markerShapes = [.. Enum.GetValues<MarkerShape>()];
        Category20 palette = new Category20();

        for (int i = 0; i < markerShapes.Length; i++)
        {
            Marker mp1 = plt.Add.Marker(0, -i);
            mp1.MarkerStyle.Shape = markerShapes[i];
            mp1.MarkerStyle.Size = 20;
            mp1.MarkerStyle.LineWidth = 2;
            mp1.MarkerStyle.LineColor = Colors.Blue;
            mp1.MarkerStyle.FillColor = Colors.Green.WithAlpha(.2);
            mp1.MarkerStyle.OutlineColor = Colors.Green;
            mp1.MarkerStyle.OutlineWidth = 2;

            Marker mp2 = plt.Add.Marker(.5, -i);
            mp2.MarkerStyle.Shape = markerShapes[i];
            mp2.MarkerStyle.Size = 20;
            mp2.MarkerStyle.LineWidth = 2;
            mp2.MarkerStyle.OutlineWidth = 2;
            mp2.MarkerStyle.OutlineColor = palette.GetColor(i);
            mp2.MarkerStyle.LineColor = palette.GetColor(i);
            mp2.MarkerStyle.FillColor = palette.GetColor(i).WithAlpha(.3);

            Text txt = plt.Add.Text(markerShapes[i].ToString(), 1, -i);
            txt.LabelAlignment = Alignment.MiddleLeft;
            txt.LabelFontSize = 22;
        }

        plt.Axes.SetLimitsX(-1, 5);
        plt.HideGrid();
        plt.Layout.Frameless();
        plt.SaveTestImage(400, 800);
    }
}
