using JetBrains.Annotations;

namespace ScottPlotCookbook.Website;

[UsedImplicitly]
internal class PalettesPage : PageBase
{
    private static void MoveFront(List<IPalette> palettes, string name)
    {
        for (int i = 0; i < palettes.Count; i++)
        {
            IPalette pal = palettes[i];

            if (pal.Name == name)
            {
                palettes.RemoveAt(i);
                palettes.Insert(0, pal);

                return;
            }
        }
    }

    public static void Generate(string folder)
    {
        StringBuilder sb = new StringBuilder();

        List<IPalette> palettes = Palette.GetPalettes().ToList();
        MoveFront(palettes, "Category 20");
        MoveFront(palettes, "Category 10");

        sb.AppendLine("# ScottPlot 5.0 Color Palettes");
        sb.AppendLine();

        foreach (IPalette palette in palettes)
        {
            sb.Append("## ").AppendLine(palette.Name);
            sb.AppendLine();
            sb.AppendLine(palette.Description);
            sb.AppendLine();
            sb.AppendLine("```cs");
            sb.Append("IPalette palette = new ").Append(palette).AppendLine("();");
            sb.AppendLine("```");
            sb.AppendLine();

            for (int i = 0; i < palette.Colors.Length; i++)
            {
                Color color = palette.Colors[i];
                string textColor = color.Luminance > .5 ? "black" : "white";

                sb.Append("<div class='px-3 py-2 fw-semibold' style='background-color: ")
                  .Append(color.ToHex())
                  .Append("; color: ")
                  .Append(textColor)
                  .Append("'>palette.GetColor(")
                  .Append(i)
                  .Append(") returns ")
                  .Append(color.ToHex())
                  .AppendLine("</div>");
            }

            sb.AppendLine();
            sb.AppendLine();
        }

        string md = """
            ---
            title: Color Palettes - ScottPlot 5.0
            description: Color palettes available in ScottPlot version 5.0
            url: /cookbook/5.0/palettes/
            type: single
            BreadcrumbNames: ["ScottPlot 5.0 Cookbook", "Palettes"]
            BreadcrumbUrls: ["/cookbook/5.0/", "/cookbook/5.0/palettes/"]
            date: {{ DATE }}
            jsFiles: ["https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.9.0/highlight.min.js", "/js/cookbook-search-5.0.js"]
            ---

            {{ HTML }}

            """.Replace("{{ DATE }}", $"{DateTime.UtcNow:yyyy-MM-dd}")
               .Replace("{{ HTML }}", sb.ToString());

        string saveAs = Path.Combine(folder, "Palettes.md");
        File.WriteAllText(saveAs, md);
    }
}
