using System.Numerics;
using Raylib_cs;

namespace StarChart3D;

/// <summary>
/// Draws the stars of a <see cref="StarCollection"/> in 3D with Raylib.
/// One world unit is one parsec, and the Sun is at (0, 0, 0).
/// </summary>
public class StarRenderer
{
    /// <summary>Radius of the small sphere drawn for each star, in parsecs.</summary>
    public const float StarRadius = 0.08f;

    /// <summary>
    /// Stars without a proper name are only labelled when they are closer to the camera than this (in parsecs),
    /// so that the screen is not covered by 1,900 catalogue names at once.
    /// </summary>
    public const float CatalogueLabelDistance = 8f;

    /// <summary>Smallest radius in pixels that a star counts as when it is clicked, so tiny distant stars can still be hit.</summary>
    public const float ClickTolerance = 7f;

    /// <summary>Minimum gap in pixels kept between labels, so that names do not run into each other.</summary>
    private const int LabelGap = 4;

    private static readonly Color ProperNameColour = new(255, 230, 150, 255);
    private static readonly Color CatalogueNameColour = new(150, 160, 180, 255);

    private readonly IReadOnlyList<Star> _stars;
    private readonly Vector3[] _positions;
    private readonly Color[] _colours;
    private readonly Dictionary<SpectralClass, int> _classCounts = new();
    private readonly List<Rectangle> _labelAreas = new(); // screen areas already used by labels this frame

    /// <summary>
    /// When true, only stars with a proper name are drawn and labelled; stars that only have a
    /// catalogue name are hidden. Toggled by the user (F key in Program.cs).
    /// </summary>
    public bool ShowOnlyNamedStars { get; set; }

    /// <summary>Number of stars currently shown, taking <see cref="ShowOnlyNamedStars"/> into account.</summary>
    public int VisibleStarCount => ShowOnlyNamedStars ? _stars.Count(star => star.HasProperName) : _stars.Count;

    /// <summary>True if the star at this position in the list should be drawn with the current filter.</summary>
    private bool IsVisible(int index) => !ShowOnlyNamedStars || _stars[index].HasProperName;

    /// <summary>Creates a renderer and works out each star's 3D position and colour once, so they are not recalculated every frame.</summary>
    /// <param name="stars">The stars to draw.</param>
    public StarRenderer(IReadOnlyList<Star> stars)
    {
        _stars = stars;
        _positions = new Vector3[stars.Count];
        _colours = new Color[stars.Count];
        for (int i = 0; i < stars.Count; i++)
        {
            _positions[i] = ToWorldPosition(stars[i]);

            // Spectral class and colour are also worked out once, not every frame.
            SpectralClass spectralClass = SpectralClasses.Parse(stars[i].SpectralType);
            _colours[i] = SpectralClasses.ColourOf(spectralClass);
            _classCounts[spectralClass] = _classCounts.GetValueOrDefault(spectralClass) + 1;
        }
    }

    /// <summary>
    /// Converts a star's HYG position to Raylib's 3D world.
    /// HYG uses +Z towards the north celestial pole, but in Raylib +Y is "up",
    /// so (x, y, z) becomes (x, z, -y). This is a rotation, not a mirror,
    /// so the sky is not flipped left-to-right.
    /// </summary>
    public static Vector3 ToWorldPosition(Star star)
    {
        return new Vector3((float)star.X, (float)star.Z, (float)-star.Y);
    }

    /// <summary>
    /// Draws every visible star as a small sphere in the colour of its spectral class
    /// (all stars, or only named stars when <see cref="ShowOnlyNamedStars"/> is on). Must be called between
    /// <c>Raylib.BeginMode3D</c> and <c>Raylib.EndMode3D</c>.
    /// </summary>
    public void DrawStars()
    {
        for (int i = 0; i < _positions.Length; i++)
        {
            if (!IsVisible(i))
            {
                continue;
            }

            // Few rings and slices: each star is tiny on screen, and 2,000 detailed spheres would be slow.
            Raylib.DrawSphereEx(_positions[i], StarRadius, 6, 8, _colours[i]);
        }
    }

