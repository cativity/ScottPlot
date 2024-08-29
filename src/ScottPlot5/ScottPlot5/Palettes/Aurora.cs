/* Sourced from Nord:
 * https://github.com/arcticicestudio/nord
 * https://www.nordtheme.com/docs/colors-and-palettes
 */

namespace ScottPlot.Palettes;

public class Aurora : IPalette
{
    public string Name => "Aurora";

    public string Description => "From the Nord collection of palettes: https://github.com/arcticicestudio/nord";

    public Color[] Colors { get; } = Color.FromHex(_hexColors);

    private static readonly string[] _hexColors = ["#BF616A", "#D08770", "#EBCB8B", "#A3BE8C", "#B48EAD"];

    public Color GetColor(int index) => Colors[index % Colors.Length];
}
