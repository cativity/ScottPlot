using ScottPlot.TickGenerators;

namespace ScottPlotTests.UnitTests.TickGenerators;

internal class DecimalTickSpacingCalculatorTests
{
    [Test]
    public void TestCalculatorLabelsShouldAlwaysFitInGivenSpace()
    {
        DecimalTickSpacingCalculator calc = new DecimalTickSpacingCalculator();
        CoordinateRange range = new CoordinateRange(-500_000_000, 500_000_000);
        PixelLength axisLength = new PixelLength(500);

        for (int i = 10; i < 100; i += 10)
        {
            PixelLength maxLabelLength = new PixelLength(i);
            double[] positions = calc.GenerateTickPositions(range, axisLength, maxLabelLength);
            double spacePerLabel = axisLength.Length / positions.Length;
            Console.WriteLine($"when labels are {maxLabelLength}, each {spacePerLabel} px of space");
            spacePerLabel.Should().BeGreaterThan(maxLabelLength.Length);
        }
    }
}
