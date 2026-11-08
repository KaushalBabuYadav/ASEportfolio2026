using System.Numerics;
using Raylib_cs;

namespace StarChart3D;

/// <summary>
/// An on-screen table of the stars nearest to the current location, with the
/// distance (light years), bearing (degrees) and path (degrees) to each one.
/// A row can be chosen by clicking it or by pressing its number key (1–9, 0 for the tenth).
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

    /// <summary>
    /// Returns the course the user chose this frame – by clicking a row or pressing its number key – or null if none.
    /// </summary>
    public Course? GetSelection()
    {
        for (int i = 0; i < _courses.Count; i++)
        {
            var key = (KeyboardKey)((int)KeyboardKey.Zero + (i + 1) % 10); // row 1 → key 1, ..., row 10 → key 0
            if (Raylib.IsKeyPressed(key))
            {
                return _courses[i];
            }
        }

        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            int row = RowUnderMouse();
            if (row >= 0)
            {
                return _courses[row];
            }
        }

        return null;
    }

    /// <summary>True if the mouse is anywhere over the panel, so a click there is not also treated as a click on a star behind it.</summary>
    public bool IsMouseOver()
    {
        Vector2 mouse = Raylib.GetMousePosition();
        int x = Raylib.GetScreenWidth() - Width - RightMargin;
        int height = 58 + _courses.Count * RowHeight;
        return mouse.X >= x && mouse.X <= x + Width && mouse.Y >= Top && mouse.Y <= Top + height;
    }

    /// <summary>Returns the index of the row under the mouse, or −1 if the mouse is not over a row.</summary>
    private int RowUnderMouse()
    {
        Vector2 mouse = Raylib.GetMousePosition();
        int x = Raylib.GetScreenWidth() - Width - RightMargin;
        int firstRowY = Top + 32 + RowHeight;

        if (mouse.X < x || mouse.X > x + Width || mouse.Y < firstRowY)
        {
            return -1;
        }

        int row = (int)((mouse.Y - firstRowY + 3) / RowHeight); // +3: text is drawn slightly below the row's top edge
        return row < _courses.Count ? row : -1;
    }

    /// <summary>Draws the table in the right-hand side of the window. The row under the mouse is highlighted.</summary>
    public void Draw()
    {
        int x = Raylib.GetScreenWidth() - Width - RightMargin;
        int height = 58 + _courses.Count * RowHeight;
        Raylib.DrawRectangle(x, Top, Width, height, new Color(0, 0, 0, 225));

        int hovered = RowUnderMouse();
        if (hovered >= 0)
        {
            int highlightY = Top + 32 + (hovered + 1) * RowHeight - 3;
            Raylib.DrawRectangle(x + 4, highlightY, Width - 8, RowHeight, new Color(60, 90, 140, 200));
        }

        Raylib.DrawText($"Nearest stars to {Location.DisplayName}  (click or press 1-0)", x + 10, Top + 8, 18, Color.RayWhite);

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
