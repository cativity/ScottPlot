﻿using ScottPlot.Plottables;

namespace ScottPlotTests.RenderTests;

internal class MultiAxisTests
{
    [Test]
    public void TestMultiAxisMemory()
    {
        Plot myPlot = new Plot();

        myPlot.Add.Signal(Generate.Sin(51, 0.01));

        Signal sig2 = myPlot.Add.Signal(Generate.Cos(51, 100));
        sig2.Axes.YAxis = myPlot.Axes.AddLeftAxis();

        myPlot.SaveTestImage();

        myPlot.LastRender.AxisLimitsByAxis.Should().NotBeNullOrEmpty();
        myPlot.LastRender.AxisLimitsByAxis.Count.Should().Be(5);

        foreach ((IAxis axis, CoordinateRange range) in myPlot.LastRender.AxisLimitsByAxis)
        {
            Console.WriteLine($"{axis} {range}");
        }
    }

    [Test]
    public void TestMultiAxisRemove()
    {
        Plot myPlot = new Plot();

        myPlot.Add.Signal(Generate.Sin(51, 10));
        Signal sig2 = myPlot.Add.Signal(Generate.Cos(51, 1));

        // create an additional axis and setup the second signal to use it
        IYAxis secondYAxis = myPlot.Axes.AddLeftAxis();
        sig2.Axes.YAxis = secondYAxis;
        myPlot.Should().SavePngWithoutThrowing("1");

        // remove the additional axis
        myPlot.Axes.Remove(secondYAxis);

        // tell the signal plot to use the original axis
        sig2.Axes.YAxis = myPlot.Axes.Left;
        myPlot.Should().SavePngWithoutThrowing("2");
    }

    [Test]
    public void TestRightAxisNoLeftAxis()
    {
        Plot myPlot = new Plot();

        Signal sig = myPlot.Add.Signal(Generate.Sin());
        sig.Axes.YAxis = myPlot.Axes.Right;

        myPlot.Axes.Left.Range.HasBeenSet.Should().BeFalse();
        myPlot.Axes.Right.Range.HasBeenSet.Should().BeFalse();

        myPlot.Should().SavePngWithoutThrowing();
        myPlot.Axes.Left.Range.HasBeenSet.Should().BeFalse();
        myPlot.Axes.Right.Range.HasBeenSet.Should().BeTrue();

        myPlot.Should().SavePngWithoutThrowing();
        myPlot.Axes.Left.Range.HasBeenSet.Should().BeFalse();
        myPlot.Axes.Right.Range.HasBeenSet.Should().BeTrue();
    }
}
