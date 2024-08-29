namespace ScottPlotTests.RenderTests;

internal class FileFormatTests
{
    [Test]
    public void TestSaveBmp()
    {
        Plot plt = new Plot();
        plt.Add.Signal(Generate.Sin(51));
        plt.Add.Signal(Generate.Cos(51));
        plt.SaveBmp("test-images/test_save.bmp", 200, 100);
    }

    [Test]
    public void TestSaveJpeg()
    {
        Plot plt = new Plot();
        plt.Add.Signal(Generate.Sin(51));
        plt.Add.Signal(Generate.Cos(51));
        plt.SaveJpeg("test-images/test_save.jpg", 200, 100);
    }

    [Test]
    public void TestSavePng()
    {
        Plot plt = new Plot();
        plt.Add.Signal(Generate.Sin(51));
        plt.Add.Signal(Generate.Cos(51));
        plt.SavePng("test-images/test_save.png", 200, 100);
    }

    [Test]
    public void TestSaveWebp()
    {
        Plot plt = new Plot();
        plt.Add.Signal(Generate.Sin(51));
        plt.Add.Signal(Generate.Cos(51));
        plt.SaveWebp("test-images/test_save.webp", 200, 100);
    }

    [Test]
    public void TestSaveHtml()
    {
        Plot plt = new Plot();
        plt.Add.Signal(Generate.Sin(51));
        plt.Add.Signal(Generate.Cos(51));
        string img = plt.GetImageHtml(300, 200);
        string html = $"<html><body>{img}</body></html>";
        html.SaveTestString();
    }

    [Test]
    public void TestSaveSvg()
    {
        Plot plt = new Plot();
        plt.Add.Signal(Generate.Sin(51));
        plt.Add.Signal(Generate.Cos(51));
        plt.SaveSvg("test-images/test_save.svg", 400, 300);
    }

    [Test]
    public void TestGetSvg()
    {
        Plot plt = new Plot();
        plt.Add.Signal(Generate.Sin(51));
        plt.Add.Signal(Generate.Cos(51));
        string svgXml = plt.GetSvgXml(400, 300);
        Assert.That(!string.IsNullOrEmpty(svgXml), Is.True);
    }

    [Test]
    public void TestSvgEmptyRect()
    {
        // Empty rectangles have outlines in some browsers
        // https://github.com/ScottPlot/ScottPlot/issues/3709

        Plot plt = new Plot();
        plt.GetSvgXml(600, 400).Should().NotContain("""<rect width="600" height="400"/>""");
    }
}
