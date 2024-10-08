﻿namespace ScottPlot.Rendering.RenderActions;

public class RenderPlottables : IRenderAction
{
    public void Render(RenderPack rp)
    {
        foreach (IPlottable plottable in rp.Plot.PlottableList.Where(static x => x.IsVisible).ToArray())
        {
            plottable.Axes.DataRect = rp.DataRect;

            rp.CanvasState.Save();

            if (plottable is IPlottableGL plottableGL)
            {
                plottableGL.Render(rp);
            }
            else
            {
                rp.CanvasState.Clip(rp.DataRect);
                plottable.Render(rp);
            }

            rp.CanvasState.DisableClipping();
        }
    }
}
