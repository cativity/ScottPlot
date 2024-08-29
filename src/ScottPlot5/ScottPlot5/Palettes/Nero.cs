/* no info on where this palette originated */

namespace ScottPlot.Palettes;

public class Nero : IPalette
{
    public string Name => "Nero";

    public string Description => string.Empty;

    public Color[] Colors { get; } = Color.FromHex(_hexColors);

    private static readonly string[] _hexColors = ["#013A20", "#478C5C", "#94C973", "#BACC81", "#CDD193"];

    public Color GetColor(int index) => Colors[index % Colors.Length];
}
