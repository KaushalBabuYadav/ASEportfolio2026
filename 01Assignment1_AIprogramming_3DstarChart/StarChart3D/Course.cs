namespace StarChart3D;

/// <summary>
/// The course from one star to another: distance, bearing and path.
/// </summary>
/// <remarks>
/// "Bearing" and "path" are not official astronomy terms. Following the domain expert's advice
/// (AI_domainExpert_log.md, Entry 2), they are measured in the equatorial frame of the HYG data,
/// so the angles do not change as you travel:
/// <list type="bullet">
/// <item><b>Bearing</b>: 0–360°, measured in the equatorial plane anticlockwise from +X (vernal equinox) towards +Y.</item>
/// <item><b>Path</b>: −90° to +90°, the angle above (+, towards celestial north) or below the equatorial plane.</item>
/// </list>
/// From the Sun, bearing equals the destination's right ascension in degrees and path equals its declination.
/// </remarks>
/// <param name="Destination">The star being travelled to.</param>
/// <param name="DistanceLightYears">Straight-line distance in light years.</param>
/// <param name="BearingDegrees">Horizontal angle, 0 to 360.</param>
/// <param name="PathDegrees">Vertical angle, −90 to +90.</param>
public readonly record struct Course(Star Destination, double DistanceLightYears, double BearingDegrees, double PathDegrees)
{
    /// <summary>Works out the course from <paramref name="from"/> to <paramref name="to"/> using their HYG x, y, z (parsecs).</summary>
    public static Course Between(Star from, Star to) => FromPoint(from.X, from.Y, from.Z, to);

    /// <summary>
    /// Works out the course from any point (HYG x, y, z in parsecs, e.g. the spaceship's position) to the star <paramref name="to"/>.
    /// </summary>
    public static Course FromPoint(double x, double y, double z, Star to)
    {
        double dx = to.X - x;
        double dy = to.Y - y;
        double dz = to.Z - z;

        double distanceParsecs = Math.Sqrt(dx * dx + dy * dy + dz * dz);
        (double bearing, double path) = AnglesOf(dx, dy, dz);
        return new Course(to, distanceParsecs * Star.LightYearsPerParsec, bearing, path);
    }

    /// <summary>
    /// Returns the bearing (0–360°) and path (−90° to +90°) of a direction given in HYG axes.
    /// Used both for the course to a star and for the direction the spaceship is pointing.
    /// </summary>
    public static (double Bearing, double Path) AnglesOf(double dx, double dy, double dz)
    {
        // Atan2 gets the quarter of the circle right and does not divide by zero.
        double bearing = Math.Atan2(dy, dx) * 180.0 / Math.PI;
        if (bearing < 0)
        {
            bearing += 360.0;
        }

        double path = Math.Atan2(dz, Math.Sqrt(dx * dx + dy * dy)) * 180.0 / Math.PI;
        return (bearing, path);
    }

    /// <summary>
    /// Returns the courses from <paramref name="from"/> to its <paramref name="count"/> nearest stars,
    /// nearest first. The starting star itself is not included.
    /// </summary>
    public static IReadOnlyList<Course> ToNearest(Star from, IEnumerable<Star> stars, int count)
    {
        return stars
            .Where(star => star.Id != from.Id)
            .Select(star => Between(from, star))
            .OrderBy(course => course.DistanceLightYears)
            .Take(count)
            .ToList();
    }
}
