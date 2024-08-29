namespace ScottPlot;

internal static class ColorByteExtensions
{
    public static byte Lighten(this byte color, double fraction)
    {
        double fColor = color + ((255 - color) * fraction);

        return (byte)Math.Min(Math.Max(fColor, 0), 255);
    }

    public static byte Darken(this byte color, double fraction)
    {
        double fColor = color * fraction;

        return (byte)Math.Min(Math.Max(fColor, 0), 255);
    }
}

public readonly struct Color
{
    public readonly byte Red;
    public readonly byte Green;
    public readonly byte Blue;
    public readonly byte Alpha;

    // TODO: benchmark if referencing these is slower
    public byte R => Red;

    public byte G => Green;

    public byte B => Blue;

    public byte A => Alpha;

    public uint ARGB => ((uint)Alpha << 24) | ((uint)Red << 16) | ((uint)Green << 8) | ((uint)Blue << 0);

    public override string ToString() => $"Color {ToHex()} (R={R}, G={G}, B={B}, A={A})";

    public Color(byte red, byte green, byte blue, byte alpha = 255)
    {
        Red = red;
        Green = green;
        Blue = blue;
        Alpha = alpha;
    }

    public Color(float red, float green, float blue, float alpha = 1)
    {
        Red = (byte)(red * 255);
        Green = (byte)(green * 255);
        Blue = (byte)(blue * 255);
        Alpha = (byte)(alpha * 255);
    }

    public Color(string hexCode)
        : this(Hex2Argb(hexCode))
    {
    }

    public Color(uint argb)
    {
        Alpha = (byte)(argb >> 24);
        Red = (byte)(argb >> 16);
        Green = (byte)(argb >> 8);
        Blue = (byte)(argb >> 0);
    }

    public static bool operator ==(Color a, Color b) => a.ARGB == b.ARGB;

    public static bool operator !=(Color a, Color b) => a.ARGB != b.ARGB;

    public override int GetHashCode() => (int)ARGB;

    public override bool Equals(object? obj)
    {
        return obj is Color color && color.ARGB == ARGB;
    }

    public Color WithRed(byte red) => new Color(red, Green, Blue, Alpha);

    public Color WithGreen(byte green) => new Color(Red, green, Blue, Alpha);

    public Color WithBlue(byte blue) => new Color(Red, Green, blue, Alpha);

    public Color WithAlpha(byte alpha)
    {
        // If requesting a semitransparent black, make it slightly non-black
        // to prevent SVG export from rendering the color as opaque.
        // https://github.com/ScottPlot/ScottPlot/issues/3063
        if (Red == 0 && Green == 0 && Blue == 0 && alpha < 255)
        {
            return new Color(1, 1, 1, alpha);
        }

        return new Color(Red, Green, Blue, alpha);
    }

    public Color WithAlpha(double alpha) => WithAlpha((byte)(alpha * 255));

    public Color WithOpacity(double opacity = .5) => WithAlpha((byte)(opacity * 255));

    public static Color Gray(byte value) => new Color(value, value, value);

    public static Color FromARGB(int argb) => FromARGB((uint)argb);

    public static Color FromARGB(uint argb) => new Color(argb);

    public static Color FromHex(string hex) => new Color(hex);

    //Returns ARGB value from hex string
    private static uint Hex2Argb(string hex)
    {
        if (hex.StartsWith('#'))
        {
            hex = hex.TrimStart('#');
        }

        if (hex.Length == 6)
        {
            hex += "FF";
        }

        if (uint.TryParse(hex, NumberStyles.HexNumber, null, out uint rgba))
        {
            return ((rgba & 0xFF) << 24) | (rgba >> 8);
        }

        return 0U;
    }

    public static Color[] FromHex(string[] hex) => hex.Select(FromHex).ToArray();

    public static Color FromColor(System.Drawing.Color color) => new Color(color.R, color.G, color.B, color.A);

    public string ToHex() => Alpha == 255 ? $"#{R:X2}{G:X2}{B:X2}" : $"#{R:X2}{G:X2}{B:X2}{A:X2}";

    public static Color FromSKColor(SKColor skcolor) => new Color(skcolor.Red, skcolor.Green, skcolor.Blue, skcolor.Alpha);

    public string ToStringRGB() => "#" + Red.ToString("X2") + Green.ToString("X2") + Blue.ToString("X2");

    public string ToStringRGBA() => "#" + Red.ToString("X2") + Green.ToString("X2") + Blue.ToString("X2") + Alpha.ToString("X2");

    public SKColor ToSKColor() => new SKColor(Red, Green, Blue, Alpha);

    /// <summary>
    /// Luminance as a fraction from 0 to 1
    /// </summary>
    public float Luminance => ToHSL().l;

    /// <summary>
    /// Hue as a fraction from 0 to 1
    /// </summary>
    public float Hue => ToHSL().h;

    /// <summary>
    /// Saturation as a fraction from 0 to 1
    /// </summary>
    public float Saturation => ToHSL().s;

    /// <summary>
    /// Hue, Saturation, and Luminance (as fractions from 0 to 1)
    /// </summary>
    public (float h, float s, float l) ToHSL()
    {
        // adapted from http://en.wikipedia.org/wiki/HSL_color_space
        float r = Red / 255f;
        float g = Green / 255f;
        float b = Blue / 255f;

        float max = Math.Max(Math.Max(r, g), b);
        float min = Math.Min(Math.Min(r, g), b);
        float l = (min + max) / 2.0f;

        if (l <= 0.0)
        {
            return (0, 0, 0);
        }

        float delta = max - min;
        float s = delta;

        if (s <= 0.0)
        {
            return (0, 0, l);
        }

        s = l > 0.5f ? delta / (2.0f - delta) : delta / (max + min);

        float h;

        if (r > g && r > b)
        {
            h = ((g - b) / delta) + (g < b ? 6.0f : 0.0f);
        }
        else if (g > b)
        {
            h = ((b - r) / delta) + 2.0f;
        }
        else
        {
            h = ((r - g) / delta) + 4.0f;
        }

        h /= 6.0f;

        return (h, s, l);
    }

    /// <summary>
    /// Create a Color given Hue, Saturation, and Luminance (as fractions from 0 to 1)
    /// </summary>
    public static Color FromHSL(float hue, float saturation, float luminosity, float alpha = 1)
    {
        // adapted from Microsoft.Maui.Graphics/Color.cs (MIT license)

        if (luminosity == 0)
        {
            return new Color(0, 0, 0);
        }

        if (saturation == 0)
        {
            return new Color(luminosity, luminosity, luminosity);
        }

        float temp2 = luminosity <= 0.5f ? luminosity * (1.0f + saturation) : luminosity + saturation - (luminosity * saturation);
        float temp1 = (2.0f * luminosity) - temp2;

        float[] t3 = [hue + (1.0f / 3.0f), hue, hue - (1.0f / 3.0f)];
        float[] clr = [0, 0, 0];

        for (int i = 0; i < 3; i++)
        {
            if (t3[i] < 0)
            {
                t3[i]++;
            }

            if (t3[i] > 1)
            {
                t3[i]--;
            }

            if (6.0 * t3[i] < 1.0)
            {
                clr[i] = temp1 + ((temp2 - temp1) * t3[i] * 6.0f);
            }
            else if (2.0 * t3[i] < 1.0)
            {
                clr[i] = temp2;
            }
            else if (3.0 * t3[i] < 2.0)
            {
                clr[i] = temp1 + ((temp2 - temp1) * ((2.0f / 3.0f) - t3[i]) * 6.0f);
            }
            else
            {
                clr[i] = temp1;
            }
        }

        return new Color(clr[0], clr[1], clr[2], alpha);
    }

    public Color WithLightness(float lightness = .5f)
    {
        (float h, float s, float _) = ToHSL();

        return FromHSL(h, s, Math.Min(Math.Max(lightness, 0f), 1f));
    }

    /// <summary>
    ///     Amount to lighten the color (from 0-1).
    ///     Larger numbers produce lighter results.
    /// </summary>
    public Color Lighten(double fraction = .5f)
    {
        if (fraction < 0)
        {
            return Darken(-fraction);
        }

        fraction = Math.Min(1f, fraction);

        return new Color(R.Lighten(fraction), G.Lighten(fraction), B.Lighten(fraction), Alpha);
    }

    /// <summary>
    ///     Amount to darken the color (from 0-1).
    ///     Larger numbers produce darker results.
    /// </summary>
    public Color Darken(double fraction = .5f)
    {
        if (fraction < 0)
        {
            return Lighten(-fraction);
        }

        fraction = Math.Max(0f, 1f - fraction);

        return new Color(R.Darken(fraction), G.Darken(fraction), B.Darken(fraction), Alpha);
    }

    /// <summary>
    ///     Return this color mixed with another color.
    /// </summary>
    /// <param name="otherColor">Color to mix with this color</param>
    /// <param name="fraction">Fraction of <paramref name="otherColor" /> to use</param>
    /// <returns></returns>
    public Color MixedWith(Color otherColor, double fraction) => InterpolateRgb(otherColor, fraction);

    public Color InterpolateRgb(Color c1, double factor) => InterpolateRgb(this, c1, factor);

    public Color[] InterpolateArrayRgb(Color c1, int steps) => InterpolateRgbArray(this, c1, steps);

    private static byte InterpolateRgb(byte b1, byte b2, double factor)
    {
        if (b1 < b2)
        {
            return Math.Min(Math.Max((byte)(b1 + ((b2 - b1) * factor)), (byte)0), (byte)255);
        }

        return Math.Min(Math.Max((byte)(b2 + ((b1 - b2) * (1 - factor))), (byte)0), (byte)255);
    }

    public static Color InterpolateRgb(Color c1, Color c2, double factor)
        => new Color(InterpolateRgb(c1.R, c2.R, factor),
                     InterpolateRgb(c1.G, c2.G, factor),
                     InterpolateRgb(c1.B, c2.B, factor),
                     InterpolateRgb(c1.A, c2.A, factor));

    public static Color[] InterpolateRgbArray(Color c1, Color c2, int steps)
    {
        double stepFactor = 1.0 / (steps - 1);
        Color[] array = new Color[steps];

        for (int i = 0; i < steps; ++i)
        {
            array[i] = InterpolateRgb(c1, c2, stepFactor * i);
        }

        return array;
    }

    public uint PremultipliedARGB
    {
        get
        {
            byte premultipliedRed = (byte)(Red * Alpha / 255);
            byte premultipliedGreen = (byte)(Green * Alpha / 255);
            byte premultipliedBlue = (byte)(Blue * Alpha / 255);

            return ((uint)Alpha << 24) | ((uint)premultipliedRed << 16) | ((uint)premultipliedGreen << 8) | ((uint)premultipliedBlue << 0);
        }
    }

    public static Color RandomHue()
    {
        float hue = (float)Generate.RandomNumber();
        const float saturation = 1;
        const float luminosity = 0.5f;

        return FromHSL(hue, saturation, luminosity);
    }

    public static System.Drawing.Color ToColor(Color color) => System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
}
