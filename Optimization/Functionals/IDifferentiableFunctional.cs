using Optimization.Functions;

namespace Optimization.Functionals;

public interface IDifferentiableFunctional : IFunctional
{
    IVector Gradient(IFunction function);
}