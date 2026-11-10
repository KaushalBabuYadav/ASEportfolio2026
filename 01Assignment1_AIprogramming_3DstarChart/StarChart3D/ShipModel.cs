using System.Numerics;
using Raylib_cs;

namespace StarChart3D;

/// <summary>
/// Draws the spaceship as a small 3D model built from triangles: an arrow-shaped hull with two wings,
/// a tail fin and an engine glow that grows with speed. Only needed in the chase view, where the camera
/// is behind the ship, so the ship itself is on screen.
/// </summary>
/// <remarks>
/// The model is described in the ship's own frame (nose = −Z, top = +Y, right wing = +X), in parsecs, and each
/// corner is turned into a world position with the ship's <see cref="ShipController.Right"/>,
/// <see cref="ShipController.Up"/> and <see cref="ShipController.Forward"/> directions.
/// It is drawn much bigger than a real ship would be (0.06 pc ≈ 12,000 times the Earth–Sun distance),
/// otherwise it would be invisible next to the stars.
/// </remarks>
public static class ShipModel
{
    private static readonly Color HullColour = new(170, 180, 200, 255);
    private static readonly Color DarkHullColour = new(110, 120, 140, 255);
    private static readonly Color WingColour = new(70, 130, 200, 255);
    private static readonly Color EdgeColour = new(230, 240, 255, 255);
    private static readonly Color EngineColour = new(255, 150, 60, 255);

    // Corners of the model in the ship's own frame (x = right, y = up, z = backwards), in parsecs.
    private static readonly Vector3 Nose = new(0f, 0f, -0.030f);
    private static readonly Vector3 Top = new(0f, 0.007f, 0.020f);
    private static readonly Vector3 Bottom = new(0f, -0.005f, 0.020f);
    private static readonly Vector3 LeftBody = new(-0.008f, 0f, 0.020f);
    private static readonly Vector3 RightBody = new(0.008f, 0f, 0.020f);
    private static readonly Vector3 LeftWingTip = new(-0.032f, -0.002f, 0.028f);
    private static readonly Vector3 RightWingTip = new(0.032f, -0.002f, 0.028f);
    private static readonly Vector3 WingRoot = new(0f, 0f, 0.000f);
    private static readonly Vector3 FinTip = new(0f, 0.018f, 0.030f);
    private static readonly Vector3 Engine = new(0f, 0.001f, 0.022f);

    /// <summary>Draws the ship. Must be called between <c>BeginMode3D</c> and <c>EndMode3D</c>.</summary>
    public static void Draw(ShipController ship)
    {
        Vector3 W(Vector3 local) => ship.Position + ship.Right * local.X + ship.Up * local.Y - ship.Forward * local.Z;

        // Hull: a four-sided pyramid from the nose to the back, closed at the back.
        Face(W(Nose), W(LeftBody), W(Top), HullColour);
        Face(W(Nose), W(Top), W(RightBody), HullColour);
        Face(W(Nose), W(RightBody), W(Bottom), DarkHullColour);
        Face(W(Nose), W(Bottom), W(LeftBody), DarkHullColour);
        Face(W(LeftBody), W(Bottom), W(Top), DarkHullColour);
        Face(W(RightBody), W(Top), W(Bottom), DarkHullColour);

        // Wings and tail fin: flat triangles.
        Face(W(WingRoot), W(LeftWingTip), W(LeftBody), WingColour);
        Face(W(WingRoot), W(RightBody), W(RightWingTip), WingColour);
        Face(W(Top), W(FinTip), W(Top + new Vector3(0f, 0f, -0.012f)), WingColour);

        // Outline, so the shape is clear against black space.
        Vector3[] outline = { Nose, LeftWingTip, LeftBody, Top, RightBody, RightWingTip, Nose };
        for (int i = 0; i < outline.Length - 1; i++)
        {
            Raylib.DrawLine3D(W(outline[i]), W(outline[i + 1]), EdgeColour);
        }
        Raylib.DrawLine3D(W(Nose), W(Top), EdgeColour);
        Raylib.DrawLine3D(W(Top), W(FinTip), EdgeColour);

        // Engine glow: bigger when the ship is going faster (up to about twice the size at full speed).
        float glow = 0.003f + 0.004f * MathF.Min(1f, ship.Speed / 5f);
        if (ship.Speed > 0f)
        {
            Raylib.DrawSphereEx(W(Engine), glow, 6, 8, EngineColour);
        }
    }

    /// <summary>
    /// Draws a filled triangle visible from both sides. Raylib only draws a triangle whose corners appear
    /// anticlockwise on screen, so it is drawn once in each order.
    /// </summary>
    private static void Face(Vector3 a, Vector3 b, Vector3 c, Color colour)
    {
        Raylib.DrawTriangle3D(a, b, c, colour);
        Raylib.DrawTriangle3D(a, c, b, colour);
    }
}
