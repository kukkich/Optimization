using Optimization.Functionals;
using Optimization.Functions;
using Optimization.Optimizators.EquationSystem;
using Optimization.Optimizators.OneDimensionSearch;

namespace Optimization.Optimizators;

public class GaussNewton : IOptimizator
{
    private readonly ISLAESolver _slaeSolver;
    private readonly double _precision;
    private readonly double _maxIterations;

    public GaussNewton(ISLAESolver slaeSolver, double precision = 1e-3, int maxIterations = 1000)
    {
        _slaeSolver = slaeSolver;
        _precision = precision;
        _maxIterations = maxIterations;
    }

    public IVector Minimize(IFunctional objective, IParametricFunction function, IVector initialParameters,
        IVector minimumParameters = default, IVector maximumParameters = default)
    {
        if (objective is ILeastSquaresFunctional leastSquaresObjective)
        {
            var parameters = initialParameters.Clone();

            for (var i = 0; i < _maxIterations; i++)
            {
                var bindedFunction = function.Bind(parameters);
                var residuals = leastSquaresObjective.Residual(bindedFunction);
                var jacobian = leastSquaresObjective.Jacobian(bindedFunction);

                var leftPart = jacobian.MultiplyTransposeOn();
                var rightPart = jacobian.MultiplyTransposeOn(residuals).Negate();

                var deltaParameters = _slaeSolver.Solve(leftPart, rightPart);

                parameters = parameters.Sum(deltaParameters);

                if (deltaParameters.Norm() < _precision)
                {
                    return parameters;
                }
            }
        }

        throw new ArgumentException();
    }
}