namespace Optimization.Functions;

public interface IParametricFunction
{
    IFunction Bind(IVector parameters);
}