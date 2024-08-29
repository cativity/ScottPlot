namespace ScottPlot.Colormaps;

public abstract class ByteColormapBase : ColormapBase
{
    public abstract (byte r, byte g, byte b)[] Rgbs { get; }

    public override Color GetColor(double normalizedIntensity)
    {
        (byte r, byte g, byte b) = Rgbs[(int)(normalizedIntensity * (Rgbs.Length - 1))];

        return new Color(r, g, b);
    }
}
