﻿namespace ScottPlot;

public class RadarSeries : LabelStyleProperties, IHasLabel, IHasLegendText
{
    public string Label { get => LegendText; set => LegendText = value; }

    public string LegendText { get => LabelStyle.Text; set => LabelStyle.Text = value; }

    public FillStyle Fill { get; set; } = new FillStyle();

    public override LabelStyle LabelStyle { get; set; } = new LabelStyle { Alignment = Alignment.MiddleCenter };

    public Color FillColor { get => Fill.Color; set => Fill.Color = value; }

    public IReadOnlyList<double> Values { get; set; } = [];

    public RadarSeries()
    {
    }

    public RadarSeries(IReadOnlyList<double> values, Color color)
    {
        Values = values;
        Fill.Color = color;
    }

    public RadarSeries(IReadOnlyList<double> values, Color color, string label)
    {
        Values = values;
        LegendText = label;
        Fill.Color = color;
    }
}
