namespace ScottPlot.Hatches;

public class Dots : IHatch
{
    static Dots() => _bmp = CreateBitmap();

    private static readonly SKBitmap _bmp;

    private static SKBitmap CreateBitmap()
    {
        SKBitmap bmp = new SKBitmap(20, 20);
        using SKPaint paint = new SKPaint();
        paint.Color = Colors.White.ToSKColor();
        //using SKPath? path = new SKPath();
        using SKCanvas canvas = new SKCanvas(bmp);

        paint.IsAntialias = true; // AA is especially important for circles, it seems to do little for the other shapes

        canvas.Clear(Colors.Black.ToSKColor());
        canvas.DrawCircle(5, 5, 5, paint);

        return bmp;
    }

    public SKShader GetShader(Color backgroundColor, Color hatchColor, PixelRect rect)
        => SKShader.CreateBitmap(_bmp, SKShaderTileMode.Repeat, SKShaderTileMode.Repeat, SKMatrix.CreateScale(0.5f, 0.5f))
                   .WithColorFilter(Drawing.GetMaskColorFilter(hatchColor, backgroundColor));
}
