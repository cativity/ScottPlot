﻿namespace ScottPlot;

//[Obsolete("Label has been renamed to LabelStyle", true)]
//public class Label : LabelStyle
//{
//}

public class LabelStyle
{
    public bool IsVisible { get; set; } = true;

    // TODO: deprecate this and pass text into the render method
    public string Text { get; set; } = string.Empty;

    public Alignment Alignment { get; set; } = Alignment.UpperLeft;

    /// <summary>
    ///     Rotation in degrees clockwise from 0 (horizontal text)
    /// </summary>
    public float Rotation { get; set; } = 0;

    public Color ForeColor { get; set; } = Colors.Black;

    //[Obsolete("use BackgroundColor", true)]
    //[EditorBrowsable(EditorBrowsableState.Never)]
    //public Color BackColor { get; set; }

    public Color BackgroundColor { get; set; } = Colors.Transparent;

    public Color BorderColor { get; set; } = Colors.Transparent;

    public float BorderWidth { get; set; } = 1;

    public PixelRect LastRenderPixelRect { get; private set; }

    public Color ShadowColor { get; set; } = Colors.Transparent;

    public PixelOffset ShadowOffset { get; set; } = new PixelOffset(3, 3);

    public bool AntiAliasBackground { get; set; } = true;

    public bool AntiAliasText { get; set; } = true;

    public bool SubpixelText { get; set; } = true;

    public string FontName { get; set; } = Fonts.Default;

    public float FontSize { get; set; } = 12;

    public bool Bold { get; set; }

    /// <summary>
    ///     Manually defined line height in pixels.
    ///     If not defined, the default line spacing will be used (according to the typeface, size, etc.)
    /// </summary>
    public float? LineSpacing { get; set; }

    public bool Italic = false;

    //[Obsolete("use AntiAliasBackground and AntiAliasText", true)]
    //public bool AntiAlias = true;

    public float Padding
    {
        //[Obsolete("Get PixelPadding instead", true)]
        //get => 0;
        set => PixelPadding = new PixelPadding(value);
    }

    // TODO: should add padding and margin
    public PixelPadding PixelPadding { get; set; } = new PixelPadding(0, 0, 0, 0);

    public float PointSize = 0;

    public bool PointFilled { get; set; }

    public Color PointColor = Colors.Magenta;

    public float OffsetX = 0; // TODO: automatic padding support for arbitrary rotations
    public float OffsetY = 0; // TODO: automatic padding support for arbitrary rotations

    public float BorderRadius
    {
        get => BorderRadiusX;
        set
        {
            BorderRadiusX = value;
            BorderRadiusY = value;
        }
    }

    public float BorderRadiusX;
    public float BorderRadiusY;

    /// <summary>
    ///     Use the characters in <see cref="Text" /> to determine an installed
    ///     system font most likely to support this character set.
    /// </summary>
    public void SetBestFont()
    {
        FontName = Fonts.Detect(Text);
    }

    private void ApplyPointPaint(SKPaint paint)
    {
        paint.IsStroke = !PointFilled;
        paint.StrokeWidth = 1;
        paint.Color = PointColor.ToSKColor();
        paint.IsAntialias = AntiAliasBackground;
        paint.Shader = null;
    }

    private void ApplyBorderPaint(SKPaint paint)
    {
        paint.IsStroke = true;
        paint.StrokeWidth = BorderWidth;
        paint.Color = BorderColor.ToSKColor();
        paint.IsAntialias = AntiAliasBackground;
        paint.Shader = null;
    }

    private void ApplyShadowPaint(SKPaint paint)
    {
        paint.IsStroke = false;
        paint.Color = ShadowColor.ToSKColor();
        paint.IsAntialias = AntiAliasBackground;
        paint.Shader = null;
    }

    private void ApplyBackgroundPaint(SKPaint paint)
    {
        paint.IsStroke = false;
        paint.Color = BackgroundColor.ToSKColor();
        paint.IsAntialias = AntiAliasBackground;
        paint.Shader = null;
    }

