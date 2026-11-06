using StarChart3D;

// Temporary test for StarCollection: loads the star data and prints a summary.
// (The Raylib window test from setup is replaced; the 3D window comes back in Feature 2.)
string dataFile = Path.Combine(AppContext.BaseDirectory, "Data", "stars_local_2000.csv");
var stars = new StarCollection(dataFile);

Console.WriteLine($"Loaded {stars.Count} stars from {dataFile}");
Console.WriteLine($"Skipped lines: {stars.SkippedLines.Count}");
foreach (string message in stars.SkippedLines)
{
    Console.WriteLine("  " + message);
}

Console.WriteLine($"First star: {stars[0]}");
Console.WriteLine($"Second star: {stars[1]}");
Console.WriteLine($"Last star: {stars[stars.Count - 1]}");
Console.WriteLine($"Stars with a proper name: {stars.Count(star => star.HasProperName)}");

Star? tauCeti = stars.FindById(8087);
Console.WriteLine($"Star 8087: {tauCeti}, Bayer={tauCeti?.Bayer}, Flamsteed={tauCeti?.Flamsteed}, Constellation={tauCeti?.Constellation}");
