using Optimization.Functionals;
using Optimization.Functions;
using Optimization.Optimizators.OneDimensionSearch;

namespace Optimization.Optimizators;

public class NonlinearConjugateGradientMethod : IOptimizator
{
    private readonly IOneDimensionSearchMethod _oneDimensionSearchMethod;
    private readonly double _precision;

    public NonlinearConjugateGradientMethod(IOneDimensionSearchMethod oneDimensionSearchMethod, double precision = 1e-3)
    {
        _oneDimensionSearchMethod = oneDimensionSearchMethod;
        _precision = precision;
    }

    public IVector Minimize(IFunctional objective, IParametricFunction function, IVector initialParameters,
        IVector minimumParameters = default, IVector maximumParameters = default)
    {
        if (objective is IDifferentiableFunctional differentiableObjective)
        {
            var currentParameters = initialParameters.Clone();
            var currentGradient = differentiableObjective.Gradient(function.Bind(initialParameters));
            var currentDirection = currentGradient.Clone().Negate();

            do
            {
                var lambda = _oneDimensionSearchMethod.SearchMin(l => differentiableObjective.Value(function.Bind(currentParameters.Subtract(currentDirection.Multiply(l)))));

                var nextParameters = currentParameters.Sum(currentDirection.Multiply(lambda));
                var nextGradient = differentiableObjective.Gradient(function.Bind(nextParameters));

                var omega = nextGradient.ScalarProduct() / currentGradient.ScalarProduct();

                var nextDirection = nextGradient.Negate().Sum(currentDirection.Multiply(omega));

                currentParameters = nextParameters;
                currentGradient = nextDirection;
                currentDirection = nextDirection;
            } while (currentDirection.Norm() < _precision);

            return currentParameters;
        }

        throw new ArgumentException();
    }
}