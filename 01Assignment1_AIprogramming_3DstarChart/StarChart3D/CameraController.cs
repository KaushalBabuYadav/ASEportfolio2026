using System.Numerics;
using Raylib_cs;

namespace StarChart3D;

/// <summary>
/// An "orbit" camera that circles around a target point, controlled by mouse and keyboard.
/// Mouse wheel zooms, left-drag rotates, right-drag pans; keys do the same for keyboard users.
/// </summary>
/// <remarks>
/// The camera position is worked out from three numbers: how far it is from the target (distance),
/// the angle around the vertical axis (yaw) and the angle up or down (pitch).
/// This keeps the camera always looking at the target, so the user cannot get "lost" upside down.
/// </remarks>
public class CameraController
{
    private const float MinDistance = 0.5f;           // parsecs: closest the camera can get to the target
    private const float MaxDistance = 150f;           // parsecs: far enough to see all 2,000 stars
    private const float MaxPitch = 1.5f;              // radians (about 86°): stops the camera flipping over the top
    private const float MouseRotateSpeed = 0.005f;    // radians per pixel of mouse movement
    private const float KeyRotateSpeed = 1.5f;        // radians per second
    private const float ZoomStep = 0.1f;              // each wheel notch changes the distance by 10%
    private const float PanSpeed = 0.3f;              // fraction of the distance moved per second with the keys

    private readonly Vector3 _startTarget;
    private readonly float _startDistance;
    private readonly float _startYaw;
    private readonly float _startPitch;

    private Vector3 _target;
    private float _distance;
    private float _yaw;
    private float _pitch;

    /// <summary>Creates a camera at <paramref name="startPosition"/> looking at <paramref name="startTarget"/>.</summary>
    public CameraController(Vector3 startPosition, Vector3 startTarget)
    {
        Vector3 offset = startPosition - startTarget;
        _startTarget = startTarget;
        _startDistance = Math.Clamp(offset.Length(), MinDistance, MaxDistance);
        _startYaw = MathF.Atan2(offset.Z, offset.X);
        _startPitch = Math.Clamp(MathF.Asin(offset.Y / offset.Length()), -MaxPitch, MaxPitch);
        Reset();
    }

    /// <summary>The Raylib camera to draw the 3D scene with, built from the current distance, yaw, pitch and target.</summary>
    public Camera3D Camera => new(
        position: _target + Offset(),
        target: _target,
        up: Vector3.UnitY,
        fovY: 45f,
        projection: CameraProjection.Perspective);

    /// <summary>How far the camera is from the point it is looking at, in parsecs.</summary>
    public float Distance => _distance;

    /// <summary>Puts the camera back where it started.</summary>
    public void Reset()
    {
        _target = _startTarget;
        _distance = _startDistance;
        _yaw = _startYaw;
        _pitch = _startPitch;
    }

    /// <summary>Reads the mouse and keyboard and moves the camera. Call once per frame, before drawing.</summary>
    public void Update()
    {
        float seconds = Raylib.GetFrameTime(); // so keyboard speed is the same at any frame rate

        if (Raylib.IsKeyPressed(KeyboardKey.R))
        {
            Reset();
        }

        // Zoom: mouse wheel, or +/- keys (PageUp/PageDown also work).
        float zoom = Raylib.GetMouseWheelMove();
        if (Raylib.IsKeyDown(KeyboardKey.Equal) || Raylib.IsKeyDown(KeyboardKey.PageUp)) zoom += 3f * seconds;
        if (Raylib.IsKeyDown(KeyboardKey.Minus) || Raylib.IsKeyDown(KeyboardKey.PageDown)) zoom -= 3f * seconds;
        _distance = Math.Clamp(_distance * (1f - zoom * ZoomStep), MinDistance, MaxDistance);

        // Rotate: left mouse drag, or the arrow keys.
        Vector2 mouse = Raylib.GetMouseDelta();
        if (Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            _yaw += mouse.X * MouseRotateSpeed;
            _pitch += mouse.Y * MouseRotateSpeed;
        }
        if (Raylib.IsKeyDown(KeyboardKey.Left)) _yaw -= KeyRotateSpeed * seconds;
        if (Raylib.IsKeyDown(KeyboardKey.Right)) _yaw += KeyRotateSpeed * seconds;
        if (Raylib.IsKeyDown(KeyboardKey.Up)) _pitch += KeyRotateSpeed * seconds;
        if (Raylib.IsKeyDown(KeyboardKey.Down)) _pitch -= KeyRotateSpeed * seconds;
        _pitch = Math.Clamp(_pitch, -MaxPitch, MaxPitch);

        // Pan (move the point the camera looks at): right mouse drag, or W/A/S/D and Q/E.
        Vector3 forward = Vector3.Normalize(-Offset());
        Vector3 right = Vector3.Normalize(Vector3.Cross(forward, Vector3.UnitY));
        Vector3 up = Vector3.Cross(right, forward);

        if (Raylib.IsMouseButtonDown(MouseButton.Right))
        {
            float pixelsToParsecs = _distance * 0.0015f; // pan more when zoomed out, less when zoomed in
            _target += (-right * mouse.X + up * mouse.Y) * pixelsToParsecs;
        }

        float step = _distance * PanSpeed * seconds;
        if (Raylib.IsKeyDown(KeyboardKey.W)) _target += forward * step;
        if (Raylib.IsKeyDown(KeyboardKey.S)) _target -= forward * step;
        if (Raylib.IsKeyDown(KeyboardKey.D)) _target += right * step;
        if (Raylib.IsKeyDown(KeyboardKey.A)) _target -= right * step;
        if (Raylib.IsKeyDown(KeyboardKey.E)) _target += Vector3.UnitY * step;
        if (Raylib.IsKeyDown(KeyboardKey.Q)) _target -= Vector3.UnitY * step;
    }

    /// <summary>The vector from the target to the camera, worked out from distance, yaw and pitch.</summary>
    private Vector3 Offset()
    {
        return new Vector3(
            _distance * MathF.Cos(_pitch) * MathF.Cos(_yaw),
            _distance * MathF.Sin(_pitch),
            _distance * MathF.Cos(_pitch) * MathF.Sin(_yaw));
    }
}
