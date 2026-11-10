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
var ship = new ShipController(); // Feature 4: the spaceship the user can fly from the cockpit
bool shipMode = false;           // false = orbit view (Features 2 and 3), true = flying the ship
Star? shipTarget = null;         // the star chosen as the ship's target (T or click in ship mode), shown on the flight display
Star? autopilotStar = null;      // the star the autopilot is flying to
string arrivalMessage = "";      // "Arrived at ..." shown for a few seconds after the autopilot arrives
float arrivalMessageSeconds = 0f;

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
                    "Keys: +/- zoom, arrows rotate, W/A/S/D/Q/E pan, R reset, F filter named stars, V fly ship, Esc quit";
const string ShipHelp = "Ship: W/S throttle up/down, X stop, arrows or left-drag steer, Q/E roll\n" +
                        "Keys: T target (or click a star), P autopilot, C cockpit/chase view, V chart view, R reset, F filter, Esc quit";

while (!Raylib.WindowShouldClose())
{
    // V switches between the orbit view and flying the ship (not during an automatic flight).
    // The new view starts where the old one was, so the picture does not jump.
    if (Raylib.IsKeyPressed(KeyboardKey.V) && !cameraController.IsFlying)
    {
        if (shipMode)
        {
            // If the ship is parked at the current location's star (e.g. after the autopilot), orbit round that star;
            // otherwise look at a point ahead of the ship.
            Vector3 locationStar = StarRenderer.ToWorldPosition(nearbyPanel.Location);
            Vector3 lookAt = Vector3.Distance(ship.Position, locationStar) < ShipController.AutopilotStopDistance + 0.5f
                ? locationStar
                : ship.Position + ship.Forward * ArrivalDistance;
            cameraController.LookFrom(ship.Position, lookAt);
            ship.StopAutopilot();
        }
        else
        {
            Camera3D orbitCamera = cameraController.Camera;
            ship.Place(orbitCamera.Position, orbitCamera.Target);
        }
        shipMode = !shipMode;
    }

    // R always goes back to the starting orbit view (CameraController.Update does the reset itself).
    if (shipMode && Raylib.IsKeyPressed(KeyboardKey.R))
    {
        shipMode = false;
    }

    // Remember where the left button went down, so a click (hardly any movement) can be told apart from a drag.
    if (Raylib.IsMouseButtonPressed(MouseButton.Left))
    {
        leftPressPosition = Raylib.GetMousePosition();
    }
    bool leftClicked = Raylib.IsMouseButtonReleased(MouseButton.Left)
        && Vector2.Distance(Raylib.GetMousePosition(), leftPressPosition) <= MaxClickMovement;

    if (shipMode)
    {
        // C switches between the cockpit view and the chase view behind the ship.
        if (Raylib.IsKeyPressed(KeyboardKey.C))
        {
            ship.ChaseView = !ship.ChaseView;
        }

        // P switches the autopilot on (towards the target) or off.
        if (Raylib.IsKeyPressed(KeyboardKey.P) && shipTarget != null)
        {
            if (ship.AutopilotOn)
            {
                ship.StopAutopilot();
            }
            else
            {
                autopilotStar = shipTarget;
                ship.StartAutopilot(StarRenderer.ToWorldPosition(autopilotStar));
            }
        }

        bool arrived = ship.Update();
        if (arrived && autopilotStar != null)
        {
            // The star reached becomes the current location, so the nearby-stars list is measured from it (press V to see it).
            nearbyPanel.SetLocation(autopilotStar);
            flightDestination = autopilotStar;
            arrivalMessage = $"Arrived at {autopilotStar.DisplayName}";
            arrivalMessageSeconds = 4f;
        }

        // T targets the star closest to the crosshair (within 15 degrees); clicking a star targets that star.
        // Choosing a new target switches the autopilot off, so it never flies somewhere the pilot did not choose.
        Star? newTarget = null;
        if (Raylib.IsKeyPressed(KeyboardKey.T))
        {
            newTarget = renderer.StarNearestDirection(ship.Position, ship.Forward, 15f);
        }
        if (leftClicked)
        {
            newTarget = renderer.StarAt(Raylib.GetMousePosition(), ship.ViewCamera) ?? newTarget;
        }
        if (newTarget != null && newTarget != shipTarget)
        {
            shipTarget = newTarget;
            ship.StopAutopilot();
        }
    }
    else
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
        if (leftClicked && !cameraController.IsFlying && !nearbyPanel.IsMouseOver())
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
    }

    // F switches between all stars and only the stars that have a proper name.
    if (Raylib.IsKeyPressed(KeyboardKey.F))
    {
        renderer.ShowOnlyNamedStars = !renderer.ShowOnlyNamedStars;
    }
    Camera3D camera = shipMode ? ship.ViewCamera : cameraController.Camera;
    // In the ship the dust box grows with speed, so the streaks stay visible at high speed.
    float dustScale = shipMode ? Math.Clamp(ship.Speed * 1.5f, 1f, 30f) : cameraController.Distance;
    dust.Update(camera, dustScale);

    Raylib.BeginDrawing();
    Raylib.ClearBackground(spaceColour);

    Raylib.BeginMode3D(camera);
    Raylib.DrawGrid(20, 2f); // 2-parsec grid on the plane of the celestial equator, to help judge depth
    renderer.DrawStars();
    dust.Draw(camera, dustScale); // faint particles that streak past when the camera moves
    if (shipMode && ship.ChaseView)
    {
        ShipModel.Draw(ship); // the ship is only visible from outside it
    }
    Raylib.EndMode3D();

    renderer.DrawLabels(camera); // star names are 2D text, drawn after the 3D scene

    // Dark panels behind the text, so star labels underneath do not make it unreadable.
    var panelColour = new Color(0, 0, 0, 225);
    Raylib.DrawRectangle(0, 0, 520, 126, panelColour);
    string help = shipMode ? ShipHelp : Help;
    int helpWidth = help.Split('\n').Max(line => Raylib.MeasureText(line, 16)) + 20; // fits the longest line
    Raylib.DrawRectangle(0, Raylib.GetScreenHeight() - 50, helpWidth, 50, panelColour);
    Raylib.DrawText(title, 10, 10, 20, Color.RayWhite);
    Raylib.DrawFPS(10, 36);
    Raylib.DrawText($"Camera is {camera.Position.Length() * Star.LightYearsPerParsec:F1} light years from the Sun", 10, 62, 16, Color.LightGray);
    string filterText = renderer.ShowOnlyNamedStars
        ? $"Filter ON: showing {renderer.VisibleStarCount} named stars (F to show all)"
        : $"Showing all {renderer.VisibleStarCount} stars (F to show only named stars)";
    Raylib.DrawText(filterText, 10, 82, 16, renderer.ShowOnlyNamedStars ? Color.Yellow : Color.LightGray);
    string modeText = shipMode
        ? $"SHIP: {ShipController.DescribeSpeed(ship.Speed)}{(ship.AutopilotOn ? "  (autopilot)" : "")}  [{(ship.ChaseView ? "chase" : "cockpit")} view]"
        : "Press V to fly the spaceship";
    Raylib.DrawText(modeText, 10, 104, 16, shipMode ? Color.SkyBlue : Color.Gray);

    if (shipMode && arrivalMessageSeconds > 0f)
    {
        arrivalMessageSeconds -= Raylib.GetFrameTime();
        Raylib.DrawText(arrivalMessage, (Raylib.GetScreenWidth() - Raylib.MeasureText(arrivalMessage, 30)) / 2, 120, 30, Color.Green);
    }
    if (!shipMode && cameraController.IsFlying)
    {
        string flying = $"Flying to {flightDestination.DisplayName}...";
        Raylib.DrawText(flying, (Raylib.GetScreenWidth() - Raylib.MeasureText(flying, 30)) / 2, 120, 30, Color.SkyBlue);
    }
    Raylib.DrawText(help, 10, Raylib.GetScreenHeight() - 44, 16, Color.Gray);
    renderer.DrawLegend();
    if (shipMode)
    {
        FlightHud.Draw(ship, camera, shipTarget); // crosshair, target marker and flight panel (in place of the list)
    }
    else
    {
        nearbyPanel.Draw(); // the list is measured from the last star visited, so it is hidden while flying the ship
    }
    Raylib.EndDrawing();
}

Raylib.CloseWindow();
