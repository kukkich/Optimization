namespace Optimization.Functions.Implementations;

using Extensions;

public class PiecewiseLinearFunction(IVector x, IVector y) : IDifferentiableFunction
{
    public double Value(IVector point)
    {
        if (point.Count != 1)
        {
            throw new ArgumentException("Point must be one-dimensional", nameof(point));
        }

        var xv = point[0];
        var n = x.Count;

        if (xv < x[0] || xv > x[n - 1])
        {
            throw new ArgumentOutOfRangeException(nameof(point), "Point is out of domain of definition [x0, xN].");
        }

        if (xv.EqualsWithPrecision(x[n - 1]))
        {
            return y[n - 1];
        }

        var segment = LocateSegment(xv);
        var segmentLength = x[segment + 1] - x[segment];
        var alpha = (xv - x[segment]) / segmentLength;
        return (1.0 - alpha) * y[segment] + alpha * y[segment + 1];
    }

    public IVector Gradient(IVector point)
    {
        if (point.Count != 1)
        {
            throw new ArgumentException("Point must be one-dimensional.", nameof(point));
        }

        var xv = point[0];
        var n = x.Count;

        if (xv <= x[0] || xv >= x[n - 1])
        {
            throw new ArgumentOutOfRangeException(nameof(point), "Gradient is not defined out of domain of definition (x0, xN)");
        }

        var seg = LocateSegment(xv);
        if (xv.EqualsWithPrecision(x[seg]))
        {
            throw new InvalidOperationException("Gradient is not defined in knots.");
        }

        var slope = (y[seg + 1] - y[seg]) / (x[seg + 1] - x[seg]);
        var g = new Vector(1)
        {
            [0] = slope
        };
        return g;
    }
    
    private int LocateSegment(double xv)
    {
        var n = x.Count;
        var low = 0;
        var high = n - 2;

        while (low <= high)
        {
            var mid = (low + high) >> 1;
            if (xv < x[mid])
            {
                high = mid - 1;
            }
            else if (xv >= x[mid + 1])
            {
                low = mid + 1;
            }
            else
            {
                return mid;
            }
        }

        return Math.Max(0, Math.Min(n - 2, low));
    }
}