    /// <summary>
    /// Writes star names next to the stars. Must be called after <c>Raylib.EndMode3D</c>,
    /// because the names are 2D text drawn on top of the 3D scene.
    /// Stars with a proper name are labelled; other stars get their catalogue name
    /// only when they are within <see cref="CatalogueLabelDistance"/> of the camera.
    /// A label is skipped if it would overlap a label already drawn. The stars are in
    /// nearest-first order, so the nearer star keeps its label.
    /// </summary>
    /// <param name="camera">The camera used to draw the 3D scene.</param>
    public void DrawLabels(Camera3D camera)
    {
        Vector3 viewDirection = Vector3.Normalize(camera.Target - camera.Position);
        int screenWidth = Raylib.GetScreenWidth();
        int screenHeight = Raylib.GetScreenHeight();
        _labelAreas.Clear();

        for (int i = 0; i < _positions.Length; i++)
        {
            if (!IsVisible(i))
            {
                continue;
            }

            Star star = _stars[i];
            Vector3 toStar = _positions[i] - camera.Position;

            // Skip stars behind the camera: GetWorldToScreen would still give them a (wrong) screen position.
            if (Vector3.Dot(toStar, viewDirection) <= 0)
            {
                continue;
            }

            if (!star.HasProperName && toStar.Length() > CatalogueLabelDistance)
            {
                continue;
            }

            Vector2 screen = Raylib.GetWorldToScreen(_positions[i], camera);
            if (screen.X < 0 || screen.X > screenWidth || screen.Y < 0 || screen.Y > screenHeight)
            {
                continue;
            }

            int fontSize = star.HasProperName ? 14 : 10;
            // Put the label just outside the star's sphere. Close up, a star covers many pixels, so a fixed
            // offset would put the text on top of the star. Pixels per parsec at this distance =
            // (half the screen height) / tan(half the field of view) / distance.
            float pixelRadius = StarRadius / toStar.Length() * (screenHeight / 2f) / MathF.Tan(camera.FovY * MathF.PI / 360f);
            int x = (int)(screen.X + MathF.Max(6f, pixelRadius + 4f));
            int y = (int)screen.Y - fontSize / 2;
            // The area is made a little bigger than the text, so that labels keep a gap between them.
            var area = new Rectangle(x - LabelGap, y - LabelGap / 2, Raylib.MeasureText(star.DisplayName, fontSize) + 2 * LabelGap, fontSize + LabelGap);

            if (OverlapsExistingLabel(area))
            {
                continue;
            }

            _labelAreas.Add(area);
            Raylib.DrawText(star.DisplayName, x, y, fontSize, star.HasProperName ? ProperNameColour : CatalogueNameColour);
        }
    }

    /// <summary>
    /// Returns the visible star drawn at a point on the screen (for example where the user clicked),
    /// or null if there is none. Each star counts as at least <see cref="ClickTolerance"/> pixels wide, so small,
    /// distant stars can still be clicked. If several stars are under the point, the one nearest the camera wins.
    /// </summary>
    /// <param name="screenPoint">Point in window pixels, e.g. <c>Raylib.GetMousePosition()</c>.</param>
    /// <param name="camera">The camera used to draw the 3D scene.</param>
    public Star? StarAt(Vector2 screenPoint, Camera3D camera)
    {
        Vector3 viewDirection = Vector3.Normalize(camera.Target - camera.Position);
        int screenHeight = Raylib.GetScreenHeight();
        float pixelsPerUnitAtOneParsec = (screenHeight / 2f) / MathF.Tan(camera.FovY * MathF.PI / 360f);

        Star? best = null;
        float bestDistance = float.MaxValue;
        for (int i = 0; i < _positions.Length; i++)
        {
            if (!IsVisible(i))
            {
                continue;
            }

            Vector3 toStar = _positions[i] - camera.Position;
            float distance = toStar.Length();
            if (Vector3.Dot(toStar, viewDirection) <= 0 || distance >= bestDistance)
            {
                continue; // behind the camera, or further away than a star already found
            }

            Vector2 screen = Raylib.GetWorldToScreen(_positions[i], camera);
            float pixelRadius = MathF.Max(ClickTolerance, StarRadius / distance * pixelsPerUnitAtOneParsec + 2f);
            if (Vector2.Distance(screen, screenPoint) <= pixelRadius)
            {
                best = _stars[i];
                bestDistance = distance;
            }
        }

        return best;
    }

    /// <summary>
    /// Draws a colour key in the top-right corner: one coloured dot per spectral class,
    /// with how many of the loaded stars are in that class.
    /// </summary>
    public void DrawLegend()
    {
        const int rowHeight = 20;
        int width = 190;
        int x = Raylib.GetScreenWidth() - width - 10;
        int y = 10;
        int height = 30 + SpectralClasses.LegendOrder.Length * rowHeight;

        Raylib.DrawRectangle(x, y, width, height, new Color(0, 0, 0, 225));
        Raylib.DrawText("Spectral class", x + 10, y + 8, 16, Color.RayWhite);

        for (int i = 0; i < SpectralClasses.LegendOrder.Length; i++)
        {
            SpectralClass spectralClass = SpectralClasses.LegendOrder[i];
            int rowY = y + 30 + i * rowHeight;
            Raylib.DrawCircle(x + 18, rowY + 7, 6, SpectralClasses.ColourOf(spectralClass));
            string text = $"{SpectralClasses.NameOf(spectralClass)}: {_classCounts.GetValueOrDefault(spectralClass)}";
            Raylib.DrawText(text, x + 32, rowY, 16, Color.LightGray);
        }
    }

    /// <summary>Returns true if the given screen area overlaps a label already drawn this frame.</summary>
    private bool OverlapsExistingLabel(Rectangle area)
    {
        foreach (Rectangle used in _labelAreas)
        {
            if (Raylib.CheckCollisionRecs(area, used))
            {
                return true;
            }
        }

        return false;
    }
}
