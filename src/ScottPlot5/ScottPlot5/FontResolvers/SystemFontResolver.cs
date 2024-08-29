namespace ScottPlot.FontResolvers;

public class SystemFontResolver : IFontResolver
{
    private static HashSet<string> GetInstalledFonts() => new HashSet<string>(SKFontManager.Default.FontFamilies, StringComparer.InvariantCultureIgnoreCase);

    internal static string InstalledSansFont()
    {
        // Prefer the the system default because it is probably the best for international users
        // https://github.com/ScottPlot/ScottPlot/issues/2746
        string font = SKTypeface.Default.FamilyName;

        // Favor "Open Sans" over "Segoe UI" because better anti-aliasing
        HashSet<string> installedFonts = GetInstalledFonts();

        if (font == "Segoe UI" && installedFonts.Contains("Open Sans"))
        {
            font = "Open Sans";
        }

        return font;
    }

    internal static string InstalledMonospaceFont()
    {
        HashSet<string> installedFonts = GetInstalledFonts();

        string[] preferredFonts = ["Roboto Mono", "Consolas", "DejaVu Sans Mono", "Courier"];

        foreach (string preferredFont in preferredFonts.Where(installedFonts.Contains))
        {
            return SKTypeface.FromFamilyName(preferredFont).FamilyName;
        }

        return SKTypeface.Default.FamilyName;
    }

    internal static string InstalledSerifFont()
    {
        HashSet<string> installedFonts = GetInstalledFonts();

        string[] preferredFonts = ["Times New Roman", "DejaVu Serif", "Times"];

        foreach (string preferredFont in preferredFonts.Where(installedFonts.Contains))
        {
            return SKTypeface.FromFamilyName(preferredFont).FamilyName;
        }

        return SKTypeface.Default.FamilyName;
    }

    public static string DefaultSystemFont() => SKTypeface.Default.FamilyName;

    public static SKTypeface CreateDefaultTypeface()
    {
        string fontName = DefaultSystemFont();

        return SKTypeface.FromFamilyName(fontName)
               ?? throw new InvalidOperationException($"Unable to create typeface using the default system font ({fontName})");
    }

    public SKTypeface? CreateTypeface(string fontName, bool bold, bool italic)
    {
        if (!GetInstalledFonts().Contains(fontName))
        {
            return null;
        }

        SKFontStyleWeight weight = bold ? SKFontStyleWeight.Bold : SKFontStyleWeight.Normal;
        SKFontStyleSlant slant = italic ? SKFontStyleSlant.Italic : SKFontStyleSlant.Upright;
        const SKFontStyleWidth width = SKFontStyleWidth.Normal;
        SKFontStyle style = new SKFontStyle(weight, width, slant);

        return SKTypeface.FromFamilyName(fontName, style);
    }
}
