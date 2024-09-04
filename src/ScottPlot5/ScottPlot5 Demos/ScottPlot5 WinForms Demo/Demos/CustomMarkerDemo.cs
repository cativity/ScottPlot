using JetBrains.Annotations;
using ScottPlot;
using ScottPlot.Plottables;
using SkiaSharp;

namespace WinForms_Demo.Demos;

[UsedImplicitly]
public partial class CustomMarkerDemo : Form, IDemoWindow
{
    public string Title => "Custom Marker Shapes";

    public string Description => "Demonstrates how to create plots using custom markers";

    private readonly CustomMarker _myCustomMarker = new CustomMarker();

    public CustomMarkerDemo()
    {
        InitializeComponent();

        double[] xs = Generate.Consecutive(30);
        double[] ys = Generate.Sin(30);

        Scatter sp = formsPlot1.Plot.Add.Scatter(xs, ys);
        sp.MarkerStyle.CustomRenderer = _myCustomMarker;
        sp.MarkerStyle.FillColor = Colors.Yellow;
        sp.MarkerStyle.LineColor = Colors.Black;
        sp.MarkerSize = 20;
        sp.LineWidth = 5;

        trackBar1.ValueChanged += (_, _) =>
        {
            _myCustomMarker.Happiness = trackBar1.Value / 50.0;
            formsPlot1.Refresh();
        };

        trackBar1.Value = 25;
    }

    private class CustomMarker : IMarker
    {
        public double Happiness = 1.0;

        public void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, MarkerStyle markerStyle)
        {
            float faceRadius = size / 2;
            float eyeRadius = faceRadius * 0.1f;
            float centerX = center.X;
            float centerY = center.Y;

            // face
            Drawing.DrawCircle(canvas, center, faceRadius, markerStyle.FillStyle, paint);
            Drawing.DrawCircle(canvas, center, faceRadius, markerStyle.LineStyle, paint);

            // left eye
            float leftEyeX = centerX - (faceRadius / 3);
            float leftEyeY = centerY - (faceRadius / 3);
            Drawing.DrawCircle(canvas, new Pixel(leftEyeX, leftEyeY), eyeRadius, markerStyle.LineStyle, paint);

            // right eye
            float rightEyeX = centerX + (faceRadius / 3);
            //float rightEyeY = leftEyeY;
            Drawing.DrawCircle(canvas, new Pixel(rightEyeX, leftEyeY), eyeRadius, markerStyle.LineStyle, paint);

            // mouth
            float smileHeight = faceRadius * (float)Happiness;
            float smileY = centerY + (faceRadius * .2f);
            float smileTipY = smileY + smileHeight;
            float smileTop = Math.Min(smileY, smileTipY);
            float smileBottom = Math.Max(smileY, smileTipY);

            if (Happiness > 0)
            {
                SKRect oval = new SKRect(leftEyeX, smileTop - (smileHeight / 2), rightEyeX, smileBottom - (smileHeight / 2));
                canvas.DrawArc(oval, 180, -180, false, paint);
            }
            else if (Happiness < 0)
            {
                SKRect oval = new SKRect(leftEyeX, smileTop - smileHeight, rightEyeX, smileBottom - smileHeight);
                canvas.DrawArc(oval, -180, 180, false, paint);
            }
        }
    }
}
