using Raylib_cs;

namespace StarChart3D;

/// <summary>
/// An on-screen table of the stars nearest to the current location, with the
/// distance (light years), bearing (degrees) and path (degrees) to each one.
/// </summary>
public class NearbyStarsPanel
{
    /// <summary>How many nearby stars are listed.</summary>
    public const int StarCount = 10;

    private const int Width = 470;
    private const int RowHeight = 22;
    private const int FontSize = 16;
    private const int Top = 236;              // just below the spectral class key
    private const int RightMargin = 10;

    private readonly IReadOnlyList<Star> _allStars;
    private IReadOnlyList<Course> _courses = Array.Empty<Course>();

    /// <summary>Creates the panel and works out the list for the starting star.</summary>
    public NearbyStarsPanel(IReadOnlyList<Star> allStars, Star startingStar)
    {
        _allStars = allStars;
        SetLocation(startingStar);
    }

    /// <summary>The star the list is measured from.</summary>
    public Star Location { get; private set; } = null!;

    /// <summary>The courses currently listed, nearest first.</summary>
    public IReadOnlyList<Course> Courses => _courses;

    /// <summary>Changes the current location and recalculates the list of nearest stars.</summary>
    public void SetLocation(Star star)
    {
        Location = star;
        _courses = Course.ToNearest(star, _allStars, StarCount);
    }

    /// <summary>Draws the table in the right-hand side of the window.</summary>
    public void Draw()
    {
        int x = Raylib.GetScreenWidth() - Width - RightMargin;
        int height = 58 + _courses.Count * RowHeight;
        Raylib.DrawRectangle(x, Top, Width, height, new Color(0, 0, 0, 225));

        Raylib.DrawText($"Nearest stars to {Location.DisplayName}", x + 10, Top + 8, 18, Color.RayWhite);

        int headerY = Top + 32;
        DrawRow(x, headerY, "#", "Star", "Dist (ly)", "Bearing", "Path", Color.Gray);

        for (int i = 0; i < _courses.Count; i++)
        {
            Course course = _courses[i];
            int rowY = headerY + (i + 1) * RowHeight;
            DrawRow(
                x,
                rowY,
                ((i + 1) % 10).ToString(),           // 1–9, then 0 for the tenth star (matches the number keys)
                Shorten(course.Destination.DisplayName, 16),
                $"{course.DistanceLightYears:F2}",
                $"{course.BearingDegrees:F1}",
                $"{course.PathDegrees:+0.0;-0.0;0.0}",
                Color.LightGray);
        }
    }

    /// <summary>Draws one row of the table with fixed column positions.</summary>
    private static void DrawRow(int x, int y, string number, string name, string distance, string bearing, string path, Color colour)
    {
        Raylib.DrawText(number, x + 10, y, FontSize, colour);
        Raylib.DrawText(name, x + 32, y, FontSize, colour);
        Raylib.DrawText(distance, x + 220, y, FontSize, colour);
        Raylib.DrawText(bearing, x + 315, y, FontSize, colour);
        Raylib.DrawText(path, x + 400, y, FontSize, colour);
    }

    /// <summary>Cuts long names so they do not run into the next column.</summary>
    private static string Shorten(string text, int maxLength)
    {
        return text.Length <= maxLength ? text : text[..(maxLength - 1)] + ".";
    }
}
