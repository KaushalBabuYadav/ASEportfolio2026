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

// Catalogue names for stars without a proper name (one example of each kind).
int[] exampleIds = { 8087, 15419, 27844, 103879, 103883, 7736, 118084, 118541, 82472, 70666, 0 };
foreach (int id in exampleIds)
{
    Star? star = stars.FindById(id);
    if (star is not null)
    {
        Console.WriteLine($"  HYG {star.Id}: {star.DisplayName} (catalogue name: {star.CatalogueName})");
    }
}

// Every star should now have its own name.
int duplicateNames = stars.GroupBy(star => star.DisplayName).Count(group => group.Count() > 1);
Console.WriteLine($"Names used by more than one star: {duplicateNames}");
Console.WriteLine($"Unnamed stars that still only have a HYG number: {stars.Count(star => !star.HasProperName && star.CatalogueName.StartsWith("HYG"))}");
