using ScottPlot.Statistics;

namespace ScottPlotTests.Statistics;

/* known values obtained from an online calculator:
 * https://www.calculatorsoup.com/calculators/statistics/variance-calculator.php
 */

public class DescriptiveTests
{
    [Test]
    public void TestSumMatchesKnownValue()
    {
        Descriptive.Sum(SampleData.FirstHundredPrimes).Should().Be(24133);
    }

    [Test]
    public void TestMeanMatchesKnownValue()
    {
        Descriptive.Mean(SampleData.FirstHundredPrimes).Should().Be(241.33);
    }

    [Test]
    public void TestVarianceMatchesKnownValue()
    {
        Descriptive.Variance(SampleData.FirstHundredPrimes).Should().BeApproximately(25865.759, 1e-3);
    }

    [Test]
    public void TestVariancePMatchesKnownValue()
    {
        Descriptive.VarianceP(SampleData.FirstHundredPrimes).Should().BeApproximately(25607.101, 1e-3);
    }

    [Test]
    public void TestStandardDeviationMatchesKnownValue()
    {
        Descriptive.StandardDeviation(SampleData.FirstHundredPrimes).Should().BeApproximately(160.82835, 1e-5);
    }

    [Test]
    public void TestStandardDeviationPMatchesKnownValue()
    {
        Descriptive.StandardDeviationP(SampleData.FirstHundredPrimes).Should().BeApproximately(160.02219, 1e-5);
    }

    [Test]
    public void TestMedian1MatchesKnownValues()
    {
        Descriptive.Median([42]).Should().Be(42);
    }

    [Test]
    public void TestMedian2MatchesKnownValues()
    {
        Descriptive.Median([12, 13]).Should().Be(12.5);
    }

    [Test]
    public void TestMedian3MatchesKnownValues()
    {
        Descriptive.Median([12, 13, 14]).Should().Be(13);
    }

    [Test]
    public void TestMedian4MatchesKnownValues()
    {
        Descriptive.Median([12, 13, 14, 15]).Should().Be(13.5);
    }

    [Test]
    public void TestMedian5MatchesKnownValues()
    {
        Descriptive.Median([16, 12, 13, 14, 15]).Should().Be(14);
    }
}
