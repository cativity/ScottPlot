namespace ScottPlotTests.UnitTests.PrimitiveTests;

public class LinePatternTests
{
    [Test]
    public void TestEveryLinePatternHasPathEffect()
    {
        foreach (LinePattern pattern in Enum.GetValues<LinePattern>())
        {
            Action act = () => pattern.GetPathEffect();
            act.Should().NotThrow();
        }
    }
}
