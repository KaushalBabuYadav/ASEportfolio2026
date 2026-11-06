namespace StarChart3D;

/// <summary>
/// Creates catalogue names for stars, such as "Tau Ceti", "61 Cygni A" or "Gl 845",
/// from the Bayer, Flamsteed, Gliese and Hipparcos values in the HYG data.
/// </summary>
/// <remarks>
/// Order of preference: Bayer letter + constellation, then Flamsteed number + constellation,
/// then Gliese ID, then Hipparcos number, then the HYG ID as a last resort.
/// Names use plain ASCII ("Alpha-2", "Bootis") so that Raylib's default font can draw them.
/// </remarks>
public static class CatalogueNames
{
    /// <summary>HYG three-letter Greek letter abbreviations and their full names.</summary>
    private static readonly Dictionary<string, string> GreekLetters = new()
    {
        ["Alp"] = "Alpha", ["Bet"] = "Beta", ["Gam"] = "Gamma", ["Del"] = "Delta",
        ["Eps"] = "Epsilon", ["Zet"] = "Zeta", ["Eta"] = "Eta", ["The"] = "Theta",
        ["Iot"] = "Iota", ["Kap"] = "Kappa", ["Lam"] = "Lambda", ["Mu"] = "Mu",
        ["Nu"] = "Nu", ["Xi"] = "Xi", ["Omi"] = "Omicron", ["Pi"] = "Pi",
        ["Rho"] = "Rho", ["Sig"] = "Sigma", ["Tau"] = "Tau", ["Ups"] = "Upsilon",
        ["Phi"] = "Phi", ["Chi"] = "Chi", ["Psi"] = "Psi", ["Ome"] = "Omega"
    };

    /// <summary>
    /// The 88 IAU constellation abbreviations and the Latin genitive used in star names
    /// (e.g. "Cet" → "Ceti"). The abbreviations are case-sensitive: "CMa" is not "Cma".
    /// </summary>
    private static readonly Dictionary<string, string> ConstellationGenitives = new()
    {
        ["And"] = "Andromedae", ["Ant"] = "Antliae", ["Aps"] = "Apodis", ["Aqr"] = "Aquarii",
        ["Aql"] = "Aquilae", ["Ara"] = "Arae", ["Ari"] = "Arietis", ["Aur"] = "Aurigae",
        ["Boo"] = "Bootis", ["Cae"] = "Caeli", ["Cam"] = "Camelopardalis", ["Cnc"] = "Cancri",
        ["CVn"] = "Canum Venaticorum", ["CMa"] = "Canis Majoris", ["CMi"] = "Canis Minoris",
        ["Cap"] = "Capricorni", ["Car"] = "Carinae", ["Cas"] = "Cassiopeiae", ["Cen"] = "Centauri",
        ["Cep"] = "Cephei", ["Cet"] = "Ceti", ["Cha"] = "Chamaeleontis", ["Cir"] = "Circini",
        ["Col"] = "Columbae", ["Com"] = "Comae Berenices", ["CrA"] = "Coronae Australis",
        ["CrB"] = "Coronae Borealis", ["Crv"] = "Corvi", ["Crt"] = "Crateris", ["Cru"] = "Crucis",
        ["Cyg"] = "Cygni", ["Del"] = "Delphini", ["Dor"] = "Doradus", ["Dra"] = "Draconis",
        ["Equ"] = "Equulei", ["Eri"] = "Eridani", ["For"] = "Fornacis", ["Gem"] = "Geminorum",
        ["Gru"] = "Gruis", ["Her"] = "Herculis", ["Hor"] = "Horologii", ["Hya"] = "Hydrae",
        ["Hyi"] = "Hydri", ["Ind"] = "Indi", ["Lac"] = "Lacertae", ["Leo"] = "Leonis",
        ["LMi"] = "Leonis Minoris", ["Lep"] = "Leporis", ["Lib"] = "Librae", ["Lup"] = "Lupi",
        ["Lyn"] = "Lyncis", ["Lyr"] = "Lyrae", ["Men"] = "Mensae", ["Mic"] = "Microscopii",
        ["Mon"] = "Monocerotis", ["Mus"] = "Muscae", ["Nor"] = "Normae", ["Oct"] = "Octantis",
        ["Oph"] = "Ophiuchi", ["Ori"] = "Orionis", ["Pav"] = "Pavonis", ["Peg"] = "Pegasi",
        ["Per"] = "Persei", ["Phe"] = "Phoenicis", ["Pic"] = "Pictoris", ["Psc"] = "Piscium",
        ["PsA"] = "Piscis Austrini", ["Pup"] = "Puppis", ["Pyx"] = "Pyxidis", ["Ret"] = "Reticuli",
        ["Sge"] = "Sagittae", ["Sgr"] = "Sagittarii", ["Sco"] = "Scorpii", ["Scl"] = "Sculptoris",
        ["Sct"] = "Scuti", ["Ser"] = "Serpentis", ["Sex"] = "Sextantis", ["Tau"] = "Tauri",
        ["Tel"] = "Telescopii", ["Tri"] = "Trianguli", ["TrA"] = "Trianguli Australis",
        ["Tuc"] = "Tucanae", ["UMa"] = "Ursae Majoris", ["UMi"] = "Ursae Minoris",
        ["Vel"] = "Velorum", ["Vir"] = "Virginis", ["Vol"] = "Volantis", ["Vul"] = "Vulpeculae"
    };

