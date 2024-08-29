/* Sourced from Color Hex:
 * https://www.color-hex.com/
 * https://www.color-hex.com/color-palette/616
 */

namespace ScottPlot.Palettes;

public class Redness : IPalette
{
    public string Name => "Redness";

    public string Description => string.Empty;

    public Color[] Colors { get; } = Color.FromHex(_hexColors);

    private static readonly string[] _hexColors = ["#FF0000", "#FF4F00", "#FFA900", "#900303", "#FF8181"];

    public Color GetColor(int index) => Colors[index % Colors.Length];
}
