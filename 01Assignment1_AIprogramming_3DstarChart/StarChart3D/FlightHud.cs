using System.Numerics;
using Raylib_cs;

namespace StarChart3D;

/// <summary>
/// The spaceship's flight display (head-up display): a crosshair in the middle of the screen,
/// a marker on the target star (or an arrow at the edge of the screen pointing to it), and a panel
/// with the ship's heading and the distance, bearing, path, course error and arrival time to the target.
/// </summary>
/// <remarks>
/// Bearing and path use the same definition as the nearby-stars list (<see cref="Course"/>), so the numbers can be compared.
/// </remarks>
public static class FlightHud
{
    private const int PanelWidth = 470;
    private const int PanelTop = 236;                  // same place as the nearby-stars list, which is hidden while flying
    private const int RightMargin = 10;
    private const int LineHeight = 22;
    private const int EdgeInset = 70;                  // how far from the window edge the off-screen arrow is drawn
    private static readonly Color HudColour = new(120, 220, 255, 255);
    private static readonly Color TargetColour = new(255, 210, 80, 255);

    /// <summary>Draws the whole flight display. Call after <c>EndMode3D</c>, while drawing.</summary>
    /// <param name="ship">The ship being flown.</param>
    /// <param name="camera">The camera the 3D scene was drawn with (the cockpit camera).</param>
    /// <param name="target">The chosen target star, or null if there is none.</param>
    public static void Draw(ShipController ship, Camera3D camera, Star? target)
    {
        // The crosshair marks where the nose points: a point far ahead of the ship, seen through the current camera.
        // In the cockpit view that is the middle of the screen; in the chase view the camera is above and behind
        // the ship, so the point is a little higher up the screen.
        Vector2 aim = Raylib.GetWorldToScreen(ship.Position + ship.Forward * 50f, camera);
        DrawCrosshair(aim);
        if (target != null)
        {
            DrawTargetMarker(ship, camera, target);
        }
        DrawPanel(ship, target);
    }

    /// <summary>Draws a small cross with a gap at <paramref name="aim"/>, showing where the nose points.</summary>
    private static void DrawCrosshair(Vector2 aim)
    {
        int cx = (int)aim.X;
        int cy = (int)aim.Y;
        const int Gap = 6, Length = 16;
        Raylib.DrawLine(cx - Gap - Length, cy, cx - Gap, cy, HudColour);
        Raylib.DrawLine(cx + Gap, cy, cx + Gap + Length, cy, HudColour);
        Raylib.DrawLine(cx, cy - Gap - Length, cx, cy - Gap, HudColour);
        Raylib.DrawLine(cx, cy + Gap, cx, cy + Gap + Length, HudColour);
    }

    /// <summary>
    /// Draws corner brackets round the target if it is on screen; otherwise an arrow at the edge of the
    /// screen in the direction the ship would have to turn.
    /// </summary>
    private static void DrawTargetMarker(ShipController ship, Camera3D camera, Star target)
    {
        int width = Raylib.GetScreenWidth();
        int height = Raylib.GetScreenHeight();
        Vector3 toTarget = StarRenderer.ToWorldPosition(target) - camera.Position;

        bool inFront = Vector3.Dot(toTarget, ship.Forward) > 0;
        Vector2 screen = Raylib.GetWorldToScreen(StarRenderer.ToWorldPosition(target), camera);
        bool onScreen = inFront && screen.X >= 0 && screen.X <= width && screen.Y >= 0 && screen.Y <= height;

        if (onScreen)
        {
            // The brackets must fit round the star's sphere, which can be large when the ship is close.
            float pixelsPerUnitAtOneParsec = (height / 2f) / MathF.Tan(camera.FovY * MathF.PI / 360f);
            float sphereRadius = StarRenderer.StarRadius / toTarget.Length() * pixelsPerUnitAtOneParsec;
            int halfSize = (int)MathF.Max(14f, sphereRadius + 8f);
            DrawBrackets(screen, halfSize, TargetColour);

            // The star's own label is drawn by StarRenderer, so the marker adds the distance underneath instead.
            string distance = $"{toTarget.Length() * Star.LightYearsPerParsec:F2} ly";
            Raylib.DrawText(distance, (int)screen.X - Raylib.MeasureText(distance, 16) / 2, (int)screen.Y + halfSize + 4, 16, TargetColour);
            return;
        }

        // Off screen: find which way the target is in the ship's own view (right/up), and put an arrow
        // on an ellipse just inside the window edge in that direction.
        float right = Vector3.Dot(toTarget, ship.Right);
        float up = Vector3.Dot(toTarget, ship.Up);
        Vector2 direction = new(right, -up); // screen Y grows downwards
        direction = direction.LengthSquared() < 1e-12f ? new Vector2(0, 1) : Vector2.Normalize(direction); // straight behind: point down

        Vector2 centre = new(width / 2f, height / 2f);
        float radiusX = width / 2f - EdgeInset;
        float radiusY = height / 2f - EdgeInset;
        float scale = 1f / MathF.Sqrt(direction.X * direction.X / (radiusX * radiusX) + direction.Y * direction.Y / (radiusY * radiusY));
        Vector2 point = centre + direction * scale;

        Vector2 side = new(-direction.Y, direction.X);
        Vector2 tip = point + direction * 16;
        Vector2 left = point - direction * 6 + side * 10;
        Vector2 rightCorner = point - direction * 6 - side * 10;
        Raylib.DrawTriangle(tip, left, rightCorner, TargetColour); // Raylib only fills triangles in one winding order,
        Raylib.DrawTriangle(tip, rightCorner, left, TargetColour); // so both orders are drawn and one of them shows

        string label = target.DisplayName;
        int labelWidth = Raylib.MeasureText(label, 16);
        Vector2 labelPosition = point - direction * 30 - new Vector2(labelWidth / 2f, 8);
        Raylib.DrawText(label, (int)labelPosition.X, (int)labelPosition.Y, 16, TargetColour);
    }

