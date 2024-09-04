using ScottPlot.Plottables;
using SkiaSharp;

namespace ScottPlotTests.RenderTests;

internal class LabelTests
{
    [Test]
    public void TestLabelAlignment()
    {
        SKBitmap bmp = new SKBitmap(500, 500);
        using SKCanvas canvas = new SKCanvas(bmp);
        canvas.Clear(SKColors.Navy);

        //PixelSize size = new PixelSize(40, 20);
        using SKPaint paint = new SKPaint();

        int y = 20;

        foreach (Alignment alignment in Enum.GetValues(typeof(Alignment)))
        {
            Pixel pixel = new Pixel(250, 20 + y);

            LabelStyle lbl = new LabelStyle
            {
                Text = alignment.ToString(),
                Alignment = alignment,
                FontSize = 32,
                ForeColor = Colors.White.WithOpacity(.5),
                BorderColor = Colors.Yellow,
                BorderWidth = 1,
                PointSize = 5,
                PointColor = Colors.White,
            };

            lbl.Render(canvas, pixel, paint);

            y += 50;
        }

        bmp.SaveTestImage();
    }

    [Test]
    public void TestLabelRotation()
    {
        SKBitmap bmp = new SKBitmap(500, 500);
        using SKCanvas canvas = new SKCanvas(bmp);
        canvas.Clear(SKColors.Navy);

        _ = new PixelSize(40, 20);
        //PixelSize size = new PixelSize(40, 20);
        using SKPaint paint = new SKPaint();
        const float radius = 100;

        for (int i = 0; i < 360; i += 45)
        {
            float x = (float)Math.Cos(i * Math.PI / 180) * radius;
            float y = (float)Math.Sin(i * Math.PI / 180) * radius;
            Pixel center = new Pixel((bmp.Width / 2D) + x, (bmp.Height / 2D) + y);

            LabelStyle lbl = new LabelStyle
            {
                Text = $"R{i}",
                FontSize = 32,
                ForeColor = Colors.White.WithOpacity(.5),
                Rotation = i,
                PointSize = 5,
                BorderColor = Colors.Yellow,
                PointColor = Colors.White,
                BorderWidth = 1,
            };

            lbl.Render(canvas, center, paint);
        }

        bmp.SaveTestImage();
    }

    [Test]
    public void TestLabelAlignmentWithRotation()
    {
        SKBitmap bmp = new SKBitmap(500, 500);
        using SKCanvas canvas = new SKCanvas(bmp);
        canvas.Clear(SKColors.Navy);

        _ = new PixelSize(40, 20);
        //PixelSize size = new PixelSize(40, 20);
        using SKPaint paint = new SKPaint();

        int y = 20;

        foreach (Alignment alignment in Enum.GetValues(typeof(Alignment)))
        {
            Pixel pixel = new Pixel(250, 20 + y);

            LabelStyle lbl = new LabelStyle
            {
                Text = alignment.ToString(),
                Alignment = alignment,
                FontSize = 32,
                ForeColor = Colors.White.WithOpacity(.5),
                BorderColor = Colors.Yellow,
                BorderWidth = 1,
                PointSize = 5,
                PointColor = Colors.White,
                Rotation = 30,
            };

            lbl.Render(canvas, pixel, paint);

            y += 50;
        }

        bmp.SaveTestImage();
    }

    [Test]
    public void TestMultilineLabelAlignmentWithRotation()
    {
        SKBitmap bmp = new SKBitmap(600, 600);
        using SKCanvas canvas = new SKCanvas(bmp);
        canvas.Clear(SKColors.Navy);

        //PixelSize size = new PixelSize(40, 20);
        using SKPaint paint = new SKPaint();

        Alignment[,] alignmentMatrix = AlignmentExtensions.AlignmentMatrix;

        for (int y = 0; y < alignmentMatrix.GetLength(0); y++)
        {
            for (int x = 0; x < alignmentMatrix.GetLength(1); x++)
            {
                Alignment alignment = alignmentMatrix[y, x];

                Pixel pixel = new Pixel(100 + (x * 200), 100 + (y * 200));

                LabelStyle label = new LabelStyle
                {
                    Text = alignment.ToString().Replace("Upper", "Upper\n").Replace("Middle", "Middle\n").Replace("Lower", "Lower\n"),
                    Alignment = alignment,
                    FontSize = 32,
                    ForeColor = Colors.White.WithOpacity(.5),
                    BackgroundColor = Colors.White.WithAlpha(.1),
                    BorderColor = Colors.Yellow,
                    BorderWidth = 1,
                    PointSize = 5,
                    PointColor = Colors.White,
                    Rotation = 45,
                };

                label.Render(canvas, pixel, paint);
            }
        }

        bmp.SaveTestImage();
    }

    [Test]
    public void TestStringMeasurement()
    {
        SKBitmap bmp = new SKBitmap(500, 500);
        using SKCanvas canvas = new SKCanvas(bmp);
        canvas.Clear(SKColors.Navy);

        using SKPaint paint = new SKPaint();
        string[] fonts = ["Times New Roman", "Consolas", "Impact", "Arial Narrow", "MiSsInG fOnT"];

        float yOffset = 20;

        foreach (string font in fonts)
        {
            LabelStyle lbl = new LabelStyle
            {
                Text = "Hello, World",
                FontName = font,
                FontSize = 64,
                ForeColor = Colors.White,
                BorderColor = Colors.Yellow,
                BorderWidth = 1,
            };

            Pixel px = new Pixel(20, yOffset);
            lbl.Render(canvas, px, paint);

            yOffset += 100;
        }

        bmp.SaveTestImage();
        Assert.Pass();
    }

