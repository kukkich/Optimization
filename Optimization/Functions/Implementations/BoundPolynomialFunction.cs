namespace Optimization.Functions.Implementations;

public class BoundPolynomialFunction(IVector parameters) : IFunction
{
    public double Value(IVector point)
    {
        ArgumentNullException.ThrowIfNull(point);
        if (point.Count != 1)
        {
            throw new ArgumentException("Polynomial expects 1D point (x).");
        }
        
        var x = point[0];
        var res = 0.0;
        for (var i = parameters.Count - 1; i >= 0; i--)
        {
            res = res * x + parameters[i];
        }
            
        return res;
    }
}