namespace SRI.Common;

public class Rectangle(double xMin, double yMin, double xMax, double yMax)
{
    public int Id;
    
    public double XMin = xMin;
    public double YMin = yMin;
    public double XMax = xMax;
    public double YMax = yMax;

    public Point TopLeft => new Point(XMin, YMax);
    public Point TopRight => new Point(XMax, YMax);
    public Point BottomLeft => new Point(XMin, YMin);
    public Point BottomRight => new Point(XMax, YMin);
}