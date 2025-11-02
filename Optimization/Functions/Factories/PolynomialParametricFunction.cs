using Optimization.Functions.Implementations;

namespace Optimization.Functions.Factories;

public sealed class PolynomialParametricFunction : IParametricFunction
{
    public IFunction Bind(IVector parameters)
    {
        if (parameters.Count < 1)
        {
            throw new ArgumentException($"Expected at least 2 parameters: weights + bias; got {parameters.Count}.", nameof(parameters));
        }
        
        return new BoundPolynomialFunction(parameters);
    }
}