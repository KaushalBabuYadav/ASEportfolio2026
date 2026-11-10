using System.Numerics;
using Raylib_cs;

namespace StarChart3D;

/// <summary>
/// A spaceship that the user flies from the cockpit. Unlike the orbit camera, the ship can point in any
/// direction (including upside down), because its orientation is stored as a quaternion.
/// </summary>
/// <remarks>
/// The flying is "arcade" style, not real physics: the ship always moves in the direction its nose points,
/// and its speed eases towards the speed the user has set with the throttle. Speeds are far faster than light,
/// otherwise even the nearest star would take more than four years to reach.
/// One world unit is one parsec, as in the rest of the program.
/// </remarks>
public class ShipController
{
    /// <summary>Slowest non-zero speed the throttle can be set to, in parsecs per second.</summary>
    public const float MinSpeed = 0.01f;

    /// <summary>Fastest speed the throttle can be set to, in parsecs per second.</summary>
    public const float MaxSpeed = 20f;

    /// <summary>One parsec per second expressed as a multiple of the speed of light (3.0857e13 km ÷ 299,792.458 km/s).</summary>
    public const double LightSpeedsPerParsecPerSecond = 1.02927e8;

    private const float ThrottleRate = 1.5f;          // holding W or S multiplies or divides the set speed by e^1.5 (about 4.5) per second
    private const float SpeedResponse = 2.0f;         // how quickly the actual speed catches up with the set speed (per second)
    private const float KeyTurnSpeed = 1.2f;          // radians per second for arrow keys and Q/E
    private const float MouseTurnSpeed = 0.003f;      // radians per pixel of mouse movement while the left button is held

    // In the ship's own frame the nose points along −Z, the top along +Y and the right wing along +X.
    private static readonly Vector3 LocalForward = -Vector3.UnitZ;
    private static readonly Vector3 LocalUp = Vector3.UnitY;
    private static readonly Vector3 LocalRight = Vector3.UnitX;

    private Quaternion _orientation = Quaternion.Identity;
    private Vector3 _autopilotDestination;

    /// <summary>
    /// How far short of the destination star the autopilot stops, in parsecs (about 3.3 light years). The star spheres
    /// are drawn far larger than real stars, so stopping much closer would fill the whole screen with the star.
    /// </summary>
    public const float AutopilotStopDistance = 1.0f;

    private const float AutopilotTurnSpeed = 1.0f;     // radians per second the autopilot may turn the ship
    private const float AutopilotSpeedGain = 0.5f;     // set speed (pc/s) per parsec of remaining distance
    private const float ArrivalTolerance = 0.02f;      // parsecs from the stopping point that count as "arrived"

    /// <summary>True while the autopilot is flying the ship.</summary>
    public bool AutopilotOn { get; private set; }

    /// <summary>What the autopilot is doing, for the flight display: "off", "turning towards target", "accelerating", "slowing down" or "arrived".</summary>
    public string AutopilotStatus { get; private set; } = "off";

    /// <summary>Where the ship is, in world coordinates (parsecs).</summary>
    public Vector3 Position { get; private set; }

    /// <summary>The speed the user has asked for with the throttle, in parsecs per second.</summary>
    public float SetSpeed { get; private set; }

    /// <summary>The ship's actual speed, in parsecs per second. It eases towards <see cref="SetSpeed"/>.</summary>
    public float Speed { get; private set; }

    /// <summary>The direction the nose points, as a unit vector in world coordinates.</summary>
    public Vector3 Forward => Vector3.Transform(LocalForward, _orientation);

    /// <summary>The direction the top of the ship points, as a unit vector in world coordinates.</summary>
    public Vector3 Up => Vector3.Transform(LocalUp, _orientation);

    /// <summary>The direction of the right wing, as a unit vector in world coordinates.</summary>
    public Vector3 Right => Vector3.Transform(LocalRight, _orientation);

    /// <summary>The ship's speed as a multiple of the speed of light.</summary>
    public double SpeedInLightSpeeds => Speed * LightSpeedsPerParsecPerSecond;

    /// <summary>The cockpit camera: at the ship's position, looking where the nose points, tilted with the ship.</summary>
    public Camera3D Camera => new(
        position: Position,
        target: Position + Forward,
        up: Up,
        fovY: 45f, // same as the orbit camera, so switching views does not zoom
        projection: CameraProjection.Perspective);

    /// <summary>True when the camera follows the ship from behind (chase view) instead of sitting in the cockpit.</summary>
    public bool ChaseView { get; set; }

    /// <summary>Distance of the chase camera behind the ship, in parsecs (about four ship lengths).</summary>
    private const float ChaseDistance = 0.22f;

    /// <summary>Height of the chase camera above the ship's centre line, in parsecs.</summary>
    private const float ChaseHeight = 0.06f;

