using ScottPlot.Rendering.RenderActions;

namespace ScottPlot.Rendering;

public class RenderManager(Plot plot)
{
    /// <summary>
    ///     This list of actions is performed in sequence to render a plot.
    ///     It may be modified externally to inject custom functionality.
    /// </summary>
    public List<IRenderAction> RenderActions { get; } =
    [
        new PreRenderLock(),
        new ClearCanvas(),
        new ReplaceNullAxesWithDefaults(),
        new AutoScaleUnsetAxes(),
        new ContinuouslyAutoscale(),
        new ExecutePlottableAxisManagers(),
        new ApplyAxisRulesBeforeLayout(),
        new CalculateLayout(),
        new RenderFigureBackground(),
        new ApplyAxisRulesAfterLayout(),
        new RegenerateTicks(),
        new RenderStartingEvent(),
        new RenderDataBackground(),
        new RenderGridsBelowPlottables(),
        new RenderPlottables(),
        new RenderGridsAbovePlottables(),
        new RenderLegends(),
        new RenderPanels(),
        new RenderZoomRectangle(),
        new SyncGLPlottables(),
        new RenderPlottablesLast(),
        new RenderBenchmark(),
    ];

    /// <summary>
    ///     Information about the previous render
    /// </summary>
    public RenderDetails LastRender { get; private set; }

    /// <summary>
    ///     These events are invoked before any render action.
    ///     Users can add blocking code to this event to ensure processes
    ///     that modify plottables are complete before rendering begins.
    ///     Alternatively, lock the <see cref="Plot.Sync" /> object.
    /// </summary>
    public EventHandler PreRenderLock { get; set; } = delegate { };

    /// <summary>
    ///     This event is invoked just before each render,
    ///     after axis limits are determined and axis limits are set
    /// </summary>
    public EventHandler<RenderPack> RenderStarting { get; set; } = delegate { };

    /// <summary>
    ///     This event is invoked after each render
    /// </summary>
    public EventHandler<RenderDetails> RenderFinished { get; set; } = delegate { };

    /// <summary>
    ///     This event a render where the figure size (in pixels) changed from the previous render
    /// </summary>
    public EventHandler<RenderDetails> SizeChanged { get; set; } = delegate { };

    /// <summary>
    ///     This event is invoked during a render where the axis limits (in coordinate units) changed from the previous render
    ///     This event occurs after render actions are performed.
    /// </summary>
    public EventHandler<RenderDetails> AxisLimitsChanged { get; set; } = delegate { };

    /// <summary>
    ///     Prevents <see cref="AxisLimitsChanged" /> from being invoked in situations that may cause infinite loops
    /// </summary>
    public bool DisableAxisLimitsChangedEventOnNextRender { get; set; } = false;

    /// <summary>
    ///     Indicates whether this plot is in the process of executing a render
    /// </summary>
    public bool IsRendering { get; private set; } = false;

    /// <summary>
    ///     If false, any calls to Render() return immediately
    /// </summary>
    public bool EnableRendering { get; set; } = true;

    public bool EnableEvents { get; set; } = true;

    public bool ClearCanvasBeforeEachRender { get; set; } = true;

    private Plot Plot { get; } = plot;

    /// <summary>
    ///     Total number of renders completed
    /// </summary>
    public int RenderCount { get; private set; } = 0;

    /// <summary>
    ///     Remove all render actions of the given type
    /// </summary>
    public void Remove<T>()
        where T : IRenderAction
    {
        RenderActions.RemoveAll(x => x is T);
    }

    public void Render(SKCanvas canvas, PixelRect rect)
    {
        canvas.Scale(Plot.ScaleFactorF);
        const int maxRenderCount = 5;

        for (int i = 0; i < maxRenderCount; i++)
        {
            RenderOnce(canvas, rect);

            if (!AxisLimitsChangedSinceLastRender())
            {
                return;
            }
            //Debug.WriteLine($"Re-Render required! #{i}");
        }
    }

    private void RenderOnce(SKCanvas canvas, PixelRect rect)
    {
        if (!EnableRendering)
        {
            return;
        }

        IsRendering = true;

        // TODO: make this an object
        List<(string, TimeSpan)> actionTimes = [];

        RenderPack rp = new RenderPack(Plot, rect, canvas);

        Stopwatch sw = new Stopwatch();

        foreach (IRenderAction action in RenderActions)
        {
            sw.Restart();
            rp.CanvasState.Save();
            action.Render(rp);
            rp.CanvasState.RestoreAll();
            actionTimes.Add((action.ToString() ?? string.Empty, sw.Elapsed));
        }

        LastRender = new RenderDetails(rp, [.. actionTimes], LastRender);
        RenderCount++;
        IsRendering = false;

        if (EnableEvents)
        {
            RenderFinished.Invoke(Plot, LastRender);

            if (LastRender.SizeChanged)
            {
                SizeChanged.Invoke(Plot, LastRender);
            }

            if (!DisableAxisLimitsChangedEventOnNextRender && LastRender.AxisLimitsChanged)
            {
                AxisLimitsChanged.Invoke(Plot, LastRender);
                Plot.Axes.ApplyLinkedAxisRules();
            }
        }

        DisableAxisLimitsChangedEventOnNextRender = false;

        // TODO: event for when layout changes
    }

    private bool AxisLimitsChangedSinceLastRender()
    {
        foreach (IAxis axis in LastRender.AxisLimitsByAxis.Keys)
        {
            //if (axis is null)
            //{
            //    continue;
            //}

            if (double.IsNaN(axis.Range.Span))
            {
                continue;
            }

            CoordinateRangeMutable rangeNow = axis.Range;
            CoordinateRange rangeBefore = LastRender.AxisLimitsByAxis[axis];
            bool axisLimitsChanged = rangeNow.Min != rangeBefore.Min || rangeNow.Max != rangeBefore.Max;

            if (axisLimitsChanged)
            {
                return true;
            }
        }

        return false;
    }
}
