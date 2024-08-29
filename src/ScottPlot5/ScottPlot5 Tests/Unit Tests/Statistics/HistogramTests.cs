using ScottPlot.Statistics;

namespace ScottPlotTests.Statistics;

internal class HistogramTests
{
    [Test]
    public void TestHistogramIgnoringOutliers()
    {
        Histogram hist = new Histogram(100, 200, 5, false, false);

        hist.Min.Should().Be(100);
        hist.Max.Should().Be(200);
        hist.Bins[0].Should().Be(100);
        hist.Bins[^1].Should().Be(180);

        hist.Bins.Should().BeEquivalentTo(new double[] { 100, 120, 140, 160, 180 });
        hist.Counts.Should().BeEquivalentTo(new double[] { 0, 0, 0, 0, 0 });

        hist.Add(123);
        hist.Counts.Should().BeEquivalentTo(new double[] { 0, 1, 0, 0, 0 });

        hist.Add(173);
        hist.Counts.Should().BeEquivalentTo(new double[] { 0, 1, 0, 1, 0 });

        hist.Add(123);
        hist.Counts.Should().BeEquivalentTo(new double[] { 0, 2, 0, 1, 0 });

        hist.Sum.Should().Be(123 + 173 + 123);

        hist.Clear();
        hist.Counts.Should().BeEquivalentTo(new double[] { 0, 0, 0, 0, 0 });

        hist.Add(50);
        hist.Counts.Should().BeEquivalentTo(new double[] { 0, 0, 0, 0, 0 });

        hist.Add(250);
        hist.Counts.Should().BeEquivalentTo(new double[] { 0, 0, 0, 0, 0 });

        hist.Sum.Should().Be(0);
    }

    [Test]
    public void TestHistogramIncludingOutliers()
    {
        Histogram hist = new Histogram(100, 200, 5, true, false);

        hist.Counts.Should().BeEquivalentTo(new double[] { 0, 0, 0, 0, 0 });

        hist.Add(50);
        hist.Counts.Should().BeEquivalentTo(new double[] { 1, 0, 0, 0, 0 });

        hist.Add(250);
        hist.Counts.Should().BeEquivalentTo(new double[] { 1, 0, 0, 0, 1 });

        hist.Sum.Should().Be(50 + 250);
    }

    [Test]
    public void TestHistogramNormalization()
    {
        Histogram hist = new Histogram(100, 200, 5, addFinalBin: false);

        hist.Add(125);
        hist.Add(145);
        hist.Add(145);
        hist.Add(165);
        hist.Counts.Should().BeEquivalentTo(new double[] { 0, 1, 2, 1, 0 });

        hist.GetProbability().Should().BeEquivalentTo([0, .25, .5, .25, 0]);

        hist.GetNormalized().Should().BeEquivalentTo([0, .5, 1, .5, 0]);

        hist.GetNormalized(256).Should().BeEquivalentTo(new double[] { 0, 128, 256, 128, 0 });
    }

    [Test]
    public void TestHistogramCph()
    {
        Histogram hist = new Histogram(100, 200, 5, addFinalBin: false);

        hist.Add(125);
        hist.Add(145);
        hist.Add(145);
        hist.Add(165);
        hist.Counts.Should().BeEquivalentTo(new double[] { 0, 1, 2, 1, 0 });

        hist.GetCumulative().Should().BeEquivalentTo(new double[] { 0, 1, 3, 4, 4 });

        hist.GetCumulativeProbability().Should().BeEquivalentTo([0, .25, .75, 1, 1]);
    }

    [Test]
    public void TestHistogramFixedBinSize()
    {
        // Extending conversation in #2403, this test confirms bins meet expectations
        // https://github.com/ScottPlot/ScottPlot/issues/2403

        Histogram hist1 = Histogram.WithFixedBinSize(0, 10, 1);

        hist1.BinSize.Should().Be(1);

        hist1.Bins.Should().BeEquivalentTo(new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
        hist1.Counts.Should().BeEquivalentTo(new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

        hist1.Add(10); // since bins are max-exclusive, this counts as an outlier
        hist1.Counts.Should().BeEquivalentTo(new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 });
    }

    [Test]
    public void TestHistogramFixedBinCount()
    {
        // Extending conversation in #2403, this test confirms bins meet expectations
        // https://github.com/ScottPlot/ScottPlot/issues/2403

        Histogram hist1 = Histogram.WithFixedBinCount(0, 10, 10);

        hist1.BinSize.Should().Be(1);

        hist1.Bins.Should().BeEquivalentTo(new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
        hist1.Counts.Should().BeEquivalentTo(new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

        hist1.Add(10); // since bins are max-exclusive, this counts as an outlier
        hist1.Counts.Should().BeEquivalentTo(new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 });
    }

    [Test]
    public void TestHistogramFractionalBinSize()
    {
        // https://github.com/ScottPlot/ScottPlot/issues/2490

        Histogram hist1 = Histogram.WithFixedBinCount(0, 1, 10);

        hist1.BinSize.Should().Be(0.1);

        double[] expectedBins = [0.0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0];

        for (int i = 0; i < expectedBins.Length; i++)
        {
            hist1.Bins[i].Should().BeApproximately(expectedBins[i], 1e-10);
        }
    }

    [Test]
    public void TestHistogramMinMaxValidation()
    {
        FluentActions.Invoking(static () => Histogram.WithFixedBinCount(0, 1, 1)).Should().NotThrow();

        FluentActions.Invoking(static () => Histogram.WithFixedBinCount(1, 0, 10)).Should().Throw<ArgumentException>();

        FluentActions.Invoking(static () => Histogram.WithFixedBinCount(1, 1, 10)).Should().Throw<ArgumentException>();

        FluentActions.Invoking(static () => Histogram.WithFixedBinCount(0, 1, 0)).Should().Throw<ArgumentException>();
    }
}
