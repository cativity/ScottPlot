namespace ScottPlotTests.RenderTests.Figure;

public class Tests
{
    [Test]
    public void TestGenerateConsecutive()
    {
        double[] values = Generate.Consecutive(10);
        values.Should().NotBeNullOrEmpty();
        values.Should().HaveCount(10);
    }

    [Test]
    public void TestRandomNormal()
    {
        double[] values = Generate.RandomNormal(10);
        Console.WriteLine(string.Join(Environment.NewLine, values.Select(static x => x.ToString())));
    }

    [Test]
    public void TestRange()
    {
        double[] values = Generate.Range(7, 9, 0.5);
        double[] expected = [7, 7.5, 8, 8.5, 9];
        values.Should().BeEquivalentTo(expected);
    }
}
