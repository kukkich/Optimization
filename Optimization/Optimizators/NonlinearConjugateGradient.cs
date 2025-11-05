using Optimization.Functionals;
using Optimization.Functions;
using Optimization.Optimizators.OneDimensionSearch;

namespace Optimization.Optimizators;

public class NonlinearConjugateGradient : IOptimizator
{
    private readonly IOneDimensionSearch _oneDimensionSearch;
    private readonly double _precision;
    private readonly double _maxIterations;

    public NonlinearConjugateGradient(IOneDimensionSearch oneDimensionSearch, double precision = 1e-3, int maxIterations = 1000)
    {
        _oneDimensionSearch = oneDimensionSearch;
        _precision = precision;
        _maxIterations = maxIterations;
    }

    public IVector Minimize(IFunctional objective, IParametricFunction function, IVector initialParameters,
        IVector minimumParameters = default, IVector maximumParameters = default)
    {
        if (objective is IDifferentiableFunctional differentiableObjective)
        {
            var currentParameters = initialParameters.Clone();
            var currentGradient = differentiableObjective.Gradient(function.Bind(initialParameters));
            var currentDirection = currentGradient.Clone().Negate();

            for (var i = 0; i < _maxIterations && currentDirection.Norm() < _precision; i++)
            {
                var lambda = _oneDimensionSearch.SearchMin(l => differentiableObjective.Value(function.Bind(currentParameters.Subtract(currentDirection.Multiply(l)))));

                var nextParameters = currentParameters.Sum(currentDirection.Multiply(lambda));
                var nextGradient = differentiableObjective.Gradient(function.Bind(nextParameters));

                var omega = nextGradient.ScalarProduct() / currentGradient.ScalarProduct();

                var nextDirection = nextGradient.Negate().Sum(currentDirection.Multiply(omega));

                currentParameters = nextParameters;
                currentGradient = nextDirection;
                currentDirection = nextDirection;
            }

            return currentParameters;
        }

        throw new ArgumentException();
    }
}