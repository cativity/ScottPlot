namespace ScottPlot.Hatches;

public enum StripeDirection
{
    DiagonalUp,
    DiagonalDown,
    Horizontal,
    Vertical
}

public class Striped(StripeDirection stripeDirection = StripeDirection.DiagonalUp) : IHatch
{
    // This is implemented as a transformation of the shader, so we don't need to invalidate the bitmap in the setter
    public StripeDirection StripeDirection { get; set; } = stripeDirection;

    static Striped() => _bmp = CreateBitmap();

    private static readonly SKBitmap _bmp;

    private static SKBitmap CreateBitmap()
    {
        SKBitmap bitmap = new SKBitmap(20, 50);

        using SKPaint paint = new SKPaint();
        paint.Color = Colors.White.ToSKColor();
        //using SKPath? path = new SKPath();
        using SKCanvas canvas = new SKCanvas(bitmap);

        canvas.Clear(Colors.Black.ToSKColor());
        canvas.DrawRect(new SKRect(0, 0, 20, 20), paint);

        return bitmap;
    }

    public SKShader GetShader(Color backgroundColor, Color hatchColor, PixelRect rect)
    {
        SKMatrix rotationMatrix = StripeDirection switch
        {
            StripeDirection.DiagonalUp => SKMatrix.CreateRotationDegrees(-45),
            StripeDirection.DiagonalDown => SKMatrix.CreateRotationDegrees(45),
            StripeDirection.Horizontal => SKMatrix.Identity,
            StripeDirection.Vertical => SKMatrix.CreateRotationDegrees(90),
            _ => throw new NotImplementedException(nameof(StripeDirection))
        };

        return SKShader.CreateBitmap(_bmp, SKShaderTileMode.Repeat, SKShaderTileMode.Repeat, SKMatrix.CreateScale(0.1f, 0.15f).PostConcat(rotationMatrix))
                       .WithColorFilter(Drawing.GetMaskColorFilter(hatchColor, backgroundColor));
    }
}
