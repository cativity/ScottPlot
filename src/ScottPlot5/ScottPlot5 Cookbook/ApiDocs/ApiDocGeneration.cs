namespace ScottPlotCookbook.ApiDocs;

internal class ApiDocGeneration
{
    [Test]
    public void TestDocs()
    {
        string xmlFilePath = Paths.GetScottPlotXmlFilePath();
        ApiDocs docs = new ApiDocs(typeof(Plot), xmlFilePath);

        string savePath = Path.Combine(Paths.OutputFolder, "API.md");
        File.WriteAllText(savePath, docs.GetMarkdown());
        Console.WriteLine(savePath);
    }
}
