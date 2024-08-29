using ScottPlot.Testing;
using Image = ScottPlot.Image;

namespace GraphicalTestRunner;

public class FolderComparisonResults
{
    public string BeforeFolder { get; }

    public string AfterFolder { get; }

    public string[] Filenames { get; }

    public string[] Summaries { get; }

    public ImageDiff?[] ImageDiffs { get; }

    public FolderComparisonResults(string before, string after)
    {
        BeforeFolder = before;
        AfterFolder = after;
        string[] beforePaths = Directory.GetFiles(before, "*.png");
        string[] afterPaths = Directory.GetFiles(after, "*.png");

        HashSet<string> allFilenames = [];
        allFilenames.UnionWith(beforePaths.Select(Path.GetFileName).OfType<string>());
        allFilenames.UnionWith(afterPaths.Select(Path.GetFileName).OfType<string>());
        Filenames = allFilenames.Order().ToArray();

        Summaries = new string[Filenames.Length];
        ImageDiffs = new ImageDiff?[Filenames.Length];
    }

    public string GetPath1(int i) => Path.GetFullPath(BeforeFolder + "/" + Filenames[i]);

    public string GetPath2(int i) => Path.GetFullPath(AfterFolder + "/" + Filenames[i]);

    public void Analyze(int i)
    {
        string pathBefore = GetPath1(i);
        string pathAfter = GetPath2(i);

        bool img1Exists = File.Exists(pathBefore);
        bool img2Exists = File.Exists(pathAfter);

        if (img1Exists && img2Exists)
        {
            Image img1 = new Image(pathBefore);
            Image img2 = new Image(pathAfter);

            if (img1.Size != img2.Size)
            {
                Summaries[i] = "resized";

                return;
            }

            ImageDiff imageDiff = new ImageDiff(img1, img2, false);
            ImageDiffs[i] = imageDiff;
            Summaries[i] = imageDiff.TotalDifference == 0 ? "unchanged" : "changed";
        }
        else if (img1Exists && !img2Exists)
        {
            Summaries[i] = "deleted";
        }
        else if (!img1Exists && img2Exists)
        {
            Summaries[i] = "added";
        }
        else
        {
            throw new InvalidOperationException("neither file exists");
        }
    }
}
