namespace ScottPlotTests.UnitTests;

internal class GenerateTests
{
    [Test]
    public void TestWeekdays()
    {
        foreach (DateTime dt in Generate.ConsecutiveWeekdays(100))
        {
            dt.DayOfWeek.Should().NotBe(DayOfWeek.Saturday);
            dt.DayOfWeek.Should().NotBe(DayOfWeek.Sunday);
        }
    }
}
