/* Sourced from Material Design
 * https://material.io/design/color/the-color-system.html
 */

namespace ScottPlot.Palettes;

public class Amber : IPalette
{
    public string Name => "Amber";

    public string Description => string.Empty;

    public Color[] Colors { get; } = Color.FromHex(_hexColors);

    private static readonly string[] _hexColors = ["#FF6F00", "#FF8F00", "#FFA000", "#FFB300", "#FFC107"];

    public Color GetColor(int index) => Colors[index % Colors.Length];
}
