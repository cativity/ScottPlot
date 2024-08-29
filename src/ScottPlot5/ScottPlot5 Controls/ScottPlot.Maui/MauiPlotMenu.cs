namespace ScottPlot.Maui;

public class MauiPlotMenu(MauiPlot mp) : IPlotMenu
{
    private readonly MauiPlot _mauiPlot = mp;

    public void Add(string label, Action<IPlotControl> action)
    {
        throw new NotImplementedException();
    }

    public void AddSeparator()
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public void ShowContextMenu(Pixel pixel)
    {
        throw new NotImplementedException();
    }
}
