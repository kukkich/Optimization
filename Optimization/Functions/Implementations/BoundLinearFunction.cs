namespace Optimization.Functions.Implementations;

public class BoundLinearFunction(IVector parameters) : IDifferentiableFunction
{
    private readonly int _dimension = parameters.Count - 1;
    public double Value(IVector point)
    {
        ArgumentNullException.ThrowIfNull(point);
        if (point.Count != _dimension)
        {
            throw new ArgumentException($"Point dimension must be {_dimension}.", nameof(point));
        }

        var sum = parameters[_dimension];
        for (var i = 0; i < _dimension; i++)
        {
            sum += parameters[i] * point[i];
        }

        return sum;
    }

    public IVector Gradient(IVector point)
    {
        if (point.Count != _dimension)
        {
            throw new ArgumentException($"Point dimension must be {_dimension}.", nameof(point));
        }

        var g = new Vector(_dimension);
        for (var i = 0; i < _dimension; i++)
        {
            g.Add(parameters[i]);
        }
            
        return g;
    }
}