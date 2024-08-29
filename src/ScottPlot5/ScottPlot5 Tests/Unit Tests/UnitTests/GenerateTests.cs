namespace ScottPlotTests.UnitTests;

internal class GenerateTests
{
    [Test]
    public void Test_Weekdays()
    {
        foreach (DateTime dt in Generate.ConsecutiveWeekdays(100))
        {
            dt.DayOfWeek.Should().NotBe(DayOfWeek.Saturday);
            dt.DayOfWeek.Should().NotBe(DayOfWeek.Sunday);
        }
    }
}
