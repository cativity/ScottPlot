namespace ScottPlot;

public static class Palette
{
    /// <summary>
    /// Create a custom palette from an array of colors
    /// </summary>
    public static IPalette FromColors(string[] hexColors)
    {
        return new Palettes.Custom(hexColors, string.Empty, string.Empty);
    }

    /// <summary>
    /// Create a custom palette from an array of colors
    /// </summary>
    public static IPalette FromColors(Color[] colors)
    {
        return new Palettes.Custom(colors, string.Empty, string.Empty);
    }

    /// <summary>
    /// Return an array containing every available palette
    /// </summary>
    public static IPalette[] GetPalettes()
    {
        return System.Reflection.Assembly.GetExecutingAssembly()
                     .GetTypes()
                     .Where(static x => x.IsClass && !x.IsAbstract && x.GetInterfaces().Contains(typeof(IPalette)) && x.GetConstructors().Any(static x => x.GetParameters().Length == 0))
                     .Select(static x => Activator.CreateInstance(x))
                     .Cast<IPalette>()
                     .ToArray();
    }
}