    private void ApplyTextPaint(SKPaint paint)
    {
        paint.TextAlign = SKTextAlign.Left;
        paint.IsStroke = false;
        paint.Typeface = Fonts.GetTypeface(FontName, Bold, Italic);
        paint.TextSize = FontSize;
        paint.Color = ForeColor.ToSKColor();
        paint.IsAntialias = AntiAliasText;
        paint.SubpixelText = SubpixelText;
        paint.Shader = null;
    }

    public void ApplyToPaint(SKPaint paint)
    {
        ApplyTextPaint(paint);
    }

    /// <summary>
    ///     Return size information for the contents of the <see cref="Text" /> property
    /// </summary>
    public MeasuredText Measure() // NOTE: This should never be called internally
        => Measure(Text);

    /// <summary>
    ///     Return size information for the given text
    /// </summary>
    public MeasuredText Measure(string text) // NOTE: This should never be called internally
    {
        using SKPaint paint = new SKPaint();

        return Measure(text, paint);
    }

    public MeasuredText Measure(string text, SKPaint paint)
    {
        string[] lines = string.IsNullOrEmpty(text) ? [] : text.Split('\n');
        ApplyToPaint(paint);
        float lineHeight = paint.GetFontMetrics(out SKFontMetrics metrics);
        float[] lineWidths = lines.Select(paint.MeasureText).ToArray();
        float maxWidth = lineWidths.Length == 0 ? 0 : lineWidths.Max();
        PixelSize size = new PixelSize(maxWidth, lineHeight * lines.Length);

        // https://github.com/ScottPlot/ScottPlot/issues/3700
        float verticalOffset = metrics.Top + metrics.CapHeight - (metrics.Bottom / 2);

        return new MeasuredText { Size = size, LineHeight = lineHeight, LineWidths = lineWidths, VerticalOffset = verticalOffset, Bottom = metrics.Bottom, };
    }

    /// <summary>
    ///     Use the Label's size and <see cref="Alignment" /> to determine where it should be drawn
    ///     relative to the given rectangle (aligned to the rectangle according to <paramref name="rectAlignment" />).
    /// </summary>
    public Pixel GetRenderLocation(PixelRect rect, Alignment rectAlignment, float offsetX, float offsetY, SKPaint paint)
    {
        PixelSize textSize = Measure(Text, paint).Size;
        float textWidth = textSize.Width;
        float textHeight = textSize.Height;

        if (Alignment != Alignment.UpperLeft)
        {
            throw new NotImplementedException("This method only works for labels with upper-left aligned text");
        }

        float x = rectAlignment switch
        {
            Alignment.UpperLeft => rect.Left + offsetX,
            Alignment.UpperCenter => rect.TopCenter.X - (0.5f * textWidth),
            Alignment.UpperRight => rect.Right - textWidth - offsetX,
            Alignment.MiddleLeft => rect.Left + offsetX,
            Alignment.MiddleCenter => rect.BottomCenter.X - (0.5f * textWidth),
            Alignment.MiddleRight => rect.Right - textWidth - offsetX,
            Alignment.LowerLeft => rect.Left + offsetX,
            Alignment.LowerCenter => rect.BottomCenter.X - (0.5f * textWidth),
            Alignment.LowerRight => rect.Right - textWidth - offsetX,
            _ => throw new NotImplementedException()
        };

        float y = rectAlignment switch
        {
            Alignment.UpperLeft => rect.Top + offsetY,
            Alignment.UpperCenter => rect.Top + offsetY,
            Alignment.UpperRight => rect.Top + offsetY,
            Alignment.MiddleLeft => rect.LeftCenter.Y - (0.5f * textHeight),
            Alignment.MiddleCenter => rect.LeftCenter.Y - (0.5f * textHeight),
            Alignment.MiddleRight => rect.LeftCenter.Y - (0.5f * textHeight),
            Alignment.LowerLeft => rect.Bottom - textHeight - offsetY,
            Alignment.LowerCenter => rect.Bottom - textHeight - offsetY,
            Alignment.LowerRight => rect.Bottom - textHeight - offsetY,
            _ => throw new NotImplementedException()
        };

        return new Pixel(x, y);
    }

