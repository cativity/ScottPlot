namespace ScottPlotTests.RenderTests.Figure;

internal class BasicImageTests
{
    [Test]
    public void TestRenderSinCos()
    {
        Plot plt = new Plot();

        plt.Add.Signal(Generate.Sin(51));
        plt.Add.Signal(Generate.Cos(51));

        plt.SaveTestImage();
    }

    [Test]
    public void TestNoData()
    {
        Plot plt = new Plot();
        plt.SaveTestImage();
    }
}
