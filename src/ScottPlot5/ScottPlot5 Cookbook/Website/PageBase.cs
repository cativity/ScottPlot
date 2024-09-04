using Version = ScottPlot.Version;

namespace ScottPlotCookbook.Website;

internal abstract class PageBase
{
    protected readonly StringBuilder Sb = new StringBuilder();

    public void AddVersionInformation()
    {
        /*
        string alertHtml = "\n\n" +
            "<div class='alert alert-warning' role='alert'>" +
            "<h4 class='alert-heading py-0 my-0'>" +
            $"⚠️ ScottPlot {ScottPlot.Version.VersionString} is a preview package" +
            "</h4>" +
            "<hr />" +
            "<p class='mb-0'>" +
            "<span class='fw-semibold'>This page describes a beta release of ScottPlot.</span> " +
            "It is available on NuGet as a preview package, but its API is not stable " +
            "and it is not recommended for production use. " +
            "See the <a href='https://scottplot.net/versions/'>ScottPlot Versions</a> page for more information. " +
            "</p>" +
            "</div>" +
            "\n\n";
        SB.AppendLine(alertHtml);
        */

        /*
        SB.AppendLine();
        SB.AppendLine("{{< banner-sp5 >}}");
        SB.AppendLine();
        */
    }

    public void Save(string folder, string title, string description, string filename, string url, string[]? frontmatter)
    {
        Directory.CreateDirectory(folder);

        if (!url.EndsWith('/'))
        {
            url += "/";
        }

        StringBuilder sbfm = new StringBuilder();
        sbfm.AppendLine("---");
        sbfm.Append("Title: ").AppendLine(title);
        sbfm.Append("Description: ").AppendLine(description);
        sbfm.Append("URL: ").AppendLine(url);

        if (frontmatter is not null)
        {
            foreach (string line in frontmatter)
            {
                sbfm.AppendLine(line);
            }
        }

        sbfm.Append("Date: ").AppendLine($"{DateTime.UtcNow:yyyy-MM-dd}");
        sbfm.Append("Version: ").AppendLine(Version.LongString);
        sbfm.Append("Version: ").AppendLine(Version.LongString);
        sbfm.AppendLine("SearchUrl: \"/cookbook/5.0/search/\"");
        sbfm.AppendLine("ShowEditLink: false");
        sbfm.AppendLine("---");
        sbfm.AppendLine();

        string md = sbfm.Append(Sb).ToString();

        string saveAs = Path.Combine(folder, filename);
        File.WriteAllText(saveAs, md);
    }
}
