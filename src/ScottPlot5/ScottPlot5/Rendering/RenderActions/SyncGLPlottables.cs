namespace ScottPlot.Rendering.RenderActions;

public class SyncGLPlottables : IRenderAction
{
    public void Render(RenderPack rp)
    {
        rp.Plot.PlottableList.OfType<IPlottableGL>().FirstOrDefault()?.GLFinish();
    }
}
