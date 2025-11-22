using Optimization.Functions.Utils;

namespace Optimization.Functions.Implementations;

using Extensions;

public class PiecewiseLinearFunction : IDifferentiableFunction
{
    private readonly IVector _knots;
    private readonly IVector _values;
    private readonly IVector _segmentSlopes;
    
    public PiecewiseLinearFunction(IVector knots, IVector values)
    {
        var knotCount = knots.Count;
        var segmentSlopes = new Vector(knotCount - 1);
        for (var i = 0; i < knotCount - 1; i++)
        {
            var deltaX = knots[i + 1] - knots[i];
            segmentSlopes.Add((values[i + 1] - values[i]) / deltaX);
        }

        _knots = knots;
        _values = values;
        _segmentSlopes = segmentSlopes;
    }

    public double Value(IVector point)
    {
        if (point.Count != 1)
        {
            throw new ArgumentException("Point must be one-dimensional", nameof(point));
        }

        var xValue = point[0];
        var knotCount = _knots.Count;

        if (xValue < _knots[0] || xValue > _knots[knotCount - 1])
        {
            throw new ArgumentOutOfRangeException(nameof(point), "Point is out of domain of definition [x0, xN]");
        }

        if (xValue.EqualsWithPrecision(_knots[knotCount - 1]))
        {
            return _values[knotCount - 1];
        }

        var segmentIndex = SegmentIndexFinder.Find(_knots, xValue);
        return _values[segmentIndex] + _segmentSlopes[segmentIndex] * (xValue - _knots[segmentIndex]);
    }

    public IVector Gradient(IVector point)
    {
        if (point.Count != 1)
        {
            throw new ArgumentException("Point must be one-dimensional.", nameof(point));
        }

        var x = point[0];
        var n = _knots.Count;

        if (x < _knots[0] || x > _knots[n - 1])
            throw new ArgumentOutOfRangeException(nameof(point), "Point is out of domain [x0, xN].");

        var grad = new Vector(n);
        for (var k = 0; k < n; k++)
        {
            grad.Add(0.0);
        }
        
        if (x.EqualsWithPrecision(_knots[n - 1]))
        {
            grad[n - 1] = 1.0;
            return grad;
        }

        var i = SegmentIndexFinder.Find(_knots, x);

        var h = _knots[i + 1] - _knots[i];
        var t = (x - _knots[i]) / h;

        grad[i] = 1.0 - t;
        grad[i + 1] = t;

        return grad;
    }
}