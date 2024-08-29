namespace ScottPlot.Control;

/// <summary>
///     This object holds actions which manipulate the plot.
///     To customize plot manipulation behavior, replace these delegates with custom ones.
/// </summary>
public class PlotActions
{
    public Action<IPlotControl, Pixel, LockedAxes> ZoomIn = static delegate { };
    public Action<IPlotControl, Pixel, LockedAxes> ZoomOut = static delegate { };
    public Action<IPlotControl> PanUp = static delegate { };
    public Action<IPlotControl> PanDown = static delegate { };
    public Action<IPlotControl> PanLeft = static delegate { };
    public Action<IPlotControl> PanRight = static delegate { };
    public Action<IPlotControl, MouseDrag, LockedAxes> DragPan = static delegate { };
    public Action<IPlotControl, MouseDrag, LockedAxes> DragZoom = static delegate { };
    public Action<IPlotControl, MouseDrag, LockedAxes> DragZoomRectangle = static delegate { };
    public Action<IPlotControl> ZoomRectangleClear = static delegate { };
    public Action<IPlotControl> ZoomRectangleApply = static delegate { };
    public Action<IPlotControl> ToggleBenchmark = static delegate { };
    public Action<IPlotControl, Pixel> AutoScale = static delegate { };
    public Action<IPlotControl, Pixel> ShowContextMenu = static delegate { };

    public static PlotActions Standard()
        => new PlotActions
        {
            ZoomIn = StandardActions.ZoomIn,
            ZoomOut = StandardActions.ZoomOut,
            PanUp = StandardActions.PanUp,
            PanDown = StandardActions.PanDown,
            PanLeft = StandardActions.PanLeft,
            PanRight = StandardActions.PanRight,
            DragPan = StandardActions.DragPan,
            DragZoom = StandardActions.DragZoom,
            DragZoomRectangle = StandardActions.DragZoomRectangle,
            ZoomRectangleClear = StandardActions.ZoomRectangleClear,
            ZoomRectangleApply = StandardActions.ZoomRectangleApply,
            ToggleBenchmark = StandardActions.ToggleBenchmark,
            AutoScale = StandardActions.AutoScale,
            ShowContextMenu = StandardActions.ShowContextMenu,
        };

    public static PlotActions NonInteractive()
    {
        return new PlotActions
        {
            ZoomIn = static delegate { },
            ZoomOut = static delegate { },
            PanUp = static delegate { },
            PanDown = static delegate { },
            PanLeft = static delegate { },
            PanRight = static delegate { },
            DragPan = static delegate { },
            DragZoom = static delegate { },
            DragZoomRectangle = static delegate { },
            ZoomRectangleClear = static delegate { },
            ZoomRectangleApply = static delegate { },
            ToggleBenchmark = static delegate { },
            AutoScale = static delegate { },
            ShowContextMenu = static delegate { },
        };
    }
}
