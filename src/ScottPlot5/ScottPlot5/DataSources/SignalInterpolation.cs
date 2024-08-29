namespace ScottPlot.DataSources;

public static class SignalInterpolation
{
    /// <summary>
    ///     If the point to the left of the graph is extremely far outside the data area,
    ///     modify it using interpolation so it's closer to the data area to prevent render artifacts.
    /// </summary>
    public static void InterpolateBeforeX(RenderPack rp, Pixel[] pixels, ConnectStyle connectStyle)
    {
        if (pixels.Length <= 2)
        {
            return;
        }

        Pixel lastOutsidePoint = pixels[0];
        Pixel firstInsidePoint = pixels[1];

        if (lastOutsidePoint.X == firstInsidePoint.X)
        {
            return;
        }

        float x = rp.DataRect.Left - 1;
        float yDelta = lastOutsidePoint.Y - firstInsidePoint.Y;
        float xDelta1 = x - firstInsidePoint.X;
        float xDelta2 = lastOutsidePoint.X - firstInsidePoint.X;

        float y = connectStyle switch
        {
            ConnectStyle.Straight => firstInsidePoint.Y + (yDelta * xDelta1 / xDelta2),
            ConnectStyle.StepHorizontal => lastOutsidePoint.Y,
            ConnectStyle.StepVertical => firstInsidePoint.Y,
            _ => throw new NotImplementedException(connectStyle.ToString())
        };

        pixels[0] = new Pixel(x, y);
    }

    /// <summary>
    ///     If the point to the bottom of the graph is extremely far outside the data area,
    ///     modify it using interpolation so it's closer to the data area to prevent render artifacts.
    /// </summary>
    public static void InterpolateBeforeY(RenderPack rp, Pixel[] pixels, ConnectStyle connectStyle)
    {
        if (pixels.Length <= 2)
        {
            return;
        }

        Pixel lastOutsidePoint = pixels[0];
        Pixel firstInsidePoint = pixels[1];

        if (lastOutsidePoint.Y == firstInsidePoint.Y)
        {
            return;
        }

        float y = rp.DataRect.Bottom + 1;
        float xDelta = lastOutsidePoint.X - firstInsidePoint.X;
        float yDelta1 = y - firstInsidePoint.Y;
        float yDelta2 = lastOutsidePoint.Y - firstInsidePoint.Y;
        float x = firstInsidePoint.X + (xDelta * yDelta1 / yDelta2);
        pixels[0] = new Pixel(x, y);
    }

    /// <summary>
    ///     If the point to the right of the graph is extremely far outside the data area,
    ///     modify it using interpolation so it's closer to the data area to prevent render artifacts.
    /// </summary>
    public static void InterpolateAfterX(RenderPack rp, Pixel[] pixels, ConnectStyle connectStyle)
    {
        if (pixels.Length <= 2)
        {
            return;
        }

        Pixel lastInsidePoint = pixels[^2];
        Pixel firstOutsidePoint = pixels[^1];

        if (firstOutsidePoint.X == lastInsidePoint.X)
        {
            return;
        }

        float x = rp.DataRect.Right + 1;
        float yDelta = firstOutsidePoint.Y - lastInsidePoint.Y;
        float xDelta1 = x - lastInsidePoint.X;
        float xDelta2 = firstOutsidePoint.X - lastInsidePoint.X;

        float y = connectStyle switch
        {
            ConnectStyle.Straight => lastInsidePoint.Y + (yDelta * xDelta1 / xDelta2),
            ConnectStyle.StepHorizontal => lastInsidePoint.Y,
            ConnectStyle.StepVertical => firstOutsidePoint.Y,
            _ => throw new NotImplementedException(connectStyle.ToString())
        };

        pixels[^1] = new Pixel(x, y);
    }

    /// <summary>
    ///     If the point to the top of the graph is extremely far outside the data area,
    ///     modify it using interpolation so it's closer to the data area to prevent render artifacts.
    /// </summary>
    public static void InterpolateAfterY(RenderPack rp, Pixel[] pixels, ConnectStyle connectStyle)
    {
        if (pixels.Length <= 2)
        {
            return;
        }

        Pixel lastInsidePoint = pixels[^2];
        Pixel firstOutsidePoint = pixels[^1];

        if (firstOutsidePoint.Y == lastInsidePoint.Y)
        {
            return;
        }

        float y = rp.DataRect.Top - 1;
        float xDelta = firstOutsidePoint.X - lastInsidePoint.X;
        float yDelta1 = y - lastInsidePoint.Y;
        float yDelta2 = firstOutsidePoint.Y - lastInsidePoint.Y;
        float x = lastInsidePoint.X + (xDelta * yDelta1 / yDelta2);
        pixels[^1] = new Pixel(x, y);
    }
}
