using System.Reflection;

namespace ScottPlot;

public static class Colormap
{
    /// <summary>
    ///     Return an array containing every available colormap
    /// </summary>
    public static IColormap[] GetColormaps()
        => Assembly.GetExecutingAssembly()
                   .GetTypes()
                   .Where(static x => x.IsClass
                                      && !x.IsAbstract
                                      && x.GetInterfaces().Contains(typeof(IColormap))
                                      && x.GetConstructors().Any(static c => c.GetParameters().Length == 0))
                   .Select(Activator.CreateInstance)
                   .Cast<IColormap>()
                   .ToArray();

    public static Image GetImage(IColormap colormap, int width, int height)
    {
        using SKBitmap bmp = new SKBitmap(width, height);
        using SKCanvas canvas = new SKCanvas(bmp);

        using SKPaint paint = new SKPaint();
        paint.IsAntialias = false;
        paint.IsStroke = true;

        for (int i = 0; i < width; i++)
        {
            paint.Color = colormap.GetColor(i / (width - 1.0)).ToSKColor();
            canvas.DrawLine(i, 0, i, height, paint);
        }

        using MemoryStream ms = new MemoryStream();
        bmp.Encode(ms, SKEncodedImageFormat.Jpeg, 85);
        byte[] bytes = ms.ToArray();

        return new Image(bytes);
    }
}
