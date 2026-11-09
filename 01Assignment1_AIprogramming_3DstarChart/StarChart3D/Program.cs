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
var dust = new DustField();

// The camera starts far outside the star field and then flies in to the first star system (the Sun).
// After that it can be moved with mouse and keyboard.
var cameraController = new CameraController(startPosition: new Vector3(90f, 60f, 90f), startTarget: Vector3.Zero);
Star home = stars.FindById(0) ?? stars[0]; // HYG id 0 is the Sun
const float ArrivalDistance = 2f;          // parsecs from the star when the camera arrives
const float FlightSeconds = 6f;
cameraController.FlyTo(StarRenderer.ToWorldPosition(home), ArrivalDistance, FlightSeconds);
var nearbyPanel = new NearbyStarsPanel(stars, home); // the 10 nearest stars to the current location
Star flightDestination = home;                       // the star the camera is flying to (or last arrived at)
Vector2 leftPressPosition = Vector2.Zero;            // where the left mouse button went down, to tell a click from a drag
const float MaxClickMovement = 5f;                   // pixels the mouse may move between press and release for a click

// Starts a flight from the current location to another star. Longer trips take longer (3 to 8 seconds).
void FlyToStar(Star destination)
{
    flightDestination = destination;
    double lightYears = Course.Between(nearbyPanel.Location, destination).DistanceLightYears;
    float seconds = Math.Clamp(2f + (float)lightYears * 0.3f, 3f, 8f);
    cameraController.FlyTo(StarRenderer.ToWorldPosition(destination), ArrivalDistance, seconds);
}

var spaceColour = new Color(5, 5, 15, 255);
string title = $"{stars.Count} stars within {stars.Max(star => star.DistanceLightYears):F0} light years"; // worked out once, not every frame
const string Help = "Mouse: click a star = fly there, wheel = zoom, left-drag = rotate, right-drag = pan\n" +
                    "Keys: +/- zoom, arrows rotate, W/A/S/D/Q/E pan, R reset, F filter named stars, Esc quit";

while (!Raylib.WindowShouldClose())
{
    // Choosing a star in the nearby list (click a row or press 1-9, 0) flies the camera there.
    // This is checked before the camera update, so the click is not also treated as a camera drag.
    Course? chosen = cameraController.IsFlying ? null : nearbyPanel.GetSelection();
    if (chosen is Course course)
    {
        FlyToStar(course.Destination);
    }

    // Clicking a star in the 3D view also flies there. It only counts as a click if the mouse hardly moved
    // between press and release; otherwise it was a left-drag to rotate the camera.
    if (Raylib.IsMouseButtonPressed(MouseButton.Left))
    {
        leftPressPosition = Raylib.GetMousePosition();
    }
    if (Raylib.IsMouseButtonReleased(MouseButton.Left) && !cameraController.IsFlying && !nearbyPanel.IsMouseOver()
        && Vector2.Distance(Raylib.GetMousePosition(), leftPressPosition) <= MaxClickMovement)
    {
        Star? clicked = renderer.StarAt(Raylib.GetMousePosition(), cameraController.Camera);
        if (clicked != null && clicked.Id != nearbyPanel.Location.Id)
        {
            FlyToStar(clicked);
        }
    }

    // R returns the camera to the starting view, so the list goes back to the Sun too.
    if (Raylib.IsKeyPressed(KeyboardKey.R))
    {
        flightDestination = home;
        nearbyPanel.SetLocation(home);
    }

    bool wasFlying = cameraController.IsFlying;
    cameraController.Update();

    // When a flight finishes, the destination becomes the new location and the list is recalculated.
    if (wasFlying && !cameraController.IsFlying && nearbyPanel.Location.Id != flightDestination.Id)
    {
        nearbyPanel.SetLocation(flightDestination);
    }

    // F switches between all stars and only the stars that have a proper name.
    if (Raylib.IsKeyPressed(KeyboardKey.F))
    {
        renderer.ShowOnlyNamedStars = !renderer.ShowOnlyNamedStars;
    }
    Camera3D camera = cameraController.Camera;
    dust.Update(camera, cameraController.Distance);

    Raylib.BeginDrawing();
    Raylib.ClearBackground(spaceColour);

    Raylib.BeginMode3D(camera);
    Raylib.DrawGrid(20, 2f); // 2-parsec grid on the plane of the celestial equator, to help judge depth
    renderer.DrawStars();
    dust.Draw(camera, cameraController.Distance); // faint particles that streak past when the camera moves
    Raylib.EndMode3D();

    renderer.DrawLabels(camera); // star names are 2D text, drawn after the 3D scene

    // Dark panels behind the text, so star labels underneath do not make it unreadable.
    var panelColour = new Color(0, 0, 0, 225);
    Raylib.DrawRectangle(0, 0, 520, 104, panelColour);
    Raylib.DrawRectangle(0, Raylib.GetScreenHeight() - 50, 760, 50, panelColour);
    Raylib.DrawText(title, 10, 10, 20, Color.RayWhite);
    Raylib.DrawFPS(10, 36);
    Raylib.DrawText($"Camera is {camera.Position.Length() * Star.LightYearsPerParsec:F1} light years from the Sun", 10, 62, 16, Color.LightGray);
    string filterText = renderer.ShowOnlyNamedStars
        ? $"Filter ON: showing {renderer.VisibleStarCount} named stars (F to show all)"
        : $"Showing all {renderer.VisibleStarCount} stars (F to show only named stars)";
    Raylib.DrawText(filterText, 10, 82, 16, renderer.ShowOnlyNamedStars ? Color.Yellow : Color.LightGray);

    if (cameraController.IsFlying)
    {
        string flying = $"Flying to {flightDestination.DisplayName}...";
        Raylib.DrawText(flying, (Raylib.GetScreenWidth() - Raylib.MeasureText(flying, 30)) / 2, 120, 30, Color.SkyBlue);
    }
    Raylib.DrawText(Help, 10, Raylib.GetScreenHeight() - 44, 16, Color.Gray);
    renderer.DrawLegend();
    nearbyPanel.Draw();
    Raylib.EndDrawing();
}

Raylib.CloseWindow();
