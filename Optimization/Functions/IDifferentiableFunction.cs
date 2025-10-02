namespace Optimization.Functions;

public interface IDifferentiableFunction : IFunction
{
    IVector Gradient(IVector point);
}