    /// <summary>
    /// Returns the best catalogue name for a star, ignoring its proper name.
    /// </summary>
    /// <example>Tau Ceti, Alpha-2 Centauri, 61 Cygni A, p Eridani B, Gl 845, HIP 1, HYG 0.</example>
    public static string Create(Star star)
    {
        string constellation = Genitive(star.Constellation);

        if (star.Bayer.Length > 0 && constellation.Length > 0)
        {
            return AddComponent($"{BayerName(star.Bayer)} {constellation}", star.GlieseId);
        }

        if (star.Flamsteed is int flamsteed && constellation.Length > 0)
        {
            return AddComponent($"{flamsteed} {constellation}", star.GlieseId);
        }

        if (star.GlieseId.Length > 0)
        {
            return star.GlieseId;
        }

        if (star.HipparcosId is int hip)
        {
            return $"HIP {hip}";
        }

        return $"HYG {star.Id}";
    }

    /// <summary>
    /// Turns a HYG Bayer value into a readable name: "Tau" → "Tau", "Alp-2" → "Alpha-2".
    /// Values that are not Greek letters (Bayer also used Latin letters, e.g. "p") are kept as they are.
    /// </summary>
    private static string BayerName(string bayer)
    {
        string letter = bayer;
        string suffix = string.Empty;

        int dash = bayer.IndexOf('-');
        if (dash > 0)
        {
            letter = bayer[..dash];
            suffix = bayer[dash..]; // e.g. "-2"
        }

        return (GreekLetters.TryGetValue(letter, out string? fullName) ? fullName : letter) + suffix;
    }

    /// <summary>Returns the genitive for a constellation abbreviation, the abbreviation itself if unknown, or "" if empty.</summary>
    private static string Genitive(string abbreviation)
    {
        if (abbreviation.Length == 0)
        {
            return string.Empty;
        }

        return ConstellationGenitives.TryGetValue(abbreviation, out string? genitive) ? genitive : abbreviation;
    }

    /// <summary>
    /// Adds the multiple-star component letter from the Gliese ID, e.g. "Gl 820A" → "61 Cygni A",
    /// so that both stars of a double star do not get the same name.
    /// </summary>
    private static string AddComponent(string name, string glieseId)
    {
        if (glieseId.Length >= 2 && char.IsUpper(glieseId[^1]) && char.IsDigit(glieseId[^2]))
        {
            return $"{name} {glieseId[^1]}";
        }

        return name;
    }
}
