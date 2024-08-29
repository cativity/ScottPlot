using System.Diagnostics.CodeAnalysis;

namespace ScottPlotTests.UnitTests;

internal class PlotCoordinateConversion
{
    [Test]
    public void TestPlotPixelToCoordsAndBack()
    {
        Plot plt = new Plot();
        plt.Axes.SetLimits(-5, 25, 10, 30);
        plt.SaveTestImage();

        Pixel initialPx = new Pixel(55.0F, 333.0F);
        Coordinates coordinates = plt.GetCoordinates(initialPx);
        Pixel convertedPx = plt.GetPixel(coordinates);

        convertedPx.X.Should().Be(initialPx.X);
        convertedPx.Y.Should().Be(initialPx.Y);
    }

    [Test]
    public void TestPlotPixelToCoordsAndBackScaled()
    {
        Plot plt = new Plot();
        plt.Axes.SetLimits(1, 2, -15, -5);
        plt.ScaleFactor = 2.5; // Change from default of 1.0
        plt.SaveTestImage();

        Pixel initialPx = new Pixel(329.0F, 200.0F);
        Coordinates coordinates = plt.GetCoordinates(initialPx);
        Pixel convertedPx = plt.GetPixel(coordinates);

        convertedPx.X.Should().Be(initialPx.X);
        convertedPx.Y.Should().Be(initialPx.Y);
    }

    [TestCase(1.0)]
    [TestCase(2.0)]
    [TestCase(0.5)]
    public void TestGetCoordinateRectFromPixels(double scaleFactor)
    {
        const double xCoordMin = -20;
        const double xCoordMax = 20;
        const double yCoordMin = -10;
        const double yCoordMax = 10;
        const double widthCoord = xCoordMax - xCoordMin;
        const double heightCoord = yCoordMax - yCoordMin;

        const double widthPx = 800;
        const double heightPx = 400;

        Plot plot = new Plot { ScaleFactor = (float)scaleFactor };
        plot.Axes.SetLimits(xCoordMin, xCoordMax, yCoordMin, yCoordMax);
        plot.Layout.Frameless();

        // Render the plot
        plot.SaveTestImage((int)widthPx, (int)heightPx);

        const double xPx = 200;
        const double yPx = 300;
        const double radius = 40;
        Pixel px = new Pixel((float)xPx, (float)yPx);

        _ = plot.GetCoordinates((float)(xPx + radius), (float)(yPx - radius));
        _ = plot.GetCoordinates((float)(xPx - radius), (float)(yPx + radius));
        //Coordinates rightTopCoords = plot.GetCoordinates((float)(xPx + radius), (float)(yPx - radius));
        //Coordinates leftBotCoords = plot.GetCoordinates((float)(xPx - radius), (float)(yPx + radius));

        CoordinateRect rect1 = plot.GetCoordinateRect((float)xPx, (float)yPx, (float)radius);
        CoordinateRect rect2 = plot.GetCoordinateRect(px, (float)radius);

        // Expected values
        const double left = xCoordMin + ((xPx - radius) * widthCoord / widthPx);
        const double right = xCoordMin + ((xPx + radius) * widthCoord / widthPx);
        const double top = yCoordMax - ((yPx - radius) * heightCoord / heightPx);
        const double bottom = yCoordMax - ((yPx + radius) * heightCoord / heightPx);

        Assert.Multiple(() =>
        {
            Assert.That(rect1.Left, Is.EqualTo(left), "rect1 left");
            Assert.That(rect1.Right, Is.EqualTo(right), "rect1 right");
            Assert.That(rect1.Top, Is.EqualTo(top), "rect1 top");
            Assert.That(rect1.Bottom, Is.EqualTo(bottom), "rect1 bottom");

            Assert.That(rect2.Left, Is.EqualTo(left), "rect2 left");
            Assert.That(rect2.Right, Is.EqualTo(right), "rect2 right");
            Assert.That(rect2.Top, Is.EqualTo(top), "rect2 top");
            Assert.That(rect2.Bottom, Is.EqualTo(bottom), "rect2 bottom");
        });
    }

    [TestCase(1.0)]
    [TestCase(2.0)]
    [TestCase(0.5)]
    public void TestGetCoordinateRectVsGetCoordinates(double scaleFactor)
    {
        Plot plot = new Plot { ScaleFactor = (float)scaleFactor };
        plot.Axes.SetLimits(5, 10, -20, -10);

        // Render the plot
        plot.SaveTestImage(400, 600);

        const float xPx = 100;
        const float yPx = 50;
        const float radius = 20;

        Coordinates rightTopCoords = plot.GetCoordinates(xPx + radius, yPx - radius);
        Coordinates leftBotCoords = plot.GetCoordinates(xPx - radius, yPx + radius);
        CoordinateRect rect = plot.GetCoordinateRect(xPx, yPx, radius);

        Assert.Multiple(() =>
        {
            Assert.That(rect.Left, Is.EqualTo(leftBotCoords.X), "left");
            Assert.That(rect.Right, Is.EqualTo(rightTopCoords.X), "right");
            Assert.That(rect.Top, Is.EqualTo(rightTopCoords.Y), "top");
            Assert.That(rect.Bottom, Is.EqualTo(leftBotCoords.Y), "bottom");
        });
    }

    [TestCase(1.0)]
    [TestCase(2.0)]
    [TestCase(0.5)]
    public void TestGetCoordinateRectFromCoords(double scaleFactor)
    {
        const double xCoordMin = 0;
        const double xCoordMax = 10;
        const double yCoordMin = 0;
        const double yCoordMax = 20;
        const double widthCoord = xCoordMax - xCoordMin;
        const double heightCoord = yCoordMax - yCoordMin;

        const double widthPx = 400;
        const double heightPx = 600;

        Plot plot = new Plot { ScaleFactor = (float)scaleFactor };
        plot.Axes.SetLimits(xCoordMin, xCoordMax, yCoordMin, yCoordMax);
        plot.Layout.Frameless();

        // Render the plot
        plot.SaveTestImage((int)widthPx, (int)heightPx);

        Coordinates coords = new Coordinates(7, 5);
        const double radius = 10; // pixels
        const double radiusXCoord = radius * widthCoord / widthPx;
        const double radiusYCoord = radius * heightCoord / heightPx;

        CoordinateRect rect = plot.GetCoordinateRect(coords, (float)radius);

        Assert.Multiple(() =>
        {
            Assert.That(rect.Left, Is.EqualTo(coords.X - radiusXCoord), "left");
            Assert.That(rect.Right, Is.EqualTo(coords.X + radiusXCoord), "right");
            Assert.That(rect.Top, Is.EqualTo(coords.Y + radiusYCoord), "top");
            Assert.That(rect.Bottom, Is.EqualTo(coords.Y - radiusYCoord), "bottom");
        });
    }
}
