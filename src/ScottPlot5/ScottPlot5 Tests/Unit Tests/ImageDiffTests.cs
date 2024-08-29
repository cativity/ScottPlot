using ScottPlot.Testing;

namespace ScottPlotTests;

internal class ImageDiffTests
{
    [Test]
    public void TestImageDiff()
    {
        Image img1 = new Image("TestImages/bag_frame1.png");
        Image img2 = new Image("TestImages/bag_frame2.png");
        ImageDiff diff = new ImageDiff(img1, img2);

        diff.PercentOfDifferencePixels.Should().BeApproximately(17.94, .01);
        diff.NumberOfDifferentPixels.Should().Be(1601);
        diff.DifferenceImage?.SaveTestImage();
    }
}
