using Optimization.Functions.Implementations;

namespace Optimization.Functions.Factories;

public sealed class PiecewiseLinearParametricFunction : IParametricFunction
{
    private readonly double[] _x;
    
    public PiecewiseLinearParametricFunction(IEnumerable<double> knots)
    {
        if (knots == null) throw new ArgumentNullException(nameof(knots));
        _x = knots.ToArray();
        if (_x.Length < 2) throw new ArgumentException("At least two knots are required.");
        for (int i = 1; i < _x.Length; i++)
            if (!(_x[i] > _x[i - 1]))
                throw new ArgumentException("Knots must be strictly increasing.");
    }
    
    public IFunction Bind(IVector parameters)
    {
        if (parameters == null) throw new ArgumentNullException(nameof(parameters));
        if (parameters.Count != _x.Length)
            throw new ArgumentException($"Expected {_x.Length} parameters (function values at knots).");

        var y = parameters.ToArray();
        return new BoundPiecewiseLinearFunction(_x, y);
    }
}