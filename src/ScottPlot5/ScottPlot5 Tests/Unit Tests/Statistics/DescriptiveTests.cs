using ScottPlot.Statistics;

namespace ScottPlotTests.Statistics;

/* known values obtained from an online calculator:
 * https://www.calculatorsoup.com/calculators/statistics/variance-calculator.php
 */

public class DescriptiveTests
{
    [Test]
    public void Test_Sum_MatchesKnownValue()
    {
        Descriptive.Sum(SampleData.FirstHundredPrimes).Should().Be(24133);
    }

    [Test]
    public void Test_Mean_MatchesKnownValue()
    {
        Descriptive.Mean(SampleData.FirstHundredPrimes).Should().Be(241.33);
    }

    [Test]
    public void Test_Variance_MatchesKnownValue()
    {
        Descriptive.Variance(SampleData.FirstHundredPrimes).Should().BeApproximately(25865.759, 1e-3);
    }

    [Test]
    public void Test_VarianceP_MatchesKnownValue()
    {
        Descriptive.VarianceP(SampleData.FirstHundredPrimes).Should().BeApproximately(25607.101, 1e-3);
    }

    [Test]
    public void Test_StandardDeviation_MatchesKnownValue()
    {
        Descriptive.StandardDeviation(SampleData.FirstHundredPrimes).Should().BeApproximately(160.82835, 1e-5);
    }

    [Test]
    public void Test_StandardDeviationP_MatchesKnownValue()
    {
        Descriptive.StandardDeviationP(SampleData.FirstHundredPrimes).Should().BeApproximately(160.02219, 1e-5);
    }

    [Test]
    public void Test_Median1_MatchesKnownValues()
    {
        Descriptive.Median([42]).Should().Be(42);
    }

    [Test]
    public void Test_Median2_MatchesKnownValues()
    {
        Descriptive.Median([12, 13]).Should().Be(12.5);
    }

    [Test]
    public void Test_Median3_MatchesKnownValues()
    {
        Descriptive.Median([12, 13, 14]).Should().Be(13);
    }

    [Test]
    public void Test_Median4_MatchesKnownValues()
    {
        Descriptive.Median([12, 13, 14, 15]).Should().Be(13.5);
    }

    [Test]
    public void Test_Median5_MatchesKnownValues()
    {
        Descriptive.Median([16, 12, 13, 14, 15]).Should().Be(14);
    }
}
