using JetBrains.Annotations;
using ScottPlot.Plottables;

namespace ScottPlotCookbook.Recipes.Miscellaneous;

[UsedImplicitly]
public class Internationalization : ICategory
{
    public string Chapter => "Miscellaneous";

    public string CategoryName => "Internationalization";

    public string CategoryDescription => "Using ScottPlot across cultures with different text and numeric requirements.";

    public class FontDetection : RecipeBase
    {
        public override string Name => "Supported Font Detection";

        public override string Description
            => "ScottPlot comes with font detection methods which help identify "
               + "the best installed font for displaying text which may contain international characters.";

        [Test]
        public override void Execute()
        {
            const string chinese = "测试";
            MyPlot.Axes.Title.Label.Text = chinese;
            MyPlot.Axes.Title.Label.FontName = Fonts.Detect(chinese);

            const string japanese = "試験";
            MyPlot.Axes.Left.Label.Text = japanese;
            MyPlot.Axes.Left.Label.FontName = Fonts.Detect(japanese);

            const string korean = "테스트";
            MyPlot.Axes.Bottom.Label.Text = korean;
            MyPlot.Axes.Bottom.Label.FontName = Fonts.Detect(korean);
        }
    }

    public class AutomaticFontDetection : RecipeBase
    {
        public override string Name => "Automatic Font Detection";

        public override string Description
            => "The Plot's Style class contains a method "
               + "which automatically sets the fonts of common plot objects to the font "
               + "most likely able to display the characters they contain.";

        [Test]
        public override void Execute()
        {
            Signal sig1 = MyPlot.Add.Signal(Generate.Sin(phase: .1));
            Signal sig2 = MyPlot.Add.Signal(Generate.Sin(phase: .2));
            Signal sig3 = MyPlot.Add.Signal(Generate.Sin(phase: .3));

            sig1.LegendText = "测试"; // Chinese
            sig2.LegendText = "試験"; // Japanese
            sig3.LegendText = "테스트"; // Korean
            MyPlot.ShowLegend();

            MyPlot.Title("测试"); // Chinese
            MyPlot.YLabel("試験"); // Japanese
            MyPlot.XLabel("테스트"); // Korean

            MyPlot.Font.Automatic(); // set font for each item based on its content
        }
    }
}
