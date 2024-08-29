using System.Diagnostics;

namespace ScottPlotTests.FontTests;

internal static class FontDetectionTests
{
    [Test]
    public static void ConvertStringToTextElements_UnitTests()
    {
        List<string> testcases = ["𝓦", "á", "🌹", "👩🏽‍🚒", "已", "a", "𝓦á🌹👩🏽‍🚒已"];
        List<List<string>> expectedResults = [["𝓦"], ["á"], ["🌹"], ["👩🏽‍🚒"], ["已"], ["a"], ["𝓦", "á", "🌹", "👩🏽‍🚒", "已"]];

        foreach ((string? testcase, List<string>? expected) in testcases.Zip(expectedResults))
        {
            List<string> res = Fonts.ConvertStringToTextElements(testcase);

            foreach ((string? resVal, string? expVal) in res.Zip(expected))
            {
                if (resVal != expVal)
                {
                    throw new InvalidOperationException($"ConvertStringToTextElementList_UnitTests() error for '{testcase}'");
                }
            }

            string resStr = string.Join(", ", res.Select(static x => $"'{x}'"));
            Debug.WriteLine($"Test: Converted '{testcase}' to text elements = {resStr}");
        }
    }

    [Test]
    public static void GetStandaloneCodePoints_UnitTests()
    {
        //List<List<string>> expectedResults = [["𝓦"], [], ["🌹"], [], ["已"], ["a"], ["𝓦"]];

        foreach (string? testcase in (List<string>)(["𝓦", "á", "🌹", "👩🏽‍🚒", "已", "a", "𝓦á"]))
        {
            List<string> testcaseTextElements = Fonts.ConvertStringToTextElements(testcase);
            List<List<int>> testcaseCodePoints = testcaseTextElements.ConvertAll(Fonts.ConvertTextElementToUtf32CodePoints);
            List<int> res = Fonts.GetStandaloneCodePoints(testcaseCodePoints);

            // TODO: Add automatic checking, was done manually

            string inpCodePointsStr = string.Join(", ", testcaseCodePoints.Select(static x => "[" + string.Join(", ", x.Select(static y => $"0x{y:X08}")) + "]"));
            string resCodePointsStr = string.Join(", ", res.Select(static x => $"0x{x:X08}"));
            Debug.WriteLine($"Test: Input string '{testcase}' has these standalone code points = {resCodePointsStr} from {inpCodePointsStr}");
        }
    }

    [Test]
    public static void ConvertTextElementToUtf32CodePoints_UnitTests()
    {
        foreach (string? testcase in (List<string>)(["𝓦", "á", "🌹", "👩🏽‍🚒", "已", "a"]))
        {
            List<int> res = Fonts.ConvertTextElementToUtf32CodePoints(testcase);

            if (string.Concat(res.Select(char.ConvertFromUtf32)) == testcase)
            {
                string codePointsStr = string.Join(", ", res.Select(static x => $"0x{x:X08}"));
                Debug.WriteLine($"Test: Converted '{testcase}' to Utf32 code points = {codePointsStr}");
            }
            else
            {
                throw new InvalidOperationException($"ConvertTextElementToUtf32CodePoints_UnitTests() error for '{testcase}'");
            }
        }
    }
}
