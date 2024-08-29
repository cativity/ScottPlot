namespace ScottPlot.DataSources;

public class CacheScatterLimitsDecorator(IScatterSource source) : IScatterSource
{
    public int MinRenderIndex
    {
        get => source.MinRenderIndex;
        set => source.MinRenderIndex = value;
    }

    public int MaxRenderIndex
    {
        get => source.MaxRenderIndex;
        set => source.MaxRenderIndex = value;
    }

    private AxisLimits? _axisLimits;
    private CoordinateRange _limitsX = CoordinateRange.NotSet;
    private CoordinateRange _limitsY = CoordinateRange.NotSet;

    public AxisLimits GetLimits()
    {
        _axisLimits ??= source.GetLimits();

        return _axisLimits.Value;
    }

    public CoordinateRange GetLimitsX()
    {
        if (_limitsX == CoordinateRange.NotSet)
        {
            _limitsX = source.GetLimitsX();
        }

        return _limitsX;
    }

    public CoordinateRange GetLimitsY()
    {
        if (_limitsY == CoordinateRange.NotSet)
        {
            _limitsY = source.GetLimitsY();
        }

        return _limitsY;
    }

    public DataPoint GetNearest(Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance = 15)
        => source.GetNearest(mouseLocation, renderInfo, maxDistance);

    public IReadOnlyList<Coordinates> GetScatterPoints() => source.GetScatterPoints();

    public DataPoint GetNearestX(Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance = 15)
        => throw
               // TODO: Implement GetNearestX() in this DataSource
               // Code can be copied from ScatterSourceDoubleArray.GetNearestX() and modified as needed
               // Contributions are welcome!
               // https://github.com/ScottPlot/ScottPlot/issues/3807
               new NotImplementedException();
}
