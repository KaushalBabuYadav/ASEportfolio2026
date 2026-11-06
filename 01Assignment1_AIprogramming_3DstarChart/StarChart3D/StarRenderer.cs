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

    /// <summary>Minimum gap in pixels kept between labels, so that names do not run into each other.</summary>
    private const int LabelGap = 4;

    private static readonly Color ProperNameColour = new(255, 230, 150, 255);
    private static readonly Color CatalogueNameColour = new(150, 160, 180, 255);

    private readonly IReadOnlyList<Star> _stars;
    private readonly Vector3[] _positions;
    private readonly List<Rectangle> _labelAreas = new(); // screen areas already used by labels this frame

    /// <summary>Creates a renderer and works out each star's 3D position once, so it is not recalculated every frame.</summary>
    /// <param name="stars">The stars to draw.</param>
    public StarRenderer(IReadOnlyList<Star> stars)
    {
        _stars = stars;
        _positions = new Vector3[stars.Count];
        for (int i = 0; i < stars.Count; i++)
        {
            _positions[i] = ToWorldPosition(stars[i]);
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
    /// Draws every star as a small sphere. Must be called between
    /// <c>Raylib.BeginMode3D</c> and <c>Raylib.EndMode3D</c>.
    /// </summary>
    public void DrawStars()
    {
        for (int i = 0; i < _positions.Length; i++)
        {
            // Few rings and slices: each star is tiny on screen, and 2,000 detailed spheres would be slow.
            Raylib.DrawSphereEx(_positions[i], StarRadius, 6, 8, Color.White);
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
            int x = (int)screen.X + 6;
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
