using System.Numerics;
using Raylib_cs;

namespace StarChart3D;

/// <summary>
/// "Space dust": faint particles in a box that moves with the camera. They drift slowly all the time,
/// and when the camera moves they are drawn as short streaks pointing back along the movement,
/// which gives the feeling of travelling through space.
/// </summary>
/// <remarks>
/// The dust is purely visual and is not real data. The box is sized relative to how far the camera is
/// from what it is looking at, so the dust stays visible whether the user is zoomed in or out.
/// </remarks>
public class DustField
{
    private const int ParticleCount = 400;
    private const float BoxFraction = 0.5f;     // half the box width, as a fraction of the camera's viewing distance
    private const float DriftFraction = 0.02f;  // slow drift speed, as a fraction of the box size per second
    private const float StreakSeconds = 0.12f;  // a streak shows where a particle appeared to be this long ago
    private const float MaxStreakFraction = 0.4f; // streaks are never longer than this fraction of the box
    private const float SpeckFraction = 0.006f;   // length of a still dust speck, as a fraction of the box

    private readonly Vector3[] _offsets = new Vector3[ParticleCount]; // position relative to the camera, as a fraction of the box (-1 to 1)
    private readonly Vector3[] _drift = new Vector3[ParticleCount];   // each particle's own slow drift direction
    private Vector3 _lastCameraPosition;
    private Vector3 _cameraVelocity;
    private bool _hasLastPosition;

    /// <summary>Creates the dust with a fixed random seed, so it looks the same each time the program runs.</summary>
    public DustField(int seed = 12345)
    {
        var random = new Random(seed);
        for (int i = 0; i < ParticleCount; i++)
        {
            _offsets[i] = new Vector3(RandomUnit(random), RandomUnit(random), RandomUnit(random));
            _drift[i] = Vector3.Normalize(new Vector3(RandomUnit(random), RandomUnit(random), RandomUnit(random)) + new Vector3(0.001f));
        }
    }

    /// <summary>Camera speed at the last update, in parsecs per second.</summary>
    public float CameraSpeed => _cameraVelocity.Length();

    /// <summary>
    /// Moves the dust for this frame. Particles that move out of the box (because the camera moved past them)
    /// wrap around to the other side, so there is always dust around the camera.
    /// </summary>
    /// <param name="camera">The camera used this frame.</param>
    /// <param name="viewingDistance">How far the camera is from the point it looks at, in parsecs.</param>
    public void Update(Camera3D camera, float viewingDistance)
    {
        float seconds = Raylib.GetFrameTime();
        float boxHalf = MathF.Max(viewingDistance * BoxFraction, 0.2f);

        if (_hasLastPosition && seconds > 0f)
        {
            _cameraVelocity = (camera.Position - _lastCameraPosition) / seconds;

            // Moving the camera forward makes the dust move backwards relative to the camera.
            Vector3 cameraMoveInBoxUnits = (camera.Position - _lastCameraPosition) / boxHalf;
            for (int i = 0; i < ParticleCount; i++)
            {
                _offsets[i] += _drift[i] * DriftFraction * seconds - cameraMoveInBoxUnits;
                _offsets[i] = Wrap(_offsets[i]);
            }
        }

        _lastCameraPosition = camera.Position;
        _hasLastPosition = true;
    }

    /// <summary>
    /// Draws the dust. Must be called between <c>Raylib.BeginMode3D</c> and <c>Raylib.EndMode3D</c>.
    /// Particles further from the camera are fainter.
    /// </summary>
    public void Draw(Camera3D camera, float viewingDistance)
    {
        float boxHalf = MathF.Max(viewingDistance * BoxFraction, 0.2f);
        Vector3 streak = -_cameraVelocity * StreakSeconds;
        float maxStreak = boxHalf * MaxStreakFraction;
        if (streak.Length() > maxStreak)
        {
            streak = Vector3.Normalize(streak) * maxStreak;
        }

        for (int i = 0; i < ParticleCount; i++)
        {
            Vector3 position = camera.Position + _offsets[i] * boxHalf;
            float nearness = 1f - Math.Clamp(_offsets[i].Length() / 1.7f, 0f, 1f); // 1 at the camera, 0 at the box corner
            var colour = new Color(180, 200, 255, (int)(40 + 120 * nearness));

            // A speck is always drawn as a tiny line in the particle's own drift direction. (Raylib's DrawPoint3D
            // is not used: it is "actually a small line" 0.1 units long along Z, which is 0.1 parsecs here,
            // so every "point" became a long line pointing the same way.) When the camera moves, the streak is added.
            Vector3 speck = _drift[i] * boxHalf * SpeckFraction;
            Raylib.DrawLine3D(position, position + speck + streak, colour);
        }
    }

    /// <summary>Wraps each coordinate back into the range -1 to 1.</summary>
    private static Vector3 Wrap(Vector3 offset)
    {
        return new Vector3(WrapOne(offset.X), WrapOne(offset.Y), WrapOne(offset.Z));
    }

    private static float WrapOne(float value)
    {
        // Shift to 0..2, take the remainder, shift back to -1..1. The extra + 2 handles negative values.
        return ((value + 1f) % 2f + 2f) % 2f - 1f;
    }

    private static float RandomUnit(Random random) => (float)(random.NextDouble() * 2.0 - 1.0);
}
