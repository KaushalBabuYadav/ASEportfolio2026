using Raylib_cs;

namespace StarChart3D;

/// <summary>
/// The main spectral classes of stars, from hottest (O, blue) to coolest (M, red),
/// plus white dwarfs and stars whose spectral type is unknown.
/// </summary>
public enum SpectralClass
{
    O, B, A, F, G, K, M, WhiteDwarf, Unknown
}

/// <summary>Works out a star's <see cref="SpectralClass"/> from its HYG "spect" text, and the colour used to draw it.</summary>
public static class SpectralClasses
{
    /// <summary>
    /// Display colours for each class. They follow the real order (hot stars blue-white, cool stars orange-red)
    /// but are made more saturated than true star colours, so that the classes are easy to tell apart on screen.
    /// </summary>
    private static readonly Dictionary<SpectralClass, Color> Colours = new()
    {
        [SpectralClass.O] = new Color(110, 140, 255, 255),
        [SpectralClass.B] = new Color(150, 180, 255, 255),
        [SpectralClass.A] = new Color(215, 225, 255, 255),
        [SpectralClass.F] = new Color(255, 255, 230, 255),
        [SpectralClass.G] = new Color(255, 235, 150, 255),
        [SpectralClass.K] = new Color(255, 180, 95, 255),
        [SpectralClass.M] = new Color(255, 110, 80, 255),
        [SpectralClass.WhiteDwarf] = new Color(200, 230, 255, 255),
        [SpectralClass.Unknown] = new Color(130, 130, 130, 255)
    };

    /// <summary>Order in which the classes are shown in the on-screen colour key.</summary>
    public static readonly SpectralClass[] LegendOrder =
    {
        SpectralClass.O, SpectralClass.B, SpectralClass.A, SpectralClass.F, SpectralClass.G,
        SpectralClass.K, SpectralClass.M, SpectralClass.WhiteDwarf, SpectralClass.Unknown
    };

    /// <summary>Returns the colour used to draw stars of the given class.</summary>
    public static Color ColourOf(SpectralClass spectralClass) => Colours[spectralClass];

    /// <summary>Returns a short name for the colour key, e.g. "M" or "White dwarf".</summary>
    public static string NameOf(SpectralClass spectralClass) => spectralClass switch
    {
        SpectralClass.WhiteDwarf => "White dwarf",
        SpectralClass.Unknown => "Unknown",
        _ => spectralClass.ToString()
    };

    /// <summary>
    /// Works out the spectral class from HYG "spect" text. Handles the different styles found in the data:
    /// "G2V", "M5Ve" (normal); "dM4", "sdM4", "sdG0" (older dwarf/subdwarf prefixes in lower case);
    /// "m", "k", "k-m" (lower-case estimates – the first letter is used); "DA7", "DC9" (white dwarfs);
    /// "Se" (S-type, a cool star treated as M); and "" or unclear codes such as "g" or "pec" (unknown).
    /// </summary>
    public static SpectralClass Parse(string spectralType)
    {
        string text = spectralType.Trim();
        if (text.Length == 0)
        {
            return SpectralClass.Unknown;
        }

        if (text[0] == 'D')
        {
            return SpectralClass.WhiteDwarf;
        }

        // Lower-case only (e.g. "m", "k-m"): an estimate. Only "k" and "m" are used this way in the data;
        // other lower-case codes such as "g" (giant, no class given) or "pec" (peculiar) are unclear.
        if (!text.Any(char.IsUpper))
        {
            return text[0] switch
            {
                'k' => SpectralClass.K,
                'm' => SpectralClass.M,
                _ => SpectralClass.Unknown
            };
        }

        // Skip lower-case prefixes such as "d" (dwarf) or "sd" (subdwarf) and use the first capital letter.
        char letter = text.First(char.IsUpper);
        return letter switch
        {
            'O' => SpectralClass.O,
            'B' => SpectralClass.B,
            'A' => SpectralClass.A,
            'F' => SpectralClass.F,
            'G' => SpectralClass.G,
            'K' => SpectralClass.K,
            'M' or 'S' => SpectralClass.M,
            _ => SpectralClass.Unknown
        };
    }
}
