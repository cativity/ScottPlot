using OpenTK.Graphics;

namespace ScottPlot;

public static class GLExtensions
{
    public static Color4 ToTkColor(this Color color) => new(color.Red, color.Green, color.Blue, color.Alpha);
}
