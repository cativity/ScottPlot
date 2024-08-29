namespace ScottPlot.Rendering.RenderActions;

public class RenderPlottablesLast : IRenderAction
{
    public void Render(RenderPack rp)
    {
        foreach (IRenderLast plottable in rp.Plot.PlottableList.Where(static x => x.IsVisible).OfType<IRenderLast>().ToArray())
        {
            plottable.RenderLast(rp);
        }
    }
}
