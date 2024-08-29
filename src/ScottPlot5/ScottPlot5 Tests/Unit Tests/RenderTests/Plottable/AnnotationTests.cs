namespace ScottPlotTests.RenderTests.Plottable;

internal class AnnotationTests
{
    [Test]
    public void TestAnnotationAlignment()
    {
        Plot plt = new Plot();

        foreach (Alignment alignment in Enum.GetValues(typeof(Alignment)))
        {
            plt.Add.Annotation(alignment.ToString(), alignment);
        }

        plt.SaveTestImage();
    }

    [Test]
    public void TestAnnotationHeight()
    {
        //https://github.com/ScottPlot/ScottPlot/issues/3749
        Plot plt = new Plot();
        string multiline = string.Join("\n", Enumerable.Range(0, 10).Select(static x => $"Line {x + 1}"));
        plt.Add.Annotation(multiline);
        plt.SaveTestImage();
    }
}
