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

        var xValue = point[0];
        var knotCount = _knots.Count;

        if (xValue <= _knots[0] || xValue >= _knots[knotCount - 1])
        {
            throw new ArgumentOutOfRangeException(nameof(point), "Gradient is not defined out of domain of definition (x0, xN)");
        }

        var segmentIndex = SegmentIndexFinder.Find(_knots, xValue);
        if (xValue.EqualsWithPrecision(_knots[segmentIndex]) || xValue.EqualsWithPrecision(_knots[segmentIndex + 1]))
        {
            throw new InvalidOperationException("Gradient is not defined in knots.");
        }

        return new Vector(1) { _segmentSlopes[segmentIndex] };
    }
}