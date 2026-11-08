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
    public static Course Between(Star from, Star to)
    {
        double dx = to.X - from.X;
        double dy = to.Y - from.Y;
        double dz = to.Z - from.Z;

        double distanceParsecs = Math.Sqrt(dx * dx + dy * dy + dz * dz);

        // Atan2 gets the quarter of the circle right and does not divide by zero.
        double bearing = Math.Atan2(dy, dx) * 180.0 / Math.PI;
        if (bearing < 0)
        {
            bearing += 360.0;
        }

        double path = Math.Atan2(dz, Math.Sqrt(dx * dx + dy * dy)) * 180.0 / Math.PI;

        return new Course(to, distanceParsecs * Star.LightYearsPerParsec, bearing, path);
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