    /// <summary>Draws four corner brackets (like a camera viewfinder) round a point.</summary>
    private static void DrawBrackets(Vector2 centre, int halfSize, Color colour)
    {
        int x = (int)centre.X, y = (int)centre.Y, s = halfSize, l = halfSize / 2;
        Raylib.DrawLine(x - s, y - s, x - s + l, y - s, colour); Raylib.DrawLine(x - s, y - s, x - s, y - s + l, colour);
        Raylib.DrawLine(x + s, y - s, x + s - l, y - s, colour); Raylib.DrawLine(x + s, y - s, x + s, y - s + l, colour);
        Raylib.DrawLine(x - s, y + s, x - s + l, y + s, colour); Raylib.DrawLine(x - s, y + s, x - s, y + s - l, colour);
        Raylib.DrawLine(x + s, y + s, x + s - l, y + s, colour); Raylib.DrawLine(x + s, y + s, x + s, y + s - l, colour);
    }

    /// <summary>Draws the text panel with the ship's heading and the target information.</summary>
    private static void DrawPanel(ShipController ship, Star? target)
    {
        int x = Raylib.GetScreenWidth() - PanelWidth - RightMargin;
        var lines = new List<(string Text, Color Colour)>();

        Vector3 heading = StarRenderer.ToHygPosition(ship.Forward);
        (double headingBearing, double headingPath) = Course.AnglesOf(heading.X, heading.Y, heading.Z);
        lines.Add(("FLIGHT DISPLAY", Color.RayWhite));
        lines.Add(($"Heading: bearing {headingBearing:F1}°, path {headingPath:+0.0;-0.0;0.0}°", HudColour));

        if (target == null)
        {
            lines.Add(("No target. Aim the crosshair at a star and", Color.LightGray));
            lines.Add(("press T, or click a star.", Color.LightGray));
        }
        else
        {
            Vector3 ship3 = StarRenderer.ToHygPosition(ship.Position);
            Course course = Course.FromPoint(ship3.X, ship3.Y, ship3.Z, target);

            Vector3 toTarget = StarRenderer.ToWorldPosition(target) - ship.Position;
            double offCourse = toTarget.LengthSquared() < 1e-12f
                ? 0
                : Math.Acos(Math.Clamp(Vector3.Dot(Vector3.Normalize(toTarget), ship.Forward), -1f, 1f)) * 180.0 / Math.PI;

            lines.Add(($"Target: {target.DisplayName}", TargetColour));
            lines.Add(($"Distance: {course.DistanceLightYears:F2} ly", TargetColour));
            lines.Add(($"Bearing {course.BearingDegrees:F1}°, path {course.PathDegrees:+0.0;-0.0;0.0}°", TargetColour));
            lines.Add(($"Off course: {offCourse:F1}°", offCourse < 2 ? Color.Green : TargetColour));
            lines.Add(($"Arrival: {DescribeArrival(course.DistanceLightYears, ship.Speed, offCourse)}", TargetColour));
            lines.Add(ship.AutopilotOn
                ? ($"Autopilot: {ship.AutopilotStatus} (P to switch off)", Color.Green)
                : ("Autopilot: off (press P to fly there)", Color.LightGray));
        }

        int height = 12 + lines.Count * LineHeight;
        Raylib.DrawRectangle(x, PanelTop, PanelWidth, height, new Color(0, 0, 0, 225));
        for (int i = 0; i < lines.Count; i++)
        {
            Raylib.DrawText(lines[i].Text, x + 10, PanelTop + 8 + i * LineHeight, i == 0 ? 18 : 16, lines[i].Colour);
        }
    }

    /// <summary>
    /// Describes how long the trip would take at the current speed, e.g. "12.3 s" or "2 min 05 s".
    /// The time is only meaningful if the ship is pointing at the target, so that is said if it is not.
    /// </summary>
    public static string DescribeArrival(double distanceLightYears, float speedParsecsPerSecond, double offCourseDegrees)
    {
        if (speedParsecsPerSecond <= 0f)
        {
            return "never (ship stopped)";
        }
        if (offCourseDegrees > 90)
        {
            return "never (flying away)";
        }

        double seconds = distanceLightYears / (speedParsecsPerSecond * Star.LightYearsPerParsec);
        string time = seconds < 60 ? $"{seconds:F1} s" : $"{(int)(seconds / 60)} min {(int)(seconds % 60):00} s";
        return offCourseDegrees < 2 ? time : $"{time} if pointed at it";
    }
}
