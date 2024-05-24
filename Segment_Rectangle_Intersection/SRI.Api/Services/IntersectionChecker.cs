using SRI.Api.Services.Interfaces;
using SRI.Common;

namespace SRI.Api.Services;

public class IntersectionChecker : ICheckIntersection
{
    public bool IsLineIntersectingLineSegment(Point p1, Point p2, Point q1, Point q2)
    {
        // Parametric line representation for segment p1-p2
        var denominator = (p2.X - p1.X) * (q2.Y - q1.Y) - (p2.Y - p1.Y) * (q2.X - q1.X);

        // Lines are parallel (denominator is 0)
        if (denominator == 0)
        {
            return false;
        }

        var t = ((q2.X - q1.X) * (p1.Y - q1.Y) - (q2.Y - q1.Y) * (p1.X - q1.X)) / denominator;
        var u = ((p2.X - p1.X) * (p1.Y - q1.Y) - (p2.Y - p1.Y) * (p1.X - q1.X)) / denominator;

        // Check if intersection point lies on both lines (0 <= t,u <= 1)
        return t is >= 0 and <= 1 && u is >= 0 and <= 1;
    }
}