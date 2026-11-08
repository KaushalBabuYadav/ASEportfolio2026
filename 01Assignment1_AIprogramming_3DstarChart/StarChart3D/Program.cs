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

// The camera starts outside the star field looking at the Sun, and can then be moved with mouse and keyboard.
var cameraController = new CameraController(startPosition: new Vector3(30f, 20f, 30f), startTarget: Vector3.Zero);

var spaceColour = new Color(5, 5, 15, 255);
string title = $"{stars.Count} stars within {stars.Max(star => star.DistanceLightYears):F0} light years"; // worked out once, not every frame
const string Help = "Mouse: wheel = zoom, left-drag = rotate, right-drag = pan\n" +
                    "Keys: +/- zoom, arrows rotate, W/A/S/D/Q/E pan, R reset, Esc quit";

while (!Raylib.WindowShouldClose())
{
    cameraController.Update();
    Camera3D camera = cameraController.Camera;

    Raylib.BeginDrawing();
    Raylib.ClearBackground(spaceColour);

    Raylib.BeginMode3D(camera);
    Raylib.DrawGrid(20, 2f); // 2-parsec grid on the plane of the celestial equator, to help judge depth
    renderer.DrawStars();
    Raylib.EndMode3D();

    renderer.DrawLabels(camera); // star names are 2D text, drawn after the 3D scene

    // Dark panels behind the text, so star labels underneath do not make it unreadable.
    var panelColour = new Color(0, 0, 0, 225);
    Raylib.DrawRectangle(0, 0, 470, 84, panelColour);
    Raylib.DrawRectangle(0, Raylib.GetScreenHeight() - 50, 620, 50, panelColour);
    Raylib.DrawText(title, 10, 10, 20, Color.RayWhite);
    Raylib.DrawFPS(10, 36);
    Raylib.DrawText($"Camera is {camera.Position.Length() * Star.LightYearsPerParsec:F1} light years from the Sun", 10, 62, 16, Color.LightGray);
    Raylib.DrawText(Help, 10, Raylib.GetScreenHeight() - 44, 16, Color.Gray);
    renderer.DrawLegend();
    Raylib.EndDrawing();
}

Raylib.CloseWindow();
