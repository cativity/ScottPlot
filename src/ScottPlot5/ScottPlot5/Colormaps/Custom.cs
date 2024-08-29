namespace ScottPlot.Colormaps;

public class Custom(Color[] colors, string name = "custom") : ColormapBase
{
    public override string Name { get; } = name;

    public override Color GetColor(double position)
    {
        int index = (int)((colors.Length - 1) * position);

        return colors[index];
    }
}