    /// <summary>
    /// The camera to draw the scene with: the cockpit camera, or in chase view a camera behind and slightly
    /// above the ship, looking at a point ahead of it and tilting with it.
    /// </summary>
    public Camera3D ViewCamera => ChaseView
        ? new Camera3D(
            position: Position - Forward * ChaseDistance + Up * ChaseHeight,
            target: Position + Forward * 0.5f,
            up: Up,
            fovY: 45f,
            projection: CameraProjection.Perspective)
        : Camera;

    /// <summary>
    /// Describes a speed for the screen, e.g. "1.63 ly/s (51.5 million × light speed)".
    /// </summary>
    /// <param name="parsecsPerSecond">Speed in parsecs per second.</param>
    public static string DescribeSpeed(float parsecsPerSecond)
    {
        if (parsecsPerSecond <= 0f)
        {
            return "stopped";
        }

        double lightYearsPerSecond = parsecsPerSecond * Star.LightYearsPerParsec;
        double lightSpeeds = parsecsPerSecond * LightSpeedsPerParsecPerSecond;
        string multiple = lightSpeeds switch
        {
            >= 1e9 => $"{lightSpeeds / 1e9:F1} billion",
            >= 1e6 => $"{lightSpeeds / 1e6:F1} million",
            _ => $"{lightSpeeds:N0}",
        };
        return $"{lightYearsPerSecond:F2} ly/s ({multiple} × light speed)";
    }

    /// <summary>
    /// Puts the ship at <paramref name="position"/> pointing at <paramref name="lookAt"/>, level with the
    /// equatorial plane (world +Y up), and stops it. Used when the user switches from the orbit view to the ship,
    /// so the view does not jump.
    /// </summary>
    public void Place(Vector3 position, Vector3 lookAt)
    {
        Position = position;
        Vector3 forward = lookAt - position;
        if (forward.LengthSquared() < 1e-12f)
        {
            forward = LocalForward;
        }

        // CreateWorld builds a rotation whose −Z axis is "forward" and whose Y axis is as close to "up" as possible.
        // If the ship would point straight up or down, a different "up" is used so the cross product is not zero.
        Vector3 up = MathF.Abs(Vector3.Dot(Vector3.Normalize(forward), Vector3.UnitY)) > 0.999f ? Vector3.UnitZ : Vector3.UnitY;
        Matrix4x4 world = Matrix4x4.CreateWorld(Vector3.Zero, Vector3.Normalize(forward), up);
        _orientation = Quaternion.Normalize(Quaternion.CreateFromRotationMatrix(world));

        SetSpeed = 0f;
        Speed = 0f;
    }

    /// <summary>
    /// Reads the keyboard and mouse, turns the ship and moves it. Call once per frame, before drawing.
    /// W/S = throttle up/down, X = stop, arrow keys or left-drag = turn (up/down and left/right), Q/E = roll.
    /// Any of these controls switches the autopilot off, so the pilot can always take over.
    /// </summary>
    /// <returns>True on the one frame in which the autopilot arrives at its destination.</returns>
    public bool Update()
    {
        float seconds = Raylib.GetFrameTime();

        // Turning. FPS-style: mouse up or the Up arrow lifts the nose; mouse right or the Right arrow turns right.
        float pitch = 0f, yaw = 0f, roll = 0f;
        if (Raylib.IsKeyDown(KeyboardKey.Up)) pitch += KeyTurnSpeed * seconds;
        if (Raylib.IsKeyDown(KeyboardKey.Down)) pitch -= KeyTurnSpeed * seconds;
        if (Raylib.IsKeyDown(KeyboardKey.Left)) yaw += KeyTurnSpeed * seconds;
        if (Raylib.IsKeyDown(KeyboardKey.Right)) yaw -= KeyTurnSpeed * seconds;
        if (Raylib.IsKeyDown(KeyboardKey.Q)) roll += KeyTurnSpeed * seconds;
        if (Raylib.IsKeyDown(KeyboardKey.E)) roll -= KeyTurnSpeed * seconds;
        if (Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            Vector2 mouse = Raylib.GetMouseDelta();
            pitch -= mouse.Y * MouseTurnSpeed;
            yaw -= mouse.X * MouseTurnSpeed;
        }

        bool throttleUp = Raylib.IsKeyDown(KeyboardKey.W);
        bool throttleDown = Raylib.IsKeyDown(KeyboardKey.S);
        bool stop = Raylib.IsKeyPressed(KeyboardKey.X);

        // The pilot takes over as soon as any flying control is used.
        if (AutopilotOn && (pitch != 0f || yaw != 0f || roll != 0f || throttleUp || throttleDown || stop))
        {
            StopAutopilot();
        }

        Turn(pitch, yaw, roll);

        // Throttle. The set speed changes by a factor per second (not a fixed amount), so the same key
        // gives fine control at low speed and fast change at high speed.
        if (throttleUp)
        {
            SetSpeed = SetSpeed < MinSpeed ? MinSpeed : MathF.Min(SetSpeed * MathF.Exp(ThrottleRate * seconds), MaxSpeed);
        }
        if (throttleDown)
        {
            SetSpeed *= MathF.Exp(-ThrottleRate * seconds);
            if (SetSpeed < MinSpeed)
            {
                SetSpeed = 0f;
            }
        }
        if (stop)
        {
            SetSpeed = 0f;
        }

        if (AutopilotOn)
        {
            RunAutopilot(seconds);
        }

        // The real speed eases towards the set speed, so the ship does not jump from stopped to full speed.
        Speed += (SetSpeed - Speed) * MathF.Min(1f, SpeedResponse * seconds);
        if (SetSpeed == 0f && Speed < MinSpeed * 0.1f)
        {
            Speed = 0f;
        }

        Position += Forward * Speed * seconds;

        // Arrived: close enough to the stopping point. The ship is stopped and the autopilot switches itself off.
        if (AutopilotOn && Vector3.Distance(Position, _autopilotDestination) - AutopilotStopDistance <= ArrivalTolerance)
        {
            SetSpeed = 0f;
            Speed = 0f;
            AutopilotOn = false;
            AutopilotStatus = "arrived";
            return true;
        }

        return false;
    }

