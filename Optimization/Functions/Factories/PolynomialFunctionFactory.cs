using Optimization.Functions.Implementations;

namespace Optimization.Functions.Factories;

public sealed class PolynomialFunctionFactory : IParametricFunction
{
    public IFunction Bind(IVector parameters)
    {
        if (parameters.Count < 1)
        {
            throw new ArgumentException($"Expected at least 1 coefficient; got {parameters.Count}.", nameof(parameters));
        }
        
        return new PolynomialFunction(parameters);
    }
}