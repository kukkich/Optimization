using Optimization.Functions;

namespace Optimization.Functionals;

public interface ILeastSquaresFunctional : IFunctional
{
    IVector Residual(IFunction function);
    IMatrix Jacobian(IFunction function);
}