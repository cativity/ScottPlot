namespace ScottPlot.Interactivity;

/// <summary>
///     This class stores logic for changing axis limits according to mouse inputs in pixel units.
///     Methods here are similar to those in <see cref="Plot.Axes" /> except their inputs are all mouse events.
/// </summary>
public static class MouseAxisManipulation
{
    public static void MouseWheelZoom(Plot plot, double fracX, double fracY, Pixel pixel, bool changeOpposingAxesTogether)
    {
        pixel = pixel.Divide(plot.ScaleFactorF);
        MultiAxisLimits originalLimits = new MultiAxisLimits(plot);
        PixelRect dataRect = plot.RenderManager.LastRender.DataRect;

        // restore MouseDown limits
        originalLimits.Recall();

        if (plot.GetAxis(pixel) is IAxis axisUnderMouse)
        {
            if (changeOpposingAxesTogether && axisUnderMouse.IsHorizontal())
            {
                plot.Axes.XAxes.ForEach(xAxis => xAxis.Range.ZoomFrac(fracX, xAxis.GetCoordinate(pixel.X, dataRect)));
            }

            if (changeOpposingAxesTogether && axisUnderMouse.IsVertical())
            {
                plot.Axes.YAxes.ForEach(yAxis => yAxis.Range.ZoomFrac(fracY, yAxis.GetCoordinate(pixel.Y, dataRect)));
            }
            else
            {
                double frac = axisUnderMouse.IsHorizontal() ? fracX : fracY;
                float scaledCoord = axisUnderMouse.IsHorizontal() ? pixel.X : pixel.Y;
                axisUnderMouse.Range.ZoomFrac(frac, axisUnderMouse.GetCoordinate(scaledCoord, dataRect));
            }
        }
        else
        {
            // modify all axes
            plot.Axes.XAxes.ForEach(xAxis => xAxis.Range.ZoomFrac(fracX, xAxis.GetCoordinate(pixel.X, dataRect)));
            plot.Axes.YAxes.ForEach(yAxis => yAxis.Range.ZoomFrac(fracY, yAxis.GetCoordinate(pixel.Y, dataRect)));
        }
    }

    public static void DragPan(Plot plot, Pixel mouseDown, Pixel mouseNow)
    {
        mouseDown = mouseDown.Divide(plot.ScaleFactorF);
        mouseNow = mouseNow.Divide(plot.ScaleFactorF);
        IPlotControl control = plot.PlotControl ?? throw new NullReferenceException();

        float pixelDeltaX = -(mouseNow.X - mouseDown.X);
        float pixelDeltaY = mouseNow.Y - mouseDown.Y;

        float scaledDeltaX = pixelDeltaX / plot.ScaleFactorF;
        float scaledDeltaY = pixelDeltaY / plot.ScaleFactorF;

        PixelRect dataRect = plot.RenderManager.LastRender.DataRect;

        if (plot.GetAxis(mouseDown) is IAxis axisUnderMouse)
        {
            if (control.Interaction.ChangeOpposingAxesTogether && axisUnderMouse.IsHorizontal())
            {
                plot.Axes.XAxes.ForEach(xAxis => xAxis.Range.PanMouse(scaledDeltaX, dataRect.Width));
            }
            else if (control.Interaction.ChangeOpposingAxesTogether && axisUnderMouse.IsVertical())
            {
                plot.Axes.YAxes.ForEach(yAxis => yAxis.Range.PanMouse(scaledDeltaY, dataRect.Height));
            }
            else
            {
                float scaledDelta = axisUnderMouse.IsHorizontal() ? scaledDeltaX : scaledDeltaY;
                float dataSize = axisUnderMouse.IsHorizontal() ? dataRect.Width : dataRect.Height;
                axisUnderMouse.Range.PanMouse(scaledDelta, dataSize);
            }
        }
        else
        {
            // modify all axes
            plot.Axes.XAxes.ForEach(xAxis => xAxis.Range.PanMouse(scaledDeltaX, dataRect.Width));
            plot.Axes.YAxes.ForEach(yAxis => yAxis.Range.PanMouse(scaledDeltaY, dataRect.Height));
        }
    }

