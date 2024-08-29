using ScottPlot.Plottables;

namespace ScottPlot.DataViews;

public class Scroll(DataStreamer streamer, bool newOnRight) : IDataStreamerView
{
    public DataStreamer Streamer { get; } = streamer;

    public void Render(RenderPack rp)
    {
        int dataLength = Streamer.Data.Length;
        int dataNextIndex = Streamer.Data.NextIndex;
        int oldPointCount = dataLength - dataNextIndex;

        Pixel[] points = new Pixel[dataLength];

        for (int i = 0; i < dataLength; i++)
        {
            bool isNewPoint = i < oldPointCount;
            int sourceIndex = isNewPoint ? dataNextIndex + i : i - oldPointCount;
            int targetIndex = newOnRight ? i : dataLength - 1 - i;

            points[targetIndex] = new Pixel(Streamer.Axes.GetPixelX((targetIndex * Streamer.Data.SamplePeriod) + Streamer.Data.OffsetX),
                                            Streamer.Axes.GetPixelY(Streamer.Data.Data[sourceIndex] + Streamer.Data.OffsetY));
        }

        using SKPaint paint = new SKPaint();
        Drawing.DrawLines(rp.Canvas, paint, points, Streamer.LineStyle);
    }
}