    // TODO: deprecate this and require a string to be passed in
    public void Render(SKCanvas canvas, Pixel px, SKPaint paint, bool bottom = true)
    {
        if (!IsVisible)
        {
            return;
        }

        MeasuredText measured = Measure(Text, paint);
        PixelRect textRect = measured.Rect(Alignment);

        CanvasState canvasState = new CanvasState(canvas);
        canvasState.Save();

        canvas.Translate(px.X + OffsetX, px.Y + OffsetY);
        canvas.RotateDegrees(Rotation);

        DrawBackground(canvas, px, paint, textRect);
        DrawText(canvas, measured, paint, textRect, bottom);
        DrawBorder(canvas, px, paint, textRect);
        DrawPoint(canvas, px, paint);

        canvasState.Restore();
    }

    private void DrawBorder(SKCanvas canvas, Pixel px, SKPaint paint, PixelRect textRect)
    {
        if (BorderWidth == 0)
        {
            return;
        }

        PixelRect backgroundRect = textRect.Expand(PixelPadding);
        ApplyBorderPaint(paint);
        canvas.DrawRoundRect(backgroundRect.ToSKRect(), BorderRadiusX, BorderRadiusY, paint);
    }

    private void DrawText(SKCanvas canvas, MeasuredText measured, SKPaint paint, PixelRect textRect, bool bottom)
    {
        ApplyTextPaint(paint);

        float dY = bottom ? -measured.Bottom : measured.VerticalOffset;

        if (Text.Contains('\n'))
        {
            string[] lines = Text.Split('\n');
            float lineHeight = LineSpacing ?? paint.FontSpacing;

            for (int i = 0; i < lines.Length; i++)
            {
                float dX = Alignment.HorizontalFraction() switch
                {
                    0 => 0,
                    0.5f => (textRect.Width / 2) - (measured.LineWidths[i] / 2),
                    1 => textRect.Width - measured.LineWidths[i],
                    _ => throw new NotImplementedException(paint.TextAlign.ToString()),
                };

                float xPx = textRect.Left + dX;
                float yPx = textRect.Top + ((1 + i) * lineHeight) + dY;
                canvas.DrawText(lines[i], xPx, yPx, paint);
            }
        }
        else
        {
            float xPx = textRect.Left;
            float yPx = textRect.Bottom + dY;
            canvas.DrawText(Text, xPx, yPx, paint);
        }
    }

    private void DrawBackground(SKCanvas canvas, Pixel px, SKPaint paint, PixelRect textRect)
    {
        PixelRect backgroundRect = textRect.Expand(PixelPadding);
        PixelRect shadowRect = backgroundRect.WithOffset(ShadowOffset);

        if (ShadowColor != Colors.Transparent)
        {
            ApplyShadowPaint(paint);
            canvas.DrawRoundRect(shadowRect.ToSKRect(), BorderRadiusX, BorderRadiusY, paint);
        }

        if (BackgroundColor != Colors.Transparent)
        {
            ApplyBackgroundPaint(paint);
            canvas.DrawRoundRect(backgroundRect.ToSKRect(), BorderRadiusX, BorderRadiusY, paint);
        }

        // TODO: support rotation
        // https://github.com/ScottPlot/ScottPlot/issues/3519
        LastRenderPixelRect = backgroundRect.Expand(shadowRect).WithDelta(px.X + OffsetX, px.Y + OffsetY);
    }

    private void DrawPoint(SKCanvas canvas, Pixel px, SKPaint paint)
    {
        if (PointSize > 0)
        {
            ApplyPointPaint(paint);
            canvas.DrawCircle(0, 0, PointSize, paint);
        }
    }

    public (string text, float height) MeasureHighestString(string[] strings, SKPaint paint)
    {
        float maxHeight = 0;
        string maxText = string.Empty;

        foreach (string t in strings)
        {
            PixelSize size = Measure(t, paint).Size;

            if (size.Height > maxHeight)
            {
                maxHeight = size.Height;
                maxText = t;
            }
        }

        return (maxText, maxHeight);
    }

    public (string text, PixelLength width) MeasureWidestString(string[] strings, SKPaint paint)
    {
        float maxWidth = 0;
        string maxText = string.Empty;

        foreach (string t in strings)
        {
            PixelSize size = Measure(t, paint).Size;

            if (size.Width > maxWidth)
            {
                maxWidth = size.Width;
                maxText = t;
            }
        }

        return (maxText, maxWidth);
    }
}
