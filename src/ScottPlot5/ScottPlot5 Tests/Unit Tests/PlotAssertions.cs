﻿using System.Diagnostics;
using System.Reflection;

namespace ScottPlotTests;

internal class PlotAssertions(Plot plot)
{
    public void SavePngWithoutThrowing(string subName = "", int width = 400, int height = 300)
    {
        StackTrace stackTrace = new StackTrace();
        StackFrame frame = stackTrace.GetFrame(1) ?? throw new InvalidOperationException("unknown caller");
        MethodBase method = frame.GetMethod() ?? throw new InvalidDataException("unknown method");
        string callingMethod = method.Name;

        string saveFolder = Path.Combine(TestContext.CurrentContext.TestDirectory, "test-images");

        if (!Directory.Exists(saveFolder))
        {
            Directory.CreateDirectory(saveFolder);
        }

        string fileName = string.IsNullOrWhiteSpace(subName) ? $"{callingMethod}.png" : $"{callingMethod}-{subName}.png";

        string filePath = Path.Combine(saveFolder, fileName);
        Console.WriteLine(filePath);

        plot.SavePng(filePath, width, height);
    }

    public void RenderInMemoryWithoutThrowing(int width = 400, int height = 300)
    {
        plot.GetImage(width, height);
    }

    public void RenderIdenticallyTo(Plot otherPlot, int width = 400, int height = 300)
    {
        byte[] bytes1 = plot.GetImage(width, height).GetImageBytes();
        byte[] bytes2 = otherPlot.GetImage(width, height).GetImageBytes();

        if (bytes1.Length != bytes2.Length)
        {
            throw new InvalidOperationException("images are not identical");
        }

        if (bytes1.Where((t, i) => t != bytes2[i]).Any())
        {
            throw new InvalidOperationException("images are not identical");
        }
    }
}
