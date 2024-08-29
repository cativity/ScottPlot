namespace ScottPlot.NamedColors;

public abstract class NamedColorsBase : INamedColors
{
    public Color[] GetAllColors()
    {
        return GetType().GetMethods().Where(static x => x.ReturnType == typeof(Color)).Select(static x => x.Invoke(null, null)).Cast<Color>().ToArray();
    }
}
