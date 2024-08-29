namespace ScottPlot.Rendering.RenderActions;

public class ApplyAxisRulesBeforeLayout : IRenderAction
{
    public void Render(RenderPack rp)
    {
        rp.Plot.Axes.Rules.ForEach(x => x.Apply(rp, true));
    }
}