    /// <summary>
    /// Switches the autopilot on. It will turn the ship towards <paramref name="destination"/> (world coordinates),
    /// fly there and stop <see cref="AutopilotStopDistance"/> parsecs short of it, so the ship does not end up inside the star.
    /// </summary>
    public void StartAutopilot(Vector3 destination)
    {
        _autopilotDestination = destination;
        AutopilotOn = true;
        AutopilotStatus = "turning towards target";
    }

    /// <summary>Switches the autopilot off. The ship keeps its current set speed.</summary>
    public void StopAutopilot()
    {
        AutopilotOn = false;
        AutopilotStatus = "off";
    }

    /// <summary>
    /// One frame of autopilot: turn the nose towards the destination at a limited rate, and set the speed from the
    /// remaining distance. The set speed is half the remaining distance per second; with the speed easing in
    /// <see cref="Update"/> this approaches the stopping point smoothly without overshooting it
    /// (a "critically damped" approach). While the nose is more than 30° off, the ship slows right down to turn.
    /// </summary>
    private void RunAutopilot(float seconds)
    {
        Vector3 toDestination = _autopilotDestination - Position;
        float distance = toDestination.Length();
        if (distance < 1e-6f)
        {
            return;
        }
        Vector3 direction = toDestination / distance;

        // Turn towards the destination, at most AutopilotTurnSpeed radians per second.
        float angle = MathF.Acos(Math.Clamp(Vector3.Dot(Forward, direction), -1f, 1f));
        if (angle > 1e-4f)
        {
            Vector3 axis = Vector3.Cross(Forward, direction);
            axis = axis.LengthSquared() < 1e-12f ? Up : Vector3.Normalize(axis); // exactly behind: turn over the top
            float step = MathF.Min(angle, AutopilotTurnSpeed * seconds);
            // Multiplying on the left applies the rotation in world axes (around "axis"), not the ship's own axes.
            _orientation = Quaternion.Normalize(Quaternion.CreateFromAxisAngle(axis, step) * _orientation);
        }

        float angleDegrees = angle * 180f / MathF.PI;
        float remaining = MathF.Max(0f, distance - AutopilotStopDistance);
        float wanted = Math.Clamp(remaining * AutopilotSpeedGain, 0f, MaxSpeed);
        float alignment = Math.Clamp(1f - angleDegrees / 30f, 0f, 1f); // 1 when pointing at it, 0 when 30° or more off
        SetSpeed = wanted * alignment;

        AutopilotStatus = angleDegrees > 5f ? "turning towards target"
                        : SetSpeed < Speed ? "slowing down"
                        : "accelerating";
    }

    /// <summary>
    /// Rotates the ship around its own axes: <paramref name="pitch"/> around the right wing (nose up is positive),
    /// <paramref name="yaw"/> around its top (nose left is positive) and <paramref name="roll"/> around its nose.
    /// Angles are in radians.
    /// </summary>
    public void Turn(float pitch, float yaw, float roll)
    {
        // Multiplying on the right applies the rotation in the ship's own frame, not the world's.
        Quaternion change = Quaternion.CreateFromAxisAngle(LocalRight, pitch)
                          * Quaternion.CreateFromAxisAngle(LocalUp, yaw)
                          * Quaternion.CreateFromAxisAngle(LocalForward, -roll);
        _orientation = Quaternion.Normalize(_orientation * change);
    }
}
