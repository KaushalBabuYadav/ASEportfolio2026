using System.Collections;
using System.Globalization;
using System.Text;

namespace StarChart3D;

/// <summary>
/// All the stars read from a HYG star database CSV file.
/// Columns are found by their header names (e.g. "dist"), not by fixed positions,
/// so the code still works if the column order changes.
/// </summary>
public class StarCollection : IReadOnlyList<Star>
{
    /// <summary>The HYG columns this class reads. The file must contain all of them.</summary>
    private static readonly string[] RequiredColumns =
    {
        "id", "hip", "gl", "proper", "dist", "mag", "spect",
        "x", "y", "z", "bayer", "flam", "con"
    };

    private readonly List<Star> _stars = new();
    private readonly List<string> _skippedLines = new();

    /// <summary>
    /// Reads every star from a HYG CSV file. The first line must be the header row.
    /// Lines that cannot be read are skipped and listed in <see cref="SkippedLines"/>.
    /// </summary>
    /// <param name="csvFilePath">Path to the CSV file.</param>
    /// <exception cref="FileNotFoundException">The file does not exist.</exception>
    /// <exception cref="InvalidDataException">The file is empty or a required column is missing.</exception>
    public StarCollection(string csvFilePath)
    {
        if (!File.Exists(csvFilePath))
        {
            throw new FileNotFoundException("Star data file not found.", csvFilePath);
        }

        using var reader = new StreamReader(csvFilePath);

        string? headerLine = reader.ReadLine();
        if (headerLine is null)
        {
            throw new InvalidDataException($"Star data file is empty: {csvFilePath}");
        }

        string[] headers = SplitCsvLine(headerLine);
        Dictionary<string, int> columns = FindColumns(headers);

        int lineNumber = 1;
        string? line;
        while ((line = reader.ReadLine()) is not null)
        {
            lineNumber++;
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            string[] fields = SplitCsvLine(line);
            if (fields.Length != headers.Length)
            {
                _skippedLines.Add($"Line {lineNumber}: expected {headers.Length} values but found {fields.Length}.");
                continue;
            }

            try
            {
                _stars.Add(CreateStar(fields, columns));
            }
            catch (FormatException ex)
            {
                _skippedLines.Add($"Line {lineNumber}: {ex.Message}");
            }
        }
    }

    /// <summary>Number of stars that were read successfully.</summary>
    public int Count => _stars.Count;

    /// <summary>Gets the star at the given position (stars are kept in file order).</summary>
    public Star this[int index] => _stars[index];

    /// <summary>A message for every line that was skipped because it could not be read.</summary>
    public IReadOnlyList<string> SkippedLines => _skippedLines;

    /// <summary>Finds a star by its HYG database ID, or returns null if there is none.</summary>
    public Star? FindById(int id) => _stars.Find(star => star.Id == id);

    /// <summary>Returns an enumerator that goes through all the stars.</summary>
    public IEnumerator<Star> GetEnumerator() => _stars.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Splits one CSV line into values. Handles values in double quotes, such as "52" or "",
    /// including commas and doubled quotes ("") inside a quoted value.
    /// </summary>
    internal static string[] SplitCsvLine(string line)
    {
        var values = new List<string>();
        var current = new StringBuilder();
        bool inQuotes = false;

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (inQuotes)
            {
                if (c == '"' && i + 1 < line.Length && line[i + 1] == '"')
                {
                    current.Append('"'); // a doubled quote inside quotes is one quote character
                    i++;
                }
                else if (c == '"')
                {
                    inQuotes = false;
                }
                else
                {
                    current.Append(c);
                }
            }
            else if (c == '"')
            {
                inQuotes = true;
            }
            else if (c == ',')
            {
                values.Add(current.ToString());
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }

        values.Add(current.ToString());
        return values.ToArray();
    }

    /// <summary>Maps each required column name to its position in the header row.</summary>
    private static Dictionary<string, int> FindColumns(string[] headers)
    {
        var positions = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        for (int i = 0; i < headers.Length; i++)
        {
            positions[headers[i].Trim()] = i;
        }

        var missing = RequiredColumns.Where(name => !positions.ContainsKey(name)).ToList();
        if (missing.Count > 0)
        {
            throw new InvalidDataException("Star data file is missing columns: " + string.Join(", ", missing));
        }

        return positions;
    }

    /// <summary>Creates one <see cref="Star"/> from the values of one CSV line.</summary>
    /// <exception cref="FormatException">A number in the line is not valid.</exception>
    private static Star CreateStar(string[] fields, Dictionary<string, int> columns)
    {
        string Text(string column) => fields[columns[column]].Trim();

        return new Star
        {
            Id = ParseInt(Text("id"), "id"),
            HipparcosId = ParseOptionalInt(Text("hip"), "hip"),
            GlieseId = Text("gl"),
            ProperName = Text("proper"),
            DistanceParsecs = ParseDouble(Text("dist"), "dist"),
            Magnitude = ParseDouble(Text("mag"), "mag"),
            SpectralType = Text("spect"),
            X = ParseDouble(Text("x"), "x"),
            Y = ParseDouble(Text("y"), "y"),
            Z = ParseDouble(Text("z"), "z"),
            Bayer = Text("bayer"),
            Flamsteed = ParseOptionalInt(Text("flam"), "flam"),
            Constellation = Text("con")
        };
    }

    /// <summary>Reads a whole number that must be present.</summary>
    private static int ParseInt(string value, string column)
    {
        if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int result))
        {
            return result;
        }

        throw new FormatException($"column '{column}' is not a whole number: '{value}'.");
    }

    /// <summary>Reads a whole number that may be missing (empty), returning null if it is.</summary>
    private static int? ParseOptionalInt(string value, string column)
    {
        return value.Length == 0 ? null : ParseInt(value, column);
    }

    /// <summary>
    /// Reads a decimal number that must be present. Always uses "." as the decimal point
    /// (invariant culture), whatever the computer's language settings are.
    /// </summary>
    private static double ParseDouble(string value, string column)
    {
        if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out double result))
        {
            return result;
        }

        throw new FormatException($"column '{column}' is not a number: '{value}'.");
    }
}
