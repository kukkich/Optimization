namespace Optimization.Functions.Implementations;

public class PolynomialFunction(IVector parameters) : IFunction
{
    public double Value(IVector point)
    {
        if (point.Count != 1)
        {
            throw new ArgumentException("Polynomial expects 1D point (x).");
        }
        
        var xValue = point[0];
        var result = parameters[0];
        for (var i = 1; i < parameters.Count; i++)
        {
            result = result * xValue + parameters[i];
        }

        return result;
    }
}