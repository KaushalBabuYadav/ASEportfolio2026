namespace StarChart3D;

/// <summary>
/// One star from the HYG v4.4 star database (one row of the CSV file).
/// Positions and distances are in parsecs, with the Sun at (0, 0, 0).
/// </summary>
public class Star
{
    /// <summary>Number of light years in one parsec.</summary>
    public const double LightYearsPerParsec = 3.26156;

    /// <summary>HYG database ID (column "id"). The Sun is 0.</summary>
    public int Id { get; init; }

    /// <summary>Proper (common) name, e.g. "Sirius" (column "proper"). Empty if the star has none.</summary>
    public string ProperName { get; init; } = string.Empty;

    /// <summary>Hipparcos catalogue number (column "hip"), or null if the star has none.</summary>
    public int? HipparcosId { get; init; }

    /// <summary>Gliese catalogue ID, e.g. "Gl 551" (column "gl"). Empty if the star has none.</summary>
    public string GlieseId { get; init; } = string.Empty;

    /// <summary>Bayer designation, e.g. "Alp" or "Alp-2" (column "bayer"). Empty if the star has none.</summary>
    public string Bayer { get; init; } = string.Empty;

    /// <summary>Flamsteed number, e.g. 61 for 61 Cygni (column "flam"), or null if the star has none.</summary>
    public int? Flamsteed { get; init; }

    /// <summary>Three-letter constellation abbreviation, e.g. "Cen" (column "con"). Empty if unknown.</summary>
    public string Constellation { get; init; } = string.Empty;

    /// <summary>Distance from the Sun in parsecs (column "dist").</summary>
    public double DistanceParsecs { get; init; }

    /// <summary>X position in parsecs (column "x"). +X points towards the vernal equinox.</summary>
    public double X { get; init; }

    /// <summary>Y position in parsecs (column "y"). +Y points towards right ascension 6 hours.</summary>
    public double Y { get; init; }

    /// <summary>Z position in parsecs (column "z"). +Z points towards the north celestial pole.</summary>
    public double Z { get; init; }

    /// <summary>Apparent visual magnitude as seen from Earth (column "mag"). Smaller numbers are brighter.</summary>
    public double Magnitude { get; init; }

    /// <summary>Spectral type, e.g. "G2V" (column "spect"). Empty if unknown.</summary>
    public string SpectralType { get; init; } = string.Empty;

    /// <summary>Distance from the Sun in light years.</summary>
    public double DistanceLightYears => DistanceParsecs * LightYearsPerParsec;

    /// <summary>True if the star has a proper (common) name.</summary>
    public bool HasProperName => !string.IsNullOrWhiteSpace(ProperName);

    private string? _catalogueName;

    /// <summary>
    /// Catalogue name made from the star's catalogue IDs, e.g. "Tau Ceti", "61 Cygni A" or "Gl 845".
    /// Created the first time it is needed and then remembered.
    /// </summary>
    public string CatalogueName => _catalogueName ??= CatalogueNames.Create(this);

    /// <summary>The name to show for the star: its proper name if it has one, otherwise its catalogue name.</summary>
    public string DisplayName => HasProperName ? ProperName : CatalogueName;

    /// <summary>Returns the star's name and distance, for debugging.</summary>
    public override string ToString() => $"{DisplayName} ({DistanceLightYears:F2} ly)";
}