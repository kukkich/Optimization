namespace Optimization.Functions;

public interface IDifferentiableFunction
{
    IVector Gradient(IVector point);
}