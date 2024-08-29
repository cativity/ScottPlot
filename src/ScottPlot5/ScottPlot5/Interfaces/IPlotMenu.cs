namespace ScottPlot;

public interface IPlotMenu
{
    public void Reset();

    public void Clear();

    public void Add(string label, Action<IPlotControl> action);

    public void AddSeparator();

    void ShowContextMenu(Pixel pixel);
}
