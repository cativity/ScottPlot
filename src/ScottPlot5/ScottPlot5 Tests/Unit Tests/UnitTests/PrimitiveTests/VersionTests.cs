using Version = ScottPlot.Version;

namespace ScottPlotTests.UnitTests.PrimitiveTests;

internal class VersionTests
{
    [Test]
    public void Test_Version_Numbers_Valid()
    {
        Version.VersionString.Should().NotBeNullOrWhiteSpace();
        Version.Major.Should().BeGreaterThanOrEqualTo(5);
        Version.Minor.Should().BeGreaterThanOrEqualTo(0);
        Version.Build.Should().BeGreaterThanOrEqualTo(0);
    }
}
