using Optimization.Functionals;
using Optimization.Functions;

namespace Optimization.Optimizators;

public interface IOptimizator
{
    IVector Minimize
    (
        IFunctional objective,
        IParametricFunction function,
        IVector initialParameters,
        IVector minimumParameters = default,
        IVector maximumParameters = default
    );
}