using JetBrains.Annotations;
using ScottPlot;
using ScottPlot.WinForms;
using SkiaSharp;
using Image = System.Drawing.Image;

namespace WinForms_Demo.Demos;

[UsedImplicitly]
public partial class LabelDemo : Form, IDemoWindow
{
    public string Title => "Font Styling";

    public string Description
        => "A tool to facilitate evaluating different fonts and the customization options for size, alignment, line height, and more.";

    public LabelDemo()
    {
        InitializeComponent();

        SizeChanged += (_, _) => RegenerateImage();
        tbSize.ValueChanged += (_, _) => RegenerateImage();
        tbAlignment.ValueChanged += (_, _) => RegenerateImage();
        tbPadding.ValueChanged += (_, _) => RegenerateImage();
        tbRotation.ValueChanged += (_, _) => RegenerateImage();
        Load += (_, _) => RegenerateImage();
    }

    public void RegenerateImage()
    {
        SKImageInfo info = new SKImageInfo(pictureBox1.Width, pictureBox1.Height);
        SKSurface surface = SKSurface.Create(info);
        SKCanvas canvas = surface.Canvas;
        canvas.Clear(SKColors.White);

        LabelStyle label = new LabelStyle
        {
            ForeColor = Colors.Black,
            BorderColor = Colors.Gray,
            BorderWidth = 1,
            Text = "Testing",
            FontSize = tbSize.Value,
            Alignment = (Alignment)tbAlignment.Value,
            Rotation = tbRotation.Value,
            Padding = tbPadding.Value,
        };

        PixelSize size = new PixelSize(pictureBox1.Width, pictureBox1.Height);
        PixelRect rect = new PixelRect(size);

        LineStyle ls = new LineStyle { Width = 1, Color = Colors.Magenta, };

        using SKPaint paint = new SKPaint();
        Drawing.DrawCircle(canvas, rect.Center, 5, ls, paint);
        label.Render(canvas, rect.Center, paint);

        ScottPlot.Image img2 = new ScottPlot.Image(surface);
        Image? oldImage = pictureBox1.Image;
        pictureBox1.Image = img2.GetBitmap();
        oldImage?.Dispose();
    }
}
