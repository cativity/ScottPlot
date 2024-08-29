using ScottPlot.Statistics;

namespace ScottPlotTests.Statistics;

internal class ArrayTests
{
    private readonly double[] _sample1D = [2, 3, 5, 7];

    private readonly double[] _sample1DWithNan = [2, 3, double.NaN, 7];

    private readonly double[,] _sample2D = { { 2, 3, 5, 7 }, { 11, 13, 17, 19 }, { 23, 29, 31, 37 } };

    private readonly double[,] _sample2DWithNaN = { { 2, 3, double.NaN, 7 }, { 11, double.NaN, double.NaN, double.NaN }, { 23, double.NaN, double.NaN, 37 } };

    [Test]
    public void Test_ArrayStats1D_NanMean()
    {
        // known values calculated with https://goodcalculators.com/standard-error-calculator/

        // real of all values are real
        Descriptive.Mean(_sample1D).Should().Be(4.25);

        // NaN if any values are nan
        double.IsNaN(Descriptive.Mean(_sample1DWithNan)).Should().BeTrue();

        // Nan methods ignore NaN values
        Descriptive.NanMean(_sample1DWithNan).Should().Be(4);
    }

    [Test]
    public void Test_ArrayStats1D_NanStdev()
    {
        // known values calculated with https://goodcalculators.com/standard-error-calculator/

        // real of all values are real
        Descriptive.StandardDeviation(_sample1D).Should().BeApproximately(2.217356, 1e-5);
        Descriptive.StandardError(_sample1D).Should().BeApproximately(1.1086778913, 1e-5);

        // NaN if any values are nan
        double.IsNaN(Descriptive.StandardDeviation(_sample1DWithNan)).Should().BeTrue();
        double.IsNaN(Descriptive.StandardError(_sample1DWithNan)).Should().BeTrue();

        // Nan methods ignore NaN values
        Descriptive.NanStandardDeviation(_sample1DWithNan).Should().BeApproximately(2.645751, 1e-5);
        Descriptive.NanStandardError(_sample1DWithNan).Should().BeApproximately(1.5275252317, 1e-5);
    }

    [Test]
    public void Test_ArrayStats2D_AllReal()
    {
        // known values calculated with https://goodcalculators.com/standard-error-calculator/

        double[] expectedMeans = [12, 15, 17.66666, 21];
        double[] expectedStandardError = [6.0827625303, 7.5718777944, 7.5129517797, 8.7177978871];

        double[] vMeans = Descriptive.VerticalMean(_sample2D);
        double[] vStdErrs = Descriptive.VerticalStandardError(_sample2D);

        vMeans.Length.Should().Be(4);
        vStdErrs.Length.Should().Be(4);

        for (int i = 0; i < 4; i++)
        {
            vMeans[i].Should().BeApproximately(expectedMeans[i], 1e-5);
            vStdErrs[i].Should().BeApproximately(expectedStandardError[i], 1e-5);
        }
    }

    [Test]
    public void Test_ArrayStats2D_WithNan()
    {
        // known values calculated with https://goodcalculators.com/standard-error-calculator/

        double[] expectedMeans = [12, 3, double.NaN, 22];
        Descriptive.VerticalNanMean(_sample2DWithNaN).Should().BeEquivalentTo(expectedMeans);

        double[] expectedStandardError = [6.082762530298219, 0, double.NaN, 15];
        Descriptive.VerticalNanStandardError(_sample2DWithNaN).Should().BeEquivalentTo(expectedStandardError);
    }
}
