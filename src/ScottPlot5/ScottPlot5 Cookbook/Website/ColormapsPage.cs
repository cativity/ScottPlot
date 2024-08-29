using JetBrains.Annotations;

namespace ScottPlotCookbook.Website;

[UsedImplicitly]
internal class ColormapsPage : PageBase
{
    private static void MoveFront(List<IColormap> colormaps, string name)
    {
        for (int i = 0; i < colormaps.Count; i++)
        {
            IColormap colormap = colormaps[i];

            if (colormap.Name == name)
            {
                colormaps.RemoveAt(i);
                colormaps.Insert(0, colormap);

                return;
            }
        }
    }

    private static string GenerateColormapImage(IColormap colormap)
    {
        string className = colormap.ToString()?.Split(".")[^1] ?? string.Empty;
        string filename = $"Colormap_{className}.png";
        Console.WriteLine(filename);
        Image img = Colormap.GetImage(colormap, 1000, 100);
        string filePath = Path.Combine(Paths.OutputImageFolder, filename);
        img.SavePng(filePath);

        return filename;
    }

    public static void Generate(string folder)
    {
        StringBuilder sb = new StringBuilder();

        List<IColormap> colormaps = Colormap.GetColormaps().ToList();
        MoveFront(colormaps, "Turbo");
        MoveFront(colormaps, "Viridis");

        sb.AppendLine("# ScottPlot 5.0 Colormaps");
        sb.AppendLine();

        foreach (IColormap colormap in colormaps)
        {
            string filename = GenerateColormapImage(colormap);
            sb.Append("### ").AppendLine(colormap.Name);
            sb.AppendLine();
            sb.AppendLine("```cs");
            sb.Append("IColormap colormap = new ").Append(colormap).AppendLine("();");
            sb.AppendLine("```");
            sb.AppendLine();
            sb.Append("<img src='../images/").Append(filename).AppendLine("' class='w-100' height=100>");
            sb.AppendLine();
            sb.AppendLine();
        }

        string md = """
            ---
            title: Colormaps - ScottPlot 5.0
            description: Colormaps available in ScottPlot version 5.0
            url: /cookbook/5.0/colormaps/
            type: single
            BreadcrumbNames: ["ScottPlot 5.0 Cookbook", "Colormaps"]
            BreadcrumbUrls: ["/cookbook/5.0/", "/cookbook/5.0/colormaps/"]
            date: {{ DATE }}
            jsFiles: ["https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.9.0/highlight.min.js", "/js/cookbook-search-5.0.js"]
            ---

            {{ HTML }}

            """.Replace("{{ DATE }}", $"{DateTime.UtcNow:yyyy-MM-dd}")
               .Replace("{{ HTML }}", sb.ToString());

        string saveAs = Path.Combine(folder, "Colormaps.md");
        File.WriteAllText(saveAs, md);
    }
}