    [Test]
    public void TestLabelMultiline()
    {
        SKSurface surface = Drawing.CreateSurface(400, 300);
        SKCanvas canvas = surface.Canvas;
        canvas.Clear(SKColors.Navy);
        using SKPaint paint = new SKPaint();

        LabelStyle lbl = new LabelStyle
        {
            Text = "One\nTwo",
            ForeColor = Colors.White.WithAlpha(.5),
            FontSize = 22,
            PointSize = 5,
            PointColor = Colors.White,
            BorderWidth = 1,
            BorderColor = Colors.Yellow,
        };

        lbl.Render(canvas, new Pixel(200, 100), paint);

        lbl.Rotation = 45;

        lbl.Render(canvas, new Pixel(200, 200), paint);

        surface.SaveTestImage();
    }

    [Test]
    public void TestLabelRounded()
    {
        SKBitmap bmp = new SKBitmap(200, 150);
        using SKCanvas canvas = new SKCanvas(bmp);
        canvas.Clear(SKColors.Navy);
        using SKPaint paint = new SKPaint();

        LabelStyle lbl = new LabelStyle
        {
            Text = "Hello",
            FontSize = 32,
            ForeColor = Colors.White.WithOpacity(.5),
            BorderColor = Colors.Yellow.WithAlpha(.5),
            BackgroundColor = Colors.White.WithAlpha(.2),
            BorderWidth = 2,
            Padding = 10,
            BorderRadius = 15,
        };

        lbl.Render(canvas, new Pixel(50, 50), paint);

        bmp.SaveTestImage();
    }

    [Test]
    public void TestLabelAntiAlias()
    {
        SKBitmap bmp = new SKBitmap(200, 200);
        using SKCanvas canvas = new SKCanvas(bmp);
        canvas.Clear(SKColors.White);
        using SKPaint paint = new SKPaint();

        LabelStyle lbl1 = new LabelStyle
        {
            Text = "Default",
            BorderColor = Colors.Black,
            BorderWidth = 1,
            Padding = 3,
            ShadowColor = Colors.Black.WithAlpha(.5),
            ShadowOffset = new PixelOffset(5, 5),
            BackgroundColor = Colors.White,
        };

        LabelStyle lbl2 = new LabelStyle
        {
            Text = "AntiAliasBackground = false",
            BorderColor = Colors.Black,
            BorderWidth = 1,
            Padding = 3,
            ShadowColor = Colors.Black.WithAlpha(.5),
            ShadowOffset = new PixelOffset(5, 5),
            BackgroundColor = Colors.White,
            AntiAliasBackground = false,
        };

        LabelStyle lbl3 = new LabelStyle
        {
            Text = "AntiAliasText = false",
            BorderColor = Colors.Black,
            BorderWidth = 1,
            Padding = 3,
            ShadowColor = Colors.Black.WithAlpha(.5),
            ShadowOffset = new PixelOffset(5, 5),
            BackgroundColor = Colors.White,
            AntiAliasText = false,
        };

        lbl1.Render(canvas, new Pixel(25, 50), paint);
        lbl2.Render(canvas, new Pixel(25, 100), paint);
        lbl3.Render(canvas, new Pixel(25, 150), paint);

        bmp.SaveTestImage();
    }

    [Test]
    public void TestLabelOffset()
    {
        SKBitmap bmp = new SKBitmap(500, 500);
        TestLabelOffset(bmp, "X:{0}, Y:{1}");
        bmp.SaveTestImage();
    }

    [Test]
    public void TestLabelMultiLineOffset()
    {
        SKBitmap bmp = new SKBitmap(500, 500);
        TestLabelOffset(bmp, "X:{0}\nY:{1}");
        bmp.SaveTestImage();
    }

    private static void TestLabelOffset(SKBitmap bmp, string format)
    {
        using SKCanvas canvas = new SKCanvas(bmp);
        canvas.Clear(SKColors.Navy);

        using SKPaint paint = new SKPaint();

        Pixel center = new Pixel(bmp.Width / 2D, bmp.Height / 2D);
        const float offset = 150f;

        for (int y = -1; y < 2; y++)
        {
            for (int x = -1; x < 2; x++)
            {
                float offsetX = offset * x;
                float offsetY = offset * y;

                LabelStyle lbl = new LabelStyle
                {
                    Text = string.Format(format, offsetX, offsetY),
                    Alignment = Alignment.MiddleCenter,
                    FontSize = 24,
                    ForeColor = Colors.White.WithOpacity(.5),
                    PointSize = 5,
                    BorderColor = Colors.Yellow,
                    PointColor = Colors.White,
                    BorderWidth = 1,
                    OffsetX = offsetX,
                    OffsetY = offsetY,
                };

                lbl.Render(canvas, center, paint);
            }
        }
    }

    [Test]
    public static void TestLabelMultilineAlignment()
    {
        Plot plot = new Plot();

        Text txt1 = plot.Add.Text("aaa\nbbbbbbbbbbb\nccc", 0, 0);
        txt1.Alignment = Alignment.MiddleLeft;
        txt1.LabelBackgroundColor = Colors.SkyBlue;

        Text txt2 = plot.Add.Text("aaa\nbbbbbbbbbbb\nccc", 1, 1);
        txt2.Alignment = Alignment.MiddleCenter;
        txt2.LabelBackgroundColor = Colors.SkyBlue;

        Text txt3 = plot.Add.Text("aaa\nbbbbbbbbbbb\nccc", 2, 2);
        txt3.Alignment = Alignment.MiddleRight;
        txt3.LabelBackgroundColor = Colors.SkyBlue;

        plot.Axes.SetLimits(-1, 3, -1, 3);

        plot.SaveTestImage();
    }
}
