namespace ScottPlot.Rendering.RenderActions;

public class RegenerateTicks : IRenderAction
{
    public void Render(RenderPack rp)
    {
        foreach (IXAxis xAxis in rp.Plot.PlottableList.Select(static x => x.Axes.XAxis).OfType<IXAxis>().Distinct())
        {
            xAxis.RegenerateTicks(rp.DataRect.Width);
        }

        foreach (IYAxis yAxis in rp.Plot.PlottableList.Select(static x => x.Axes.YAxis).OfType<IYAxis>().Distinct())
        {
            yAxis.RegenerateTicks(rp.DataRect.Height);
        }
    }
}
