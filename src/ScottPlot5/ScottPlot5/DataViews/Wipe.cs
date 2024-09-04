using ScottPlot.Plottables;

namespace ScottPlot.DataViews;

public class Wipe(DataStreamer streamer, bool wipeRight) : IDataStreamerView
{
    public DataStreamer Streamer { get; } = streamer;

    public double BlankFraction { get; set; }

    public void Render(RenderPack rp)
    {
        int newestCount = Streamer.Data.NextIndex;
        int oldestCount = Streamer.Data.Data.Length - newestCount;

        double xMax = (Streamer.Data.Data.Length * Streamer.Data.SamplePeriod) + Streamer.Data.OffsetX;

        Pixel[] newest = new Pixel[newestCount];
        Pixel[] oldest = new Pixel[oldestCount];

        for (int i = 0; i < newest.Length; i++)
        {
            double xPos = (i * Streamer.Data.SamplePeriod) + Streamer.Data.OffsetX;
            float x = Streamer.Axes.GetPixelX(wipeRight ? xPos : xMax - xPos);
            float y = Streamer.Axes.GetPixelY(Streamer.Data.Data[i] + Streamer.Data.OffsetY);
            newest[i] = new Pixel(x, y);
        }

        for (int i = 0; i < oldest.Length; i++)
        {
            double xPos = ((i + newestCount) * Streamer.Data.SamplePeriod) + Streamer.Data.OffsetX;
            float x = Streamer.Axes.GetPixelX(wipeRight ? xPos : xMax - xPos);
            float y = Streamer.Axes.GetPixelY(Streamer.Data.Data[i + newestCount] + Streamer.Data.OffsetY);
            oldest[i] = new Pixel(x, y);
        }

        if (BlankFraction > 0)
        {
            int blankPoints = (int)(BlankFraction * Streamer.Data.Length);

            if (blankPoints <= oldest.Length)
            {
                oldest = oldest.Skip(blankPoints).ToArray();
            }
            else
            {
                oldest = [];
                newest = newest.Skip(blankPoints - oldest.Length).ToArray();
            }
        }

        using SKPaint paint = new SKPaint();
        Drawing.DrawLines(rp.Canvas, paint, oldest, Streamer.LineStyle);
        Drawing.DrawLines(rp.Canvas, paint, newest, Streamer.LineStyle);
    }
}
