using Raylib_cs;

Raylib.InitWindow(800, 450, "StarChart3D - setup test");
Raylib.SetTargetFPS(60);

while (!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.Black);
    Raylib.DrawText("Raylib-cs is working on macOS", 20, 20, 24, Color.White);
    Raylib.EndDrawing();
}

Raylib.CloseWindow();