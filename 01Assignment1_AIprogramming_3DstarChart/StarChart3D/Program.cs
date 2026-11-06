using System.Numerics;
using Raylib_cs;
using StarChart3D;

// Load the 2,000 nearest stars.
string dataFile = Path.Combine(AppContext.BaseDirectory, "Data", "stars_local_2000.csv");
var stars = new StarCollection(dataFile);

// Open a resizable window with anti-aliasing (smoother edges).
Raylib.SetConfigFlags(ConfigFlags.Msaa4xHint | ConfigFlags.ResizableWindow);
Raylib.InitWindow(1280, 800, "StarChart3D");
Raylib.SetTargetFPS(60);

var renderer = new StarRenderer(stars);

// A fixed camera looking at the Sun from outside the star field (camera controls come later in Feature 2).
var camera = new Camera3D(
    position: new Vector3(30f, 20f, 30f),
    target: Vector3.Zero,
    up: Vector3.UnitY,
    fovY: 45f,
    projection: CameraProjection.Perspective);

var spaceColour = new Color(5, 5, 15, 255);
string title = $"{stars.Count} stars within {stars.Max(star => star.DistanceLightYears):F0} light years"; // worked out once, not every frame

while (!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();
    Raylib.ClearBackground(spaceColour);

    Raylib.BeginMode3D(camera);
    Raylib.DrawGrid(20, 2f); // 2-parsec grid on the plane of the celestial equator, to help judge depth
    renderer.DrawStars();
    Raylib.EndMode3D();

    renderer.DrawLabels(camera); // star names are 2D text, drawn after the 3D scene

    Raylib.DrawText(title, 10, 10, 20, Color.RayWhite);
    Raylib.DrawFPS(10, 36);
    Raylib.EndDrawing();
}

Raylib.CloseWindow();
