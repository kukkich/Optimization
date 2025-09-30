using Optimization.Functions;

namespace Optimization.Functionals;

public interface IFunctional
{
    double Value(IFunction function);
}