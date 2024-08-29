using ScottPlot.Colormaps;

namespace ScottPlot;

public static class SampleImages
{
    public static Image MonaLisa()
    {
        double[,] data = SampleData.MonaLisa();
        int height = data.GetLength(0);
        int width = data.GetLength(1);
        Range range = Range.GetRange(data);

        Viridis colormap = new Viridis();

        uint[] argb = new uint[data.Length];

        for (int y = 0; y < height; y++)
        {
            int rowOffset = y * width;

            for (int x = 0; x < width; x++)
            {
                argb[rowOffset + x] = colormap.GetColor(data[y, x], range).ARGB;
            }
        }

        SKBitmap bmp = Drawing.BitmapFromArgbs(argb, width, height);

        return new Image(bmp);
    }

    public static Image ScottPlotLogo(int width = 256, int height = 256)
    {
        using SKSurface surface = Drawing.CreateSurface(width, height);
        using SKCanvas canvas = surface.Canvas;
        using SKPaint paint = new SKPaint();
        PixelRect canvasRect = new PixelRect(0, width, height, 0);

        FillStyle fillStyle = new FillStyle();
        LineStyle lineStyle = new LineStyle();

        // purple upper background
        fillStyle.Color = Color.FromHex("#67217a");
        Drawing.FillRectangle(canvas, canvasRect, paint, fillStyle);

        // pink lower background
        Pixel[] pointsLowerBackground =
        [
            new Pixel(0 * width, height),
            new Pixel(0 * width, .8 * height),
            new Pixel(.3 * width, .5 * height),
            new Pixel(.5 * width, .65 * height),
            new Pixel(1 * width, .16 * height),
            new Pixel(1 * width, height),
        ];

        fillStyle.Color = Color.FromHex("#9a4993");
        FillPolygon(canvas, canvasRect, paint, fillStyle, pointsLowerBackground);

        // white ziggy
        Pixel[] pointsZiggy =
        [
            new Pixel(0 * width, .7 * height),
            new Pixel(.3 * width, .4 * height),
            new Pixel(.5 * width, .55 * height),
            new Pixel(1 * width, .06 * height),
            new Pixel(1 * width, .27 * height),
            new Pixel(.5 * width, .75 * height),
            new Pixel(.3 * width, .6 * height),
            new Pixel(0 * width, .9 * height),
        ];

        fillStyle.Color = Colors.White;
        FillPolygon(canvas, canvasRect, paint, fillStyle, pointsZiggy);

        return new Image(surface);
    }

    private static void FillPolygon(SKCanvas canvas, PixelRect canvasRect, SKPaint paint, FillStyle fillStyle, Pixel[] pixels)
    {
        using SKPath path = new SKPath();
        path.MoveTo(pixels[0].ToSKPoint());

        foreach (Pixel point in pixels.Skip(1))
        {
            path.LineTo(point.ToSKPoint());
        }

        path.Close();

        fillStyle.ApplyToPaint(paint, canvasRect);
        canvas.DrawPath(path, paint);
    }
}
