/* Sourced from Nord:
 * https://github.com/arcticicestudio/nord
 * https://www.nordtheme.com/docs/colors-and-palettes
 */

namespace ScottPlot.Palettes;

public class Frost : IPalette
{
    public string Name => "Frost";

    public string Description => "From the Nord collection of palettes: https://github.com/arcticicestudio/nord";

    public Color[] Colors { get; } = Color.FromHex(_hexColors);

    private static readonly string[] _hexColors = ["#8FBCBB", "#88C0D0", "#81A1C1", "#5E81AC"];

    public Color GetColor(int index) => Colors[index % Colors.Length];
}
