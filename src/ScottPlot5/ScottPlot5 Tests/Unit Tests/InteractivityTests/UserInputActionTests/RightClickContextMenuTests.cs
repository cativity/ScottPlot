using ScottPlot.Testing;

namespace ScottPlotTests.InteractivityTests.UserInputActionTests;

internal class RightClickContextMenuTests
{
    [Test]
    public void TestRightClickContextMenuLaunchesMenu()
    {
        MockPlotControl plotControl = new MockPlotControl();
        plotControl.ContextMenuLaunchCount.Should().Be(0);

        plotControl.RightClick(plotControl.Center);
        plotControl.ContextMenuLaunchCount.Should().Be(1);

        plotControl.RightClick(plotControl.Center);
        plotControl.ContextMenuLaunchCount.Should().Be(2);
    }

    [Test]
    public void TestRightClickDragDoesNotLaunchMenu()
    {
        MockPlotControl plotControl = new MockPlotControl();
        plotControl.ContextMenuLaunchCount.Should().Be(0);

        plotControl.RightClickDrag(plotControl.Center, plotControl.Center.MovedRight(50).MovedUp(50));
        plotControl.ContextMenuLaunchCount.Should().Be(0);

        plotControl.RightClick(plotControl.Center);
        plotControl.ContextMenuLaunchCount.Should().Be(1);
    }
}
