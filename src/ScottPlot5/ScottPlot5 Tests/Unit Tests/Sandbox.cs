namespace ScottPlotTests;

internal class Sandbox
{
    [Test]
    public void TestSandbox()
    {
        Plot plt = new Plot();
        plt.Axes.Left.Label.Text = "Vertical Axis";
        plt.Axes.Left.MinimumSize = 100;
        plt.SaveTestImage();
    }
}
