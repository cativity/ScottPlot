namespace ScottPlot.Hatches;

public class Grid(bool rotate = false) : IHatch
{
    public bool Rotate { get; set; } = rotate;

    static Grid() => _bmp = CreateBitmap();

    private static readonly SKBitmap _bmp;

    private static SKBitmap CreateBitmap()
    {
        SKBitmap bmp = new SKBitmap(20, 20);
        using SKPaint paint = new SKPaint();
        paint.Color = Colors.White.ToSKColor();
        paint.IsStroke = true;
        paint.StrokeWidth = 3;
        //using SKPath? path = new SKPath();
        using SKCanvas canvas = new SKCanvas(bmp);

        canvas.Clear(Colors.Black.ToSKColor());
        canvas.DrawRect(0, 0, 20, 20, paint);

        return bmp;
    }

    public SKShader GetShader(Color backgroundColor, Color hatchColor, PixelRect rect)
    {
        SKMatrix rotationMatrix = Rotate ? SKMatrix.CreateRotationDegrees(45) : SKMatrix.Identity;

        return SKShader.CreateBitmap(_bmp, SKShaderTileMode.Repeat, SKShaderTileMode.Repeat, SKMatrix.CreateScale(0.5f, 0.5f).PostConcat(rotationMatrix))
                       .WithColorFilter(Drawing.GetMaskColorFilter(hatchColor, backgroundColor));
    }
}
