using System.Reflection;
using System.Text;

namespace ScottPlotTests.CodeTests;

internal class ForbiddenCodeTests
{
    private readonly string[] _sourceFilePaths = SourceCodeParsing.GetSourceFilePaths();

    [Test]
    public void TestCanvasSaveIsNotCalledDirectly()
    {
        int offences = 0;
        StringBuilder errorMessages = new StringBuilder();

        foreach (string filePath in _sourceFilePaths)
        {
            if (filePath.EndsWith("CanvasState.cs"))
            {
                continue;
            }

            string[] lines = File.ReadAllLines(filePath);

            for (int i = 0; i < lines.Length; i++)
            {
                string file2 = filePath.Replace(SourceCodeParsing.SourceFolder, string.Empty);
                string line = lines[i];

                bool offense = false;
                offense |= line.Contains(".Save();") && !line.Contains("CanvasState.Save();", StringComparison.OrdinalIgnoreCase);
                offense |= line.Contains(".Restore();") && !line.Contains("CanvasState.Restore();", StringComparison.OrdinalIgnoreCase);

                if (!offense)
                {
                    continue;
                }

                offences++;
                errorMessages.Append(file2).Append(" line ").Append(i + 1).AppendLine();
                errorMessages.AppendLine(line.Trim());
                errorMessages.AppendLine();
            }
        }

        offences.Should()
                .Be(0,
                    "SKCanvas Save() and Restore() should never be called directly. "
                    + "Call RenderPack.CanvasState Save() and Restore() instead."
                    + $"{offences} offences:\n"
                    + $"{errorMessages}");
    }

    [Test]
    public void TestPrimitivesNamespaceIsNeverUsed()
    {
        Assert.That(Assembly.GetAssembly(typeof(Plot))?.GetTypes()
                            .FirstOrDefault(static x => x.Namespace is string ns && ns.Contains("ScottPlot.Primitives")), Is.Null);

        //Assembly.GetAssembly(typeof(Plot))?.GetTypes()
        //        .Where(static x => x.Namespace is string ns && ns.Contains("ScottPlot.Primitives"))
        //        .ToList()
        //        .ForEach(static t => Assert.Fail($"{t.Name} should be in the namespace ScottPlot (not ScottPlot.Primitives)"));
    }

    [Test]
    public void TestInterfacesNamespaceIsNeverUsed()
    {
        Assert.That(Assembly.GetAssembly(typeof(Plot))?.GetTypes()
                            .FirstOrDefault(static x => x.Namespace is string ns && ns.Contains("ScottPlot.Interfaces")), Is.Null);

        //Assembly.GetAssembly(typeof(Plot))?.GetTypes()
        //        .Where(static x => x.Namespace is string ns && ns.Contains("ScottPlot.Interfaces"))
        //        .ToList()
        //        .ForEach(static t => Assert.Fail($"{t.Name} should be in the namespace ScottPlot (not ScottPlot.Interfaces)"));
    }

    [Test]
    public void TestCanvasDoNotCallDrawText()
    {
        int offences = 0;
        StringBuilder errorMessages = new StringBuilder();

        foreach (string filePath in _sourceFilePaths)
        {
            if (Path.GetFileName(filePath) == "LabelStyle.cs")
            {
                continue;
            }

            if (Path.GetFileName(filePath) == "MeasuredText.cs")
            {
                continue;
            }

            string[] lines = File.ReadAllLines(filePath);

            for (int i = 0; i < lines.Length; i++)
            {
                string file2 = filePath.Replace(SourceCodeParsing.SourceFolder, string.Empty);
                string line = lines[i];

                if (line.Contains(".DrawText("))
                {
                    offences++;
                    errorMessages.Append(file2).Append(" line ").Append(i + 1).AppendLine();
                    errorMessages.AppendLine(line.Trim());
                    errorMessages.AppendLine();
                }
            }
        }

        offences.Should()
                .Be(0,
                    "SKCanvas.DrawText() must never be called directly."
                    + "Create a Label, style it as desired, and call it's Render() method."
                    + $"{offences} offences:\n"
                    + $"{errorMessages}");
    }

    [Test]
    public void TestPaintFontSpacing()
    {
        int offences = 0;
        StringBuilder errorMessages = new StringBuilder();

        foreach (string filePath in _sourceFilePaths)
        {
            if (Path.GetFileName(filePath) == "LabelStyle.cs")
            {
                continue;
            }

            string[] lines = File.ReadAllLines(filePath);

            for (int i = 0; i < lines.Length; i++)
            {
                string file2 = filePath.Replace(SourceCodeParsing.SourceFolder, string.Empty);
                string line = lines[i];

                if (line.Contains(".FontSpacing"))
                {
                    offences++;
                    errorMessages.Append(file2).Append(" line ").Append(i + 1).AppendLine();
                    errorMessages.AppendLine(line.Trim());
                    errorMessages.AppendLine();
                }
            }
        }

        offences.Should()
                .Be(0,
                    "SKPaint.FontSpacing must never be accessed."
                    + "Create a Label, style it as desired, use its Measure() method."
                    + $"{offences} offences:\n"
                    + $"{errorMessages}");
    }
}