    public static void DragZoom(Plot plot, Pixel mouseDown, Pixel mouseNow)
    {
        mouseDown = mouseDown.Divide(plot.ScaleFactorF);
        mouseNow = mouseNow.Divide(plot.ScaleFactorF);
        IPlotControl control = plot.PlotControl ?? throw new NullReferenceException();

        float pixelDeltaX = mouseNow.X - mouseDown.X;
        float pixelDeltaY = -(mouseNow.Y - mouseDown.Y);

        PixelRect lastRenderDataRect = plot.RenderManager.LastRender.DataRect;

        if (plot.GetAxis(mouseDown) is IAxis axisUnderMouse)
        {
            if (control.Interaction.ChangeOpposingAxesTogether && axisUnderMouse.IsHorizontal())
            {
                plot.Axes.XAxes.ForEach(xAxis => xAxis.Range.ZoomMouseDelta(pixelDeltaX, lastRenderDataRect.Width));
            }
            else if (control.Interaction.ChangeOpposingAxesTogether && axisUnderMouse.IsVertical())
            {
                plot.Axes.YAxes.ForEach(yAxis => yAxis.Range.ZoomMouseDelta(pixelDeltaY, lastRenderDataRect.Height));
            }
            else
            {
                // modify a single axis
                float pixelDelta = axisUnderMouse.IsHorizontal() ? pixelDeltaX : pixelDeltaY;
                float dataSize = axisUnderMouse.IsHorizontal() ? lastRenderDataRect.Width : lastRenderDataRect.Height;
                axisUnderMouse.Range.ZoomMouseDelta(pixelDelta, dataSize);
            }
        }
        else
        {
            // modify all axes
            plot.Axes.XAxes.ForEach(xAxis => xAxis.Range.ZoomMouseDelta(pixelDeltaX, lastRenderDataRect.Width));
            plot.Axes.YAxes.ForEach(yAxis => yAxis.Range.ZoomMouseDelta(pixelDeltaY, lastRenderDataRect.Height));
        }
    }

    public static void PlaceZoomRectangle(Plot plot, Pixel mouseDown, Pixel mouseNow)
    {
        mouseDown = mouseDown.Divide(plot.ScaleFactorF);
        mouseNow = mouseNow.Divide(plot.ScaleFactorF);

        if (plot.GetAxis(mouseDown) is IAxis axisUnderMouse)
        {
            // Do not respond if the axis under the mouse has no data
            // https://github.com/ScottPlot/ScottPlot/issues/3810
            IEnumerable<IAxis> xAxes = plot.GetPlottables().Select(static x => x.Axes.XAxis).Cast<IAxis>();
            IEnumerable<IAxis> yAxes = plot.GetPlottables().Select(static x => x.Axes.YAxis).Cast<IAxis>();
            bool axesIsUsedByPlottables = xAxes.Contains(axisUnderMouse) || yAxes.Contains(axisUnderMouse);

            if (!axesIsUsedByPlottables)
            {
                return;
            }

            // NOTE: this function only changes shape of the rectangle.
            // It doesn't modify axis limits, so no action is required on the opposite axis.
            plot.ZoomRectangle.VerticalSpan = axisUnderMouse.IsHorizontal();
            plot.ZoomRectangle.HorizontalSpan = axisUnderMouse.IsVertical();
        }

        plot.ZoomRectangle.MouseDown = mouseDown;
        plot.ZoomRectangle.MouseUp = mouseNow;
    }

    public static void AutoScale(Plot plot, Pixel pixel, bool allParallelAxes)
    {
        pixel = pixel.Divide(plot.ScaleFactorF);
        IAxis? axisUnderMouse = plot.GetAxis(pixel);

        if (axisUnderMouse is null)
        {
            plot.Axes.AutoScale();

            return;
        }

        if (allParallelAxes)
        {
            if (axisUnderMouse.IsHorizontal())
            {
                plot.Axes.XAxes.ForEach(plot.Axes.AutoScaleX);
            }
            else if (axisUnderMouse.IsVertical())
            {
                plot.Axes.YAxes.ForEach(plot.Axes.AutoScaleY);
            }
        }
        else
        {
            if (axisUnderMouse is IYAxis yAxisUnderMouse)
            {
                plot.Axes.AutoScaleY(yAxisUnderMouse);
            }
            else if (axisUnderMouse is IXAxis xAxisUnderMouse)
            {
                plot.Axes.AutoScaleX(xAxisUnderMouse);
            }
        }
    }
}
