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

    private readonly IReadOnlyList<Star> _stars;
    private readonly Vector3[] _positions;

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
}
