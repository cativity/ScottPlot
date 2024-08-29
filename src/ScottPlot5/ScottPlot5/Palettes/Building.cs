/* Sourced from RefactoringUI - Building Your Color Palette:
 * https://www.refactoringui.com/previews/building-your-color-palette
 */

namespace ScottPlot.Palettes;

public class Building : IPalette
{
    public string Name => "Building";

    public string Description => string.Empty;

    public Color[] Colors { get; } = Color.FromHex(_hexColors);

    private static readonly string[] _hexColors = ["#FF6F00", "#FF8F00", "#FFA000", "#FFB300", "#FFC107"];

    public Color GetColor(int index) => Colors[index % Colors.Length];
}
