using Optimization.Functions.Implementations;

namespace Optimization.Functions.Factories;

public sealed class LinearParametricFunction : IParametricFunction
{
    public IFunction Bind(IVector parameters)
    {
        if (parameters.Count < 2)
        {
            throw new ArgumentException($"Expected at least 2 parameters: weights + bias; got {parameters.Count}.", nameof(parameters));
        }

        return new BoundLinearFunction(parameters);
    }
}