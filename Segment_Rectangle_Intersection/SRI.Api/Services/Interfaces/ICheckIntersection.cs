using SRI.Common;

namespace SRI.Api.Services.Interfaces;

public interface ICheckIntersection
{
    bool IsLineIntersectingLineSegment(Point p1, Point p2, Point q1, Point q2);
}