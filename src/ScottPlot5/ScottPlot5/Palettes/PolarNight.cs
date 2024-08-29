/* Sourced from Nord:
 * https://github.com/arcticicestudio/nord
 * https://www.nordtheme.com/docs/colors-and-palettes
 */

namespace ScottPlot.Palettes;

public class PolarNight : IPalette
{
    public string Name => "Polar Night";

    public string Description => "From the Nord collection of palettes: https://github.com/arcticicestudio/nord";

    public Color[] Colors { get; } = Color.FromHex(_hexColors);

    private static readonly string[] _hexColors = ["#2E3440", "#3B4252", "#434C5E", "#4C566A"];

    public Color GetColor(int index) => Colors[index % Colors.Length];
}
