namespace ScottPlot;

public class BackgroundStyle : IDisposable
{
    public Color Color { get; set; } = Colors.White;

    private SKBitmap? SKBitmap { get; set; }

    public ImagePosition ImagePosition { get; set; } = ImagePosition.Stretch;

    public bool AntiAlias { get; set; } = true;

    private Image? _image;

    public Image? Image
    {
        get => _image;
        set
        {
            _image = value;

            if (value is not null)
            {
                byte[] bytes = value.GetImageBytes();
                SKBitmap = SKBitmap.Decode(bytes); // TODO: SKImage instead?
            }
        }
    }

    public void Dispose()
    {
        SKBitmap?.Dispose();
        GC.SuppressFinalize(this);
    }

    public PixelRect GetImageRect(PixelRect targetRect) => Image is null ? PixelRect.Zero : ImagePosition.GetRect(Image.Size, targetRect);

    public void Render(SKCanvas canvas, PixelRect target)
    {
        using SKPaint paint = new SKPaint();
        paint.Color = Color.ToSKColor();
        Drawing.FillRectangle(canvas, target, paint);

        if (Image is null)
        {
            return;
        }

        PixelRect imgRect = ImagePosition.GetRect(Image.Size, target);
        Image.Render(canvas, imgRect, paint, AntiAlias);
    }
